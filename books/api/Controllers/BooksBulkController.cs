using Books.Api.Dtos;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controller;

[ApiController]
[Route("api/books/bulk")] // this route is meant to be use for bulk operations (e.g. create, update, delete)
public class BooksBulkController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksBulkController(IBooksService booksService)
    {
        _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBooks([FromBody] IEnumerable<BookForCreationDto> booksDto)
    {
        await _booksService.CreateBooksAsync(booksDto);

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBooks([FromBody] IEnumerable<(Guid, BookForUpdateDto)> booksDto)
    {
        await _booksService.UpdateBooksAsync(booksDto);

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBooks([FromBody] IEnumerable<Guid> bookIds)
    {
        await _booksService.DeleteBooksAsync(bookIds);

        return NoContent();
    }
}