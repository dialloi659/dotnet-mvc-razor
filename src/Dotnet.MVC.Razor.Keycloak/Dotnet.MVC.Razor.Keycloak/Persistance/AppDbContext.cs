using Dotnet.MVC.Razor.Keycloak.Data.Entities;
using Dotnet.MVC.Razor.Keycloak.Models;
using Dotnet.MVC.Razor.Keycloak.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dotnet.MVC.Razor.Keycloak.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ReleaseNote> ReleaseNotes { get; set; }
    public DbSet<UserReleaseView> UserReleaseViews { get; set; }

    public DbSet<FeedbackReport> FeedbackReports { get; set; }

    public DbSet<FeedbackCategory> FeedbackCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeedbackReport>().Property(p => p.Status)
            .HasConversion<EnumToStringConverter<FeedbackStatus>>();

        modelBuilder.Entity<FeedbackCategory>().HasData([
            new FeedbackCategory { Id = -1, Name = "New" },
            new FeedbackCategory {  Id = -2, Name = "InProgress" },
            new FeedbackCategory {  Id = -3, Name = "Processed" },
            new FeedbackCategory {  Id = -4,  Name = "NotInteresting" }
            ]);

        // CreationDate configuration
        modelBuilder.Entity<FeedbackReport>().Property(p => p.CreationDate)
        .HasDefaultValueSql("CURRENT_TIMESTAMP")
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<ReleaseNote>().Property(p => p.CreationDate)
        .HasDefaultValueSql("CURRENT_TIMESTAMP")
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<UserReleaseView>().Property(p => p.CreationDate)
        .HasDefaultValueSql("CURRENT_TIMESTAMP")
        .ValueGeneratedOnAdd();

        modelBuilder.Entity<FeedbackCategory>().Property(p => p.CreationDate)
        .HasDefaultValueSql("CURRENT_TIMESTAMP")
        .ValueGeneratedOnAdd();
    }
}
