using Books.Api.Dtos;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controller;

[ApiController]
[Route("api/books/bulk")] // this route is meant to be use for bulk operations (e.g. create, update, delete)
public class BooksBulkController : ControllerBase
{
    // while bulk operations might be a good idea for performance reasons, it is not necessarily a good idea 
    // for security reasons and while bulk creation might bo OK, bulk update and bulk delete could be
    // exploited by malicious users to delete or update data they are not supposed to, so it is up to you
    // to decide if you want to implement bulk operations at all or to implement only some actions

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
    public async Task<IActionResult> UpdateBooks([FromBody] IEnumerable<BookForBulkUpdateDto> booksDto)
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