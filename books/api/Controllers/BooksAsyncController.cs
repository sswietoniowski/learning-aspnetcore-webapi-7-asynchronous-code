using System.Diagnostics;
using Books.Api.Configurations.Middleware.Filters;
using Books.Api.Dtos;
using Books.Api.Dtos.External;
using Books.Api.Services;
using BooksApi.Services.Legacy;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controller;

[ApiController]
[Route("api/books/async")]
public class BooksAsyncController : ControllerBase
{
    private readonly IBooksService _booksService;
    private readonly IBooksPageCalculatorService _booksPageCalculatorService;
    private readonly ILogger<BooksAsyncController> _logger;

    public BooksAsyncController(IBooksService booksService, IBooksPageCalculatorService booksPageCalculatorService, ILogger<BooksAsyncController> logger)
    {
        _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));
        _booksPageCalculatorService = booksPageCalculatorService ?? throw new ArgumentNullException(nameof(booksPageCalculatorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        var booksDto = await _booksService.GetBooksAsync();

        return Ok(booksDto);
    }

    [HttpGet("stream")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async IAsyncEnumerable<BookDto> GetBooksAsAsyncEnumerable() 
    {
        const int DELAY_IN_SECONDS = 1;

        await foreach (var bookDto in _booksService.GetBooksAsAsyncEnumerable())
        {
            // we're adding this delay just to see that streaming is working
            await Task.Delay(TimeSpan.FromSeconds(DELAY_IN_SECONDS));

            yield return bookDto;
        }
    }

    [HttpGet("{bookId:guid}", Name = "GetBookAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // we could use [BookResultFilter] here, except that we want to DI the logger into the filter
    // that is why we are using syntax below
    [TypeFilter(typeof(BookResultFilter))]
    public async Task<ActionResult<BookDto>> GetBook(Guid bookId, bool retrieveBookCover = false, 
        bool retrieveAllBookCovers = false, RetrievingStrategy? retrievingStrategy = null)
    {
        var bookDto = await _booksService.GetBookByIdAsync(bookId);

        if (retrieveBookCover)
        {
            // here we are retrieving the cover for the book from an external API
            var coverDto = await _booksService.GetBookCoverAsync(bookId);

            if (coverDto != null)
            {
                bookDto.Cover = coverDto;
            }
        }

        var strategies = new Dictionary<RetrievingStrategy, Func<Guid, Task<IEnumerable<CoverDto>>>>()
        {
            { RetrievingStrategy.OneByOne, _booksService.GetBookCoversOneByOneAsync },
            { RetrievingStrategy.ParallelAndWaitForAll, _booksService.GetBookCoversParallelAndWaitForAllAsync }
        };

        retrievingStrategy ??= RetrievingStrategy.OneByOne;

        if (retrieveAllBookCovers && strategies.ContainsKey(retrievingStrategy!.Value))
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _logger.LogInformation("Starting to retrieve all covers for book {BookId} (process one by one)", bookId);

            // here we are retrieving the covers for all books from an external API using chosen strategy

            var strategy = strategies[retrievingStrategy.Value];

            var coversDto = await strategy.Invoke(bookId);

            stopwatch.Stop();

            var elapsed = stopwatch.Elapsed;

            _logger.LogInformation($"Time elapsed: {elapsed}");
            
            if (coversDto != null)
            {
                bookDto.AllCovers = coversDto;
            }
        }

        return Ok(bookDto);
    }

    [HttpGet("covers/{bookId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [TypeFilter(typeof(BookWithCoversResultFilter))]
    public async Task<ActionResult<(BookDto Book, IEnumerable<CoverDto> Covers)>> GetBookWithCovers(Guid bookId, CancellationToken cancellationToken)
    {
        var bookDto = await _booksService.GetBookByIdAsync(bookId);

        if (bookDto is null)
        {
            return NotFound();
        }

        var coversDto = await _booksService.GetBookCoversParallelAndWaitForAllAsync(bookId);

        coversDto ??= Enumerable.Empty<CoverDto>();

        // returning a value tuple

        // 1st syntax
        // (BookDto Book, IEnumerable<CoverDto> Covers) result = (bookDto, coversDto);
        // return Ok(result);

        // 2nd syntax
        return Ok((Book: bookDto, Covers: coversDto));
    }

    private Task<int> CalculatePageCount_BadCode(Guid bookId)
    {
        return Task.Run(() => {
            _logger.LogInformation($"ThreadId while calculating page count: {Thread.CurrentThread.ManagedThreadId}");

            return _booksPageCalculatorService.CalculatePageCount(bookId);
        });
    }

    [HttpGet("legacy/{bookId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BookWithPageCountDto>> GetBookWithPageCount(Guid bookId, CancellationToken cancellationToken)
    {
        var bookDto = await _booksService.GetBookByIdAsync(bookId);
        
        if (bookDto is null)
        {
            return NotFound();
        }

        // 1st approach - calling a legacy service that is not async and is blocking
        // var pageCount = _booksPageCalculatorService.CalculatePageCount(bookId);

        // 2nd approach - calling a legacy service that is not async using Task.Run
        // this is not bad per se, but it is not the best approach because it can
        // starve the thread pool
        // ASP.NET Core is optimized for async/await, but it is not optimized for Task.Run
        // Task.Run is optimized for CPU-bound tasks, using Task.Run on the server side
        // decreases scalability
        var pageCount = await CalculatePageCount_BadCode(bookId);                        

        _logger.LogInformation($"ThreadId when entering {nameof(GetBookWithPageCount)}: {Thread.CurrentThread.ManagedThreadId}");

        var bookWithPageCountDto = new BookWithPageCountDto(bookDto.Id, bookDto.Title, bookDto.Description, bookDto.AuthorId, bookDto.Author, pageCount);

        _logger.LogInformation($"ThreadId after calling {nameof(CalculatePageCount_BadCode)}: {Thread.CurrentThread.ManagedThreadId}");

        // while dealing with bad practices, we should use async/await all the way, so we should not use .Result or .Wait()
        // because they are blocking and can starve the thread pool

        // another problem could be related to shared state management

        return Ok(bookWithPageCountDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBook([FromBody] BookForCreationDto bookDto)
    {
        var (bookId, createdBookDto) = await _booksService.CreateBookAsync(bookDto);

        return CreatedAtAction(nameof(GetBook), new { bookId = bookId }, createdBookDto);
    }

    [HttpPut("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBook(Guid bookId, [FromBody] BookForUpdateDto bookDto)
    {
        await _booksService.UpdateBookAsync(bookId, bookDto);

        return NoContent();
    }

    [HttpDelete("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBook(Guid bookId)
    {
        await _booksService.DeleteBookAsync(bookId);

        return NoContent();
    }
}

public enum RetrievingStrategy
{
    OneByOne,
    ParallelAndWaitForAll
}
