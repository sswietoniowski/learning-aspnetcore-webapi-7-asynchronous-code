using System.ComponentModel.DataAnnotations;
using Books.Api.Dtos.External;

namespace Books.Api.Dtos;

public class BookForCreationDto
{
    [Required]
    [MaxLength(512)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(2048)]
    public string? Description { get; set; }
    [Required]
    public Guid AuthorId { get; set; }
}

public class BookForUpdateDto : BookForCreationDto
{
}

public class BookForBulkUpdateDto : BookForUpdateDto
{
    public Guid Id { get; set; }
}

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid AuthorId { get; set; }
    public string Author { get; set; } = string.Empty;

    // consider using only constructor with parameters to eliminate risk of creating an invalid object
    // if you do that, reconfigure AutoMapper to use the constructor with parameters (example below):
    // .ConstructUsing(src => new BookDto(src.Id, src.Title, src.Description, src.AuthorId, src.Author));

    public CoverDto? Cover { get; set; }
}