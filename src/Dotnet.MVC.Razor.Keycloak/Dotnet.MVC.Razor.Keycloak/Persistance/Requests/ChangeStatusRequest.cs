using Dotnet.MVC.Razor.Keycloak.Models;
using System.ComponentModel.DataAnnotations;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Requests;

public class ChangeStatusRequest
{
    public int Id { get; set; }
    [Required]
    public FeedbackStatus Status { get; set; }
}
