using Books.Api.DataAccess;
using Books.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.Services;

public class BooksRepository : IBooksRepository
{
    private readonly BooksDbContext _context;

    public BooksRepository(BooksDbContext booksDbContext)
    {
        _context = booksDbContext ?? throw new ArgumentNullException(nameof(booksDbContext));
    }

    public async Task<IEnumerable<Book>> GetBooksAsync() => await _context.Books.Include(b => b.Author).ToListAsync();

    public async Task<Book?> GetBookByIdAsync(Guid id) => await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

    public async Task CreateBookAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public void UpdateBook(Book book)
    {
        _context.Books.Update(book);
    }

    public async Task DeleteBookAsync(Book book)
    {
        var bookToBeDeleted = await GetBookByIdAsync(book.Id);

        if (bookToBeDeleted is not null)
        {
            _context.Books.Remove(bookToBeDeleted);
        }
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public IEnumerable<Book> GetBooks() => _context.Books.Include(b => b.Author).ToList();

    public Book? GetBookById(Guid id) => _context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);

    public void CreateBook(Book book)
    {
        _context.Books.Add(book);
    }

    public void DeleteBook(Book book)
    {
        var bookToBeDeleted = GetBookById(book.Id);

        if (bookToBeDeleted is not null)
        {
            _context.Books.Remove(bookToBeDeleted);
        }
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}