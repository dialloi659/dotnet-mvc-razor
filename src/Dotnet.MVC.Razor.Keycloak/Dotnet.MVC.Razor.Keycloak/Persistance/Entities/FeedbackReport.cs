using Dotnet.MVC.Razor.Keycloak.Models;
using System.ComponentModel.DataAnnotations;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Entities;

public class FeedbackReport: EntityBase
{

    public int CategoryId { get; set; }
    /// <summary>
    /// Bug, Suggestion, Question, etc.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public FeedbackCategory Category { get; set; } = null!;

    public FeedbackStatus Status { get; set; } = FeedbackStatus.New;

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string PageUrl { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public bool IsArchived { get; set; } = false;
}
