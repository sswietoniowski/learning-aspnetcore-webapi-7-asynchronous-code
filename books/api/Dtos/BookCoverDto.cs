namespace Books.Api.Dtos;

public class BookCoverDto
{
    public string Id { get; set; } = string.Empty;
    public byte[]? Content { get; set; } = null;
}
