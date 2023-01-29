using Books.Api.DataAccess;
using Books.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.Services;

public class BooksRepository : IBooksRepository
{
    private readonly BooksDbContext context;

    public BooksRepository(BooksDbContext booksDbContext)
    {
        context = booksDbContext;
    }

    public async Task<IEnumerable<Book>> GetBooksAsync() => await context.Books.ToListAsync();

    public async Task<Book?> GetBookByIdAsync(Guid id) => await context.Books.FindAsync(id);

    public async Task CreateBookAsync(Book book)
    {
        await context.Books.AddAsync(book);
    }

    public void UpdateBook(Book book)
    {
        context.Books.Update(book);
    }

    public async Task DeleteBookAsync(Guid id)
    {
        var book = await GetBookByIdAsync(id);

        if (book is not null)
        {
            context.Books.Remove(book);
        }
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();    
}