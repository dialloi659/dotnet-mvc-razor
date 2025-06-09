using Dotnet.MVC.Razor.Keycloak.Models;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Requests;

public class FeedbackRequest
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PageUrl { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public FeedbackStatus Status { get; set; }
    public bool IsArchived { get; set; }
}
