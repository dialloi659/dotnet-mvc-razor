using Dotnet.MVC.Razor.Keycloak.Data.Entities;
using Dotnet.MVC.Razor.Keycloak.Persistance;

namespace Dotnet.MVC.Razor.Keycloak.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.ReleaseNotes.Any())
        {
            context.ReleaseNotes.Add(new ReleaseNote
            {
                Version = "1.0.0",
                Title = "Initial Release",
                ContentMarkdown = "### Features\n- Welcome to the first version!\n- This includes basic release tracking.",
                ReleaseDate = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }
    }
}
