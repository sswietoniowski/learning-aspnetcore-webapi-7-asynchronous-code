using Books.Api.Entities;

namespace Books.Api.Services;

public interface IBooksRepository
{
    IEnumerable<Book> GetBooks();
    Task<IEnumerable<Book>> GetBooksAsync();
    Book? GetBookById(Guid id);
    Task<Book?> GetBookByIdAsync(Guid id);
    void CreateBook(Book book);
    void CreateBooks(IEnumerable<Book> books);
    void UpdateBook(Book book);
    void UpdateBooks(IEnumerable<Book> books);
    void DeleteBook(Book book);
    Task DeleteBookAsync(Book book);
    Task DeleteBooksAsync(IEnumerable<Book> books);
    bool SaveChanges();
    Task<bool> SaveChangesAsync();
}