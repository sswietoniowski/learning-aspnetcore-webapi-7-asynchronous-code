using Books.Api.Dtos;

namespace Books.Api.Services;

public interface IBooksService
{
    IEnumerable<BookDto> GetBooks();
    Task<IEnumerable<BookDto>> GetBooksAsync();
    BookDto GetBookById(Guid bookId);
    Task<BookDto> GetBookByIdAsync(Guid bookId);
    (Guid, BookForCreationDto) CreateBook(BookForCreationDto bookDto);    
    Task<(Guid, BookForCreationDto)> CreateBookAsync(BookForCreationDto bookDto);
    Task CreateBooksAsync(IEnumerable<BookForCreationDto> bookDtos);
    void UpdateBook(Guid bookId, BookForUpdateDto bookDto);
    Task UpdateBookAsync(Guid bookId, BookForUpdateDto bookDto);
    Task UpdateBooksAsync(IEnumerable<(Guid, BookForUpdateDto)> bookDtos);
    void DeleteBook(Guid bookId);
    Task DeleteBookAsync(Guid bookId);
    Task DeleteBooksAsync(IEnumerable<Guid> bookIds);
}