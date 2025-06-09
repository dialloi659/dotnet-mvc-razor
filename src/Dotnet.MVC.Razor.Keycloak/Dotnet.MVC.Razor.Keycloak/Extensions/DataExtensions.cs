using Dotnet.MVC.Razor.Keycloak.Data;
using Dotnet.MVC.Razor.Keycloak.Logics;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.MVC.Razor.Keycloak.Extensions;

public static class DataExtensions
{
    public static IServiceCollection ConfigureDbContextAndRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DB")));
        services.AddScoped<IReleaseNoteRepository, ReleaseNoteRepository>();

        return services;
    }

    public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (!context.ReleaseNotes.Any())
            {
                DataSeeder.SeedAsync(context).Wait();
            }

        }
        return app;
    }
}
