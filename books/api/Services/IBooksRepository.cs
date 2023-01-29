using Books.Api.Entities;

namespace Books.Api.DataAccess;

public interface IBooksRepository
{
    Task<IEnumerable<Book>> GetBooksAsync();
    Task<Book?> GetBookByIdAsync(Guid id);
    Task CreateBookAsync(Book book);
    void UpdateBook(Book book);
    Task DeleteBookAsync(Guid id);
    Task SaveChangesAsync();
}