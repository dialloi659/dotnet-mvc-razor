using Dotnet.MVC.Razor.Keycloak.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.MVC.Razor.Keycloak.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ReleaseNote> ReleaseNotes => Set<ReleaseNote>();
    public DbSet<UserReleaseView> UserReleaseViews => Set<UserReleaseView>();
}
