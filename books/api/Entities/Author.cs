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
    public string FirstName { get; set; }
    [Required]
    [MaxLength(128)]
    public string LastName { get; set; }

    public string Name => $"{FirstName} {LastName}";

    public Author(Guid Id, string FirstName, string LastName)
    {
        this.Id = Id;
        this.FirstName = FirstName;
        this.LastName = LastName;
    }    
}