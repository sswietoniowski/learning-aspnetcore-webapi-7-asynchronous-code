using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Api.Entities;

[Table("Books")]
public class Book
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(512)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(2048)]
    public string? Description { get; set; }
    [Required]    
    [ForeignKey(nameof(Author))]
    public Guid AuthorId { get; set; }
    [Required]
    public virtual Author Author { get; set; } = default!;

    public Book()
    {
    }

    public Book(Guid Id, string Title, string? Description, Guid AuthorId)
    {
        this.Id = Id;
        this.Title = Title;
        this.Description = Description;
        this.AuthorId = AuthorId;
    }
}