using AutoMapper;
using Books.Api.DataAccess;
using Books.Api.Dtos;
using Books.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controller;

[ApiController]
[Route("api/books/sync")]
public class BooksSyncController : ControllerBase
{
    private readonly IBooksRepository _repository;
    private readonly IMapper _mapper;

    public BooksSyncController(IBooksRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<BookDto>> GetBooks()
    {
        var books = _repository.GetBooks();

        return Ok(_mapper.Map<IEnumerable<BookDto>>(books));
    }

    [HttpGet("{bookId:guid}", Name = "GetBook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<BookDto> GetBook(Guid bookId)
    {
        var book = _repository.GetBookById(bookId);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<BookDto>(book));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateBook(BookForCreationDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        _repository.CreateBook(book);
        _repository.SaveChanges();

        var bookToReturn = _mapper.Map<BookDto>(book);

        return CreatedAtAction(nameof(GetBook), new { bookId = bookToReturn.Id }, bookToReturn);
    }

    [HttpPut("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateBook(Guid bookId, BookForUpdateDto bookDto)
    {
        var book = _repository.GetBookById(bookId);

        if (book == null)
        {
            return NotFound();
        }

        _mapper.Map(bookDto, book);

        _repository.UpdateBook(book);

        _repository.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{bookId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteBook(Guid bookId)
    {
        var book = _repository.GetBookById(bookId);

        if (book == null)
        {
            return NotFound();
        }

        _repository.DeleteBook(book);
        _repository.SaveChanges();

        return NoContent();
    }
}