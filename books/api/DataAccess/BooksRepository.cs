using Books.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.DataAccess;

public class BooksRepository : IBooksRepository
{
    private readonly BooksDbContext context;

    public BooksRepository(BooksDbContext booksDbContext)
    {
        this.context = booksDbContext;
    }

    public async Task<IEnumerable<Book>> GetBooksAsync() => await context.Books.ToListAsync();

    public async Task<Book?> GetBookByIdAsync(Guid id) => await context.Books.FindAsync(id);

    public async Task CreateBookAsync(Book book)
    {
        await context.Books.AddAsync(book);

        await context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(Book book)
    {
        context.Books.Update(book);

        await context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(Guid id)
    {
        var book = await GetBookByIdAsync(id);

        if (book is not null)
        {
            context.Books.Remove(book);

            await context.SaveChangesAsync();
        }
    }
}