using System.ComponentModel.DataAnnotations;

namespace Dotnet.MVC.Razor.Keycloak.Models.ViewModels;

public class FeedbackReportViewModel
{
    [Required(ErrorMessage = "Please select a category.")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your message.")]
    [StringLength(1000, MinimumLength = 5, ErrorMessage = "Message must be at least 10 characters.")]
    public string Message { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Page URL")]
    public string PageUrl { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

