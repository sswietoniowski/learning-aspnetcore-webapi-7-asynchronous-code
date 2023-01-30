using AutoMapper;
using Books.Api.Dtos;
using Books.Api.Entities;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controller;

[ApiController]
[Route("api/books/sync")]
public class BooksSyncController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksSyncController(IBooksService booksService)
    {
        _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]        
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<BookDto>> GetBooks()
    {
        var booksDto = _booksService.GetBooks();

        return Ok(booksDto);
    }

    [HttpGet("{bookId:guid}", Name = "GetBook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<BookDto> GetBook(Guid bookId)
    {
        var bookDto = _booksService.GetBookById(bookId);

        return Ok(bookDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateBook(BookForCreationDto bookDto)
    {
        var createdBookDto = _booksService.CreateBook(bookDto);

        return CreatedAtAction(nameof(GetBook), new { bookId = createdBookDto.Id }, createdBookDto);
    }

    [HttpPut("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateBook(Guid bookId, BookForUpdateDto bookDto)
    {
        _booksService.UpdateBook(bookId, bookDto);

        return NoContent();
    }

    [HttpDelete("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteBook(Guid bookId)
    {
        _booksService.DeleteBook(bookId);

        return NoContent();
    }
}