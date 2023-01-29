using Books.Api.Dtos;

namespace Books.Api.Services;

public interface IBooksService
{
    IEnumerable<BookDto> GetBooks();
    Task<IEnumerable<BookDto>> GetBooksAsync();
    BookDto GetBookById(Guid bookId);
    Task<BookDto> GetBookByIdAsync(Guid bookId);
    BookDto CreateBook(BookForCreationDto bookDto);
    Task<BookDto> CreateBookAsync(BookForCreationDto bookDto);
    void UpdateBook(Guid bookId, BookForUpdateDto bookDto);
    Task UpdateBookAsync(Guid bookId, BookForUpdateDto bookDto);
    void DeleteBook(Guid bookId);
    Task DeleteBookAsync(Guid bookId);
}