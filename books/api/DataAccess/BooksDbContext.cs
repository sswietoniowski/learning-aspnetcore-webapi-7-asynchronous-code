using Books.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.DataAccess;

public class BooksDbContext : DbContext
{
    public virtual DbSet<Book> Books => Set<Book>();

    public virtual DbSet<Author> Authors => Set<Author>();

    public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasData
        (
            new Author
            (
                Id: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2f"),
                FirstName: "Stephen",
                LastName: "King"
            ),
            new Author
            (
                Id: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2e"),
                FirstName: "George",
                LastName: "Martin"
            )
        );

        modelBuilder.Entity<Book>().HasData
        (
            new Book
            (
                Id: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2d"),
                Title: "The Shining",
                Description: "A horror novel about an evil hotel.",
                AuthorId: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2f")
            ),
            new Book
            (
                Id: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2c"),
                Title: "A Game of Thrones",
                Description: "The first book in the A Song of Ice and Fire series.",
                AuthorId: new Guid("c0a80121-48aa-48b0-8c0c-927b1f8eac2e")
            )
        );
    }
}