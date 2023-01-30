namespace Books.Api.Configurations.Middleware.Filters;

using System.Collections.Generic;
using Books.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class BookResultFilter : IAsyncResultFilter 
// to reduce overhead, we are implementing IAsyncResultFilter instead of inheriting from ActionFilterAttribute and overriding OnResultExecuting
{
    private readonly ILogger<BookResultFilter> _logger;

    public BookResultFilter(ILogger<BookResultFilter> logger) => _logger = logger;

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        _logger.LogInformation($"Called filter: {nameof(BookResultFilter)}");

        var resultFromAction = context.Result as ObjectResult;

        if (resultFromAction?.Value is null 
            || resultFromAction?.StatusCode < StatusCodes.Status200OK 
            || resultFromAction?.StatusCode >= StatusCodes.Status300MultipleChoices)
        {
            await next();
            return;
        }

        // we could use filter to filter out sensitive data or to map data from one format to another,
        // so we could use filter to map from `Book` to `BookDto` here instead of doing that inside our
        // service - this is a matter of preference
        // if we would decide to change the format of our data, we could do that like so:
        // resultFromAction.Value = Mapper.Map<BookDto>(resultFromAction.Value);
 
        if (resultFromAction?.Value is BookDto bookDto)
        {
            bookDto.Title = $"[FILTERED] {bookDto.Title}";
        }
        else if (resultFromAction?.Value is IEnumerable<BookDto> booksDto)
        {
            foreach (var b in booksDto)
            {
                b.Title = $"[FILTERED] {b.Title}";
            }
        }
        
        await next();
    }
}