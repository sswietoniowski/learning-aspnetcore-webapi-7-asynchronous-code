using Covers.Api.Dtos;
using Covers.Api.Services;

namespace Covers.Api;

public static class WebApplicationUseMinimalApiEndpointsExtensions
{
    public static void UseMinimalApiEndpoints(this WebApplication app)
    {
        app.MapGet("api/covers/{coverId}", async (ICoversService coversService, string coverId, bool returnFault) =>
        {
            var coverDto = await coversService.GetCoverAsync(coverId);

            if (returnFault)
            {
                const int DELAY_IN_SECONDS = 5;

                await Task.Delay(TimeSpan.FromSeconds(DELAY_IN_SECONDS));
                return Results.Problem("Something went wrong", statusCode: StatusCodes.Status500InternalServerError);
            }

            return Results.Ok(coverDto);
        })
            .WithName("GetCover")
            .Produces<Task<CoverDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}