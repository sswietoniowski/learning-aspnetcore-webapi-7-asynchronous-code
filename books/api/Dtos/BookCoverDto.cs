namespace Books.Api.Dtos;

public class BookCoverDto
{
    public string Id { get; set; } = string.Empty;

    // intentionally not mapped to speed up the response
    // public byte[]? Content { get; set; } = null;
}
