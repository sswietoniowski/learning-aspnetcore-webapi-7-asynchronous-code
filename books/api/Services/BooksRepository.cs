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

    public IEnumerable<Book> GetBooks() => _context.Books.AsNoTracking().Include(b => b.Author).ToList();

    public async Task<IEnumerable<Book>> GetBooksAsync() => await _context.Books.AsNoTracking().Include(b => b.Author).ToListAsync();

    public Book? GetBookById(Guid id) => _context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);

    public async Task<Book?> GetBookByIdAsync(Guid id) => await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
    
    public void CreateBook(Book book)
    {
        // theoretically we could add CreateBookAsync and CreateBook methods to the interface
        // but in reality _context.Books.AddAsync should be used only in a special occasion,
        // for the most part _context.Books.Add should suffice
        _context.Books.Add(book);
    }

    public void UpdateBook(Book book)
    {
        _context.Books.Update(book);
    }

    public void DeleteBook(Book book)
    {
        var bookToBeDeleted = GetBookById(book.Id);

        if (bookToBeDeleted is not null)
        {
            _context.Books.Remove(bookToBeDeleted);
        }
    }

    public async Task DeleteBookAsync(Book book)
    {
        var bookToBeDeleted = await GetBookByIdAsync(book.Id);

        if (bookToBeDeleted is not null)
        {
            _context.Books.Remove(bookToBeDeleted);
        }
    }

    public bool SaveChanges() => _context.SaveChanges() > 0; // true if one or more entities were changed

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0; // true if one or more entities were changed
}