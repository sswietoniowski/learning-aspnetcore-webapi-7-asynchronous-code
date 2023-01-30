using AutoMapper;
using Books.Api.Configurations.Exceptions;
using Books.Api.Dtos;
using Books.Api.Entities;

namespace Books.Api.Services;

public class BooksService : IBooksService
{
    private readonly IBooksRepository _repository;
    private readonly IMapper _mapper;
    
    public BooksService(IBooksRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

    public async Task CreateBooksAsync(IEnumerable<BookForCreationDto> bookDtos)
    {
        foreach (var bookDto in bookDtos)
        {
            var book = _mapper.Map<Book>(bookDto);
            _repository.CreateBook(book);
        }

        await _repository.SaveChangesAsync();
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

    public async Task UpdateBooksAsync(IEnumerable<(Guid, BookForUpdateDto)> bookDtos)
    {
        foreach (var (bookId, bookDto) in bookDtos)
        {
            var book = await _repository.GetBookByIdAsync(bookId);

            if (book is null)
            {
                throw new NotFoundApiException(nameof(BookDto), bookId.ToString());
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
}