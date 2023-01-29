using Microsoft.EntityFrameworkCore;

namespace Books.Api.DataAccess;

public static class SeedDatabase
{
    private static T GetService<T>(this IApplicationBuilder app) where T : notnull =>
        app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<T>();

    public static async Task SeedAsync(this IApplicationBuilder app)
    {
        var context = app.GetService<BooksDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }

        // add code to seed data here
    }
}