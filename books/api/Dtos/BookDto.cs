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

    public BookDto()
    {
    }

    public BookDto(Guid id, string title, string? description, Guid authorId, string author)
    {
        Id = id;
        Title = title;
        Description = description;
        AuthorId = authorId;
        Author = author;
    }

    // we might return external DTOs (e.g. from external API) in our API, but it would be better to use a separate DTO for that
    // as presented inside the next class: BookWithCoversDto, which is a composition of BookDto and BookCoverDto
    // what you see here is just for quick demonstration purposes, but it's not a good practice

    public CoverDto? Cover { get; set; }
    public IEnumerable<CoverDto> AllCovers { get; set; } = new List<CoverDto>();
}

public class BookWithCoversDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid AuthorId { get; set; }
    public string Author { get; set; } = string.Empty;
    public IEnumerable<BookCoverDto> Covers { get; set; } = new List<BookCoverDto>();

    public BookWithCoversDto(Guid id, string title, string? description, Guid authorId, string author)
    {
        Id = id;
        Title = title;
        Description = description;
        AuthorId = authorId;
        Author = author;
    }
}