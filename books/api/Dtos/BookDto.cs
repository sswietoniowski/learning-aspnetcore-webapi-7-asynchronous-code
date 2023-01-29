using System.ComponentModel.DataAnnotations;

namespace Books.Api.Dtos;

public class BookForCreationDto
{
    [Required]
    [MaxLength(512)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(2048)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public Guid AuthorId { get; set; }
}

public class BookForUpdateDto : BookForCreationDto
{
}

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public string Author { get; set; } = string.Empty;
}