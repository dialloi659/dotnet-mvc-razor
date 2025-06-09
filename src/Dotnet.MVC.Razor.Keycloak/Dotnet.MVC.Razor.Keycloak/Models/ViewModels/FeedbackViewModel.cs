using System.ComponentModel.DataAnnotations;

namespace Dotnet.MVC.Razor.Keycloak.Models.ViewModels;

public class FeedbackViewModel
{
    [Required]
    [Display(Name = "Type of Issue")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Page URL")]
    public string PageUrl { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    [Display(Name = "Your Message")]
    public string Message { get; set; } = string.Empty;
}
