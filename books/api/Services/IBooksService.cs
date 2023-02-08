using Books.Api.Dtos;
using Books.Api.Dtos.External;

namespace Books.Api.Services;

public interface IBooksService
{
    IEnumerable<BookDto> GetBooks();
    Task<IEnumerable<BookDto>> GetBooksAsync();
    Task<IEnumerable<BookDto>> GetBooksAsync(IEnumerable<Guid> bookIds);
    BookDto GetBookById(Guid bookId);
    Task<BookDto> GetBookByIdAsync(Guid bookId);
    IAsyncEnumerable<BookDto> GetBooksAsAsyncEnumerable();
    (Guid, BookForCreationDto) CreateBook(BookForCreationDto bookDto);    
    Task<(Guid, BookForCreationDto)> CreateBookAsync(BookForCreationDto bookDto);
    Task<(string, IEnumerable<BookDto>)> CreateBooksAsync(IEnumerable<BookForCreationDto> bookDtos);
    void UpdateBook(Guid bookId, BookForUpdateDto bookDto);
    Task UpdateBookAsync(Guid bookId, BookForUpdateDto bookDto);
    Task UpdateBooksAsync(IEnumerable<BookForBulkUpdateDto> bookDtos);
    void DeleteBook(Guid bookId);
    Task DeleteBookAsync(Guid bookId);
    Task DeleteBooksAsync(IEnumerable<Guid> bookIds);

    // External
    Task<CoverDto?> GetBookCoverAsync(string coverId);
}