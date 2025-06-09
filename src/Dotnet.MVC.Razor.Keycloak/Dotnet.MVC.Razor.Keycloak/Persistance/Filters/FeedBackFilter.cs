using Dotnet.MVC.Razor.Keycloak.Models;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Filters;

public class FeedbackFilter
{
    public int Draw { get; set; } = 0; // For DataTables request identifier to hndle response
    public string? Email { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public FeedbackStatus? Status { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsArchived { get; set; }
}
