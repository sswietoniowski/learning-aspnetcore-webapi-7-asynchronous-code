using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using AutoMapper;
using Books.Api.Configurations.Exceptions;
using Books.Api.Dtos;
using Books.Api.Dtos.External;
using Books.Api.Entities;

namespace Books.Api.Services;

public class BooksService : IBooksService
{
    private readonly IBooksRepository _repository;
    private readonly IMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BooksService> _logger;

    public BooksService(IBooksRepository repository, IMapper mapper, 
        IHttpClientFactory httpClientFactory, IConfiguration configuration,
        ILogger<BooksService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<BookDto> GetBooks()
    {
        var books = _repository.GetBooks();

        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<IEnumerable<BookDto>> GetBooksAsync()
    {
        var books = await _repository.GetBooksAsync();

        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<IEnumerable<BookDto>> GetBooksAsync(IEnumerable<Guid> bookIds)
    {
        var books = await _repository.GetBooksAsync(bookIds);

        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public BookDto GetBookById(Guid bookId)
    {
        var book = _repository.GetBookById(bookId);

        if (book is null)
        {
            throw new NotFoundApiException(nameof(BookDto), bookId.ToString());
        }

        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto> GetBookByIdAsync(Guid bookId)
    {
        var book = await _repository.GetBookByIdAsync(bookId);

        if (book is null)
        {
            throw new NotFoundApiException(nameof(BookDto), bookId.ToString());
        }

        return _mapper.Map<BookDto>(book);
    }

    public async IAsyncEnumerable<BookDto> GetBooksAsAsyncEnumerable()
    {
        await foreach (var book in _repository.GetBooksAsAsyncEnumerable())
        {
            yield return _mapper.Map<BookDto>(book);
        }
    }

    public (Guid, BookForCreationDto) CreateBook(BookForCreationDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        _repository.CreateBook(book);
        _repository.SaveChanges();

        return (book.Id, _mapper.Map<BookForCreationDto>(book));
    }

    public async Task<(Guid, BookForCreationDto)> CreateBookAsync(BookForCreationDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        _repository.CreateBook(book);
        await _repository.SaveChangesAsync();

        return (book.Id, _mapper.Map<BookForCreationDto>(book));
    }

    public async Task<(string, IEnumerable<BookDto>)> CreateBooksAsync(IEnumerable<BookForCreationDto> bookDtos)
    {
        var books = new List<Book>();

        foreach (var bookDto in bookDtos)
        {
            var book = _mapper.Map<Book>(bookDto);
            books.Add(book);
            _repository.CreateBook(book);
        }

        await _repository.SaveChangesAsync();

        var booksToReturn = _mapper.Map<IEnumerable<BookDto>>(books);
        var bookIds = string.Join(",", booksToReturn.Select(b => b.Id));

        return (bookIds, booksToReturn);
    }

    public void UpdateBook(Guid bookId, BookForUpdateDto bookDto)
    {
        var book = _repository.GetBookById(bookId);

        if (book is null)
        {
            throw new NotFoundApiException(nameof(BookDto), bookId.ToString());
        }

        _mapper.Map(bookDto, book);

        _repository.UpdateBook(book);

        _repository.SaveChanges();
    }

    public async Task UpdateBookAsync(Guid bookId, BookForUpdateDto bookDto)
    {
        var book = await _repository.GetBookByIdAsync(bookId);

        if (book is null)
        {
            throw new NotFoundApiException(nameof(BookDto), bookId.ToString());
        }

        _mapper.Map(bookDto, book);

        _repository.UpdateBook(book);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateBooksAsync(IEnumerable<BookForBulkUpdateDto> bookDtos)
    {
        foreach (var bookDto in bookDtos)
        {
            var book = await _repository.GetBookByIdAsync(bookDto.Id);

            if (book is null)
            {
                throw new NotFoundApiException(nameof(BookDto), bookDto.Id.ToString());
            }

            _mapper.Map(bookDto, book);

            _repository.UpdateBook(book);            
        }

        await _repository.SaveChangesAsync();
    }

    public void DeleteBook(Guid bookId)
    {
        var book = _repository.GetBookById(bookId);

        if (book is null)
        {
            throw new NotFoundApiException(nameof(BookDto), bookId.ToString());
        }

        _repository.DeleteBook(book);
        _repository.SaveChanges();
    }

    public async Task DeleteBookAsync(Guid bookId)
    {
        var book = await _repository.GetBookByIdAsync(bookId);

        if (book is null)
        {
            throw new NotFoundApiException(nameof(BookDto), bookId.ToString());
        }

        await _repository.DeleteBookAsync(book);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteBooksAsync(IEnumerable<Guid> bookIds)
    {
        foreach (var bookId in bookIds)
        {
            var book = await _repository.GetBookByIdAsync(bookId);

            if (book is null)
            {
                throw new NotFoundApiException(nameof(BookDto), bookId.ToString());
            }

            await _repository.DeleteBookAsync(book);
        }
        
        await _repository.SaveChangesAsync();
    }

    public async Task<CoverDto?> GetBookCoverAsync(Guid bookId)
    {        
        var httpClient = _httpClientFactory.CreateClient();

        var externalApiBaseUrl = _configuration["ExternalApiBaseUrl"];

        // dummy cover id
        var coverId = 1;

        var response = await httpClient.GetAsync($"{externalApiBaseUrl}/api/covers/{coverId}");

        if (!response.IsSuccessStatusCode || response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        var data = await response.Content.ReadAsStringAsync();
    
        return JsonSerializer.Deserialize<CoverDto>(
            data,
            new JsonSerializerOptions 
            { PropertyNameCaseInsensitive = true 
            }
        );
    }

    public async Task<IEnumerable<CoverDto>> GetBookCoversOneByOneAsync(Guid bookId)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var externalApiBaseUrl = _configuration["ExternalApiBaseUrl"];

        // dummy cover ids
        var coverIds = new List<int> { 1, 2, 3, 4, 5 };

        var covers = new List<CoverDto>();

        using var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        foreach (var coverId in coverIds)
        {
            _logger.LogInformation($"Getting cover with id {coverId} for book with id {bookId}.");

            var response = await httpClient.GetAsync($"{externalApiBaseUrl}/api/covers/{coverId}", cancellationToken);

            if (!response.IsSuccessStatusCode || response.StatusCode != HttpStatusCode.OK)
            {
                cancellationTokenSource.Cancel();
                break;
            }

            var data = await response.Content.ReadAsStringAsync(cancellationToken);

            var cover = JsonSerializer.Deserialize<CoverDto>(
                data,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (cover is null)
            {
                continue;
            }

            covers.Add(cover);

            _logger.LogInformation($"Got cover with id {coverId} for book with id {bookId}.");
        }

        return covers;
    }

    public async Task<IEnumerable<CoverDto>> GetBookCoversParallelAndWaitForAllAsync(Guid bookId)
        => await GetBookCoversParallelAndWaitForAllAsync(bookId, default(CancellationToken));

    public async Task<IEnumerable<CoverDto>> GetBookCoversParallelAndWaitForAllAsync(Guid bookId, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var externalApiBaseUrl = _configuration["ExternalApiBaseUrl"];

        // dummy cover ids
        var coverIds = new List<int> { 1, 2, 3, 4, 5 };

        // more info about ConcurrentBag can be found here: https://dotnetpattern.com/csharp-concurrentbag
        var covers = new ConcurrentBag<CoverDto>();

        var tasks = new List<Task>();

        foreach (var coverId in coverIds)
        {
            var task = Task.Run(async () => 
            {
                _logger.LogInformation($"Getting cover with id {coverId} for book with id {bookId}.");

                var response = await httpClient.GetAsync($"{externalApiBaseUrl}/api/covers/{coverId}", cancellationToken);

                if (!response.IsSuccessStatusCode || response.StatusCode != HttpStatusCode.OK)
                {                                            
                    return;
                }

                var data = await response.Content.ReadAsStringAsync(cancellationToken);

                var cover = JsonSerializer.Deserialize<CoverDto>(
                    data,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                if (cover is null)
                {
                    return;
                }

                covers.Add(cover);

                _logger.LogInformation($"Got cover with id {coverId} for book with id {bookId}.");
            }, cancellationToken);

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        return covers.ToList();
   }
}