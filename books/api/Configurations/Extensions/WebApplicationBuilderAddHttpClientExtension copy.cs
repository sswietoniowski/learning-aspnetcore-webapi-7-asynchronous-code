namespace Books.Api.Configurations.Extensions;

public static class WebApplicationBuilderAddHttpClientExtension
{
    public static WebApplicationBuilder AddHttpClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();

        return builder;
    }
}