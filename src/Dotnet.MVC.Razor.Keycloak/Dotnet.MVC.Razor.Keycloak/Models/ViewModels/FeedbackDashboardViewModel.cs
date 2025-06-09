using Dotnet.MVC.Razor.Keycloak.Persistance.Entities;

namespace Dotnet.MVC.Razor.Keycloak.Models.ViewModels;

public class FeedbackDashboardViewModel
{
    public IEnumerable<FeedbackCategory> Categories { get; set; } = [];
}
