using Books.Api.Configurations.Middleware;

namespace Books.Api.Configurations.Extensions;

public static class WebApplicationUseGlobalErrorHandlerExtension
{
    public static WebApplication UseGlobalErrorHandler(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
 
        return app;
    }
}