using Books.Api.Entities;

namespace Books.Api.Services;

public interface IBooksRepository
{
    IEnumerable<Book> GetBooks();
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds);
    Book? GetBookById(Guid id);
    Task<Book?> GetBookByIdAsync(Guid id);
    IAsyncEnumerable<Book> GetBooksAsAsyncEnumerable();
    void CreateBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Book book);
    Task DeleteBookAsync(Book book);
    bool SaveChanges();
    Task<bool> SaveChangesAsync();
}