using Books.Api.Configurations.Middleware.Filters;
using Books.Api.Dtos;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controller;

[ApiController]
[Route("api/books/async")]
public class BooksAsyncController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksAsyncController(IBooksService booksService)
    {
        _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        var booksDto = await _booksService.GetBooksAsync();

        return Ok(booksDto);
    }

    [HttpGet("{bookId:guid}", Name = "GetBookAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // we could use [BookResultFilter] here, except that we want to DI the logger into the filter
    // that is why we are using syntax below
    [TypeFilter(typeof(BookResultFilter))]
    public async Task<ActionResult<BookDto>> GetBook(Guid bookId)
    {
        var bookDto = await _booksService.GetBookByIdAsync(bookId);

        return Ok(bookDto);
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
    public async Task<ActionResult> DeleteBook(Guid bookId)
    {
        await _booksService.DeleteBookAsync(bookId);

        return NoContent();
    }
}