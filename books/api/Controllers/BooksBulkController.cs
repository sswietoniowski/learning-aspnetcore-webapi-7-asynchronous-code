using Books.Api.Dtos;
using Books.Api.Helpers;
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

    [HttpGet("({bookIds})", Name = "GetBooksCollection")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksAsync(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> bookIds)
    {
        var bookDtos = await _booksService.GetBooksAsync(bookIds);

        if (bookDtos.Count() != bookIds.Count())
        {
            return NotFound();
        }

        return Ok(bookDtos);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBooks([FromBody] IEnumerable<BookForCreationDto> bookDtos)
    {
        var (createdBookIds, createdBookDtos) = await _booksService.CreateBooksAsync(bookDtos);
    
        return CreatedAtRoute("GetBooksCollection", new { bookIds = createdBookIds }, createdBookDtos);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBooks([FromBody] IEnumerable<BookForBulkUpdateDto> bookDtos)
    {
        await _booksService.UpdateBooksAsync(bookDtos);

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