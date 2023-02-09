using AutoMapper;
using Books.Api.Dtos;
using Books.Api.Dtos.External;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Books.Api.Configurations.Middleware.Filters;

public class BookWithCoversResultFilter : IAsyncResultFilter
{
    private readonly ILogger<BookWithCoversResultFilter> _logger;
    private readonly IMapper _mapper;

    public BookWithCoversResultFilter(ILogger<BookWithCoversResultFilter> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        _logger.LogInformation($"Called filter: {nameof(BookWithCoversResultFilter)}");

        var resultFromAction = context.Result as ObjectResult;

        if (resultFromAction?.Value is null 
            || resultFromAction?.StatusCode < StatusCodes.Status200OK 
            || resultFromAction?.StatusCode >= StatusCodes.Status300MultipleChoices)
        {
            await next();
            return;
        }

        var (bookDto, coversDto) = ((BookDto, IEnumerable<CoverDto>))resultFromAction!.Value;

        bookDto.Title = $"[FILTERED] {bookDto.Title}";

        var mappedBook = _mapper.Map<BookWithCoversDto>(bookDto);
        resultFromAction.Value = _mapper.Map(coversDto, mappedBook);

        await next();
    }
}