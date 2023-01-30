using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Books.Api.Entities;

[Table("Authors")]
public class Author
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(128)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(128)]
    public string LastName { get; set; } = string.Empty;

    public string Name => $"{FirstName} {LastName}";

    public Author()
    {
    }

    public Author(Guid Id, string FirstName, string LastName)
    {
        this.Id = Id;
        this.FirstName = FirstName;
        this.LastName = LastName;
    }    
}