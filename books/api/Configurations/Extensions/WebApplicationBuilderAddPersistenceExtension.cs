namespace Books.Api.Configurations.Extensions;

using Books.Api.DataAccess;
using Books.Api.Services;
using BooksApi.Services.Legacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderAddPersistenceExtension
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BooksDbContext>(options => 
            options.UseSqlite(builder.Configuration.GetConnectionString("BooksDb")));

        builder.Services.AddScoped<IBooksRepository, BooksRepository>();
        builder.Services.AddScoped<IBooksService, BooksService>();
        
        builder.Services.AddScoped<IBooksPageCalculatorService, BooksPageCalculatorService>();

        return builder;
    }
}
