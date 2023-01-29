using Books.Api.Entities;

namespace Books.Api.DataAccess;

public interface IBooksRepository
{
    IEnumerable<Book> GetBooks();
    Task<IEnumerable<Book>> GetBooksAsync();
    Book? GetBookById(Guid id);
    Task<Book?> GetBookByIdAsync(Guid id);
    void CreateBook(Book book);
    Task CreateBookAsync(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Guid id);
    Task DeleteBookAsync(Guid id);
    void SaveChanges();
    Task SaveChangesAsync();
}