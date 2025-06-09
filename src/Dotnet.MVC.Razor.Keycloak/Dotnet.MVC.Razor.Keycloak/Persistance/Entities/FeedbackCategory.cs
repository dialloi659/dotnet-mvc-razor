using System.ComponentModel.DataAnnotations;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Entities;

public class FeedbackCategory: EntityBase
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public ICollection<FeedbackReport> FeedbackReports { get; set; } = [];
}
