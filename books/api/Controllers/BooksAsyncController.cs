using AutoMapper;
using Books.Api.DataAccess;
using Books.Api.Dtos;
using Books.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controller;

[ApiController]
[Route("api/books/async")]
public class BooksAsyncController : ControllerBase
{
    private readonly IBooksRepository _repository;
    private readonly IMapper _mapper;

    public BooksAsyncController(IBooksRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksAsync()
    {
        var books = await _repository.GetBooksAsync();

        return Ok(_mapper.Map<IEnumerable<BookDto>>(books));
    }

    [HttpGet("{bookId:guid}", Name = "GetBookAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BookDto>> GetBookAsync(Guid bookId)
    {
        var book = await _repository.GetBookByIdAsync(bookId);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<BookDto>(book));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBookAsync(BookForCreationDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        await _repository.CreateBookAsync(book);
        await _repository.SaveChangesAsync();

        var bookToReturn = _mapper.Map<BookDto>(book);

        return CreatedAtAction(nameof(GetBookAsync), new { bookId = bookToReturn.Id }, bookToReturn);
    }

    [HttpPut("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBookAsync(Guid bookId, BookForUpdateDto bookDto)
    {
        var book = await _repository.GetBookByIdAsync(bookId);

        if (book == null)
        {
            return NotFound();
        }

        _mapper.Map(bookDto, book);

        _repository.UpdateBook(book);

        await _repository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteBook(Guid bookId)
    {
        var book = await _repository.GetBookByIdAsync(bookId);

        if (book == null)
        {
            return NotFound();
        }

        await _repository.DeleteBookAsync(book);
        await _repository.SaveChangesAsync();

        return NoContent();
    }
}