using Dotnet.MVC.Razor.Keycloak.Persistance.Entities;
using System.ComponentModel.DataAnnotations;

namespace Dotnet.MVC.Razor.Keycloak.Data.Entities;

public class ReleaseNote: EntityBase
{
    [Required]
    public string Version { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string ContentMarkdown { get; set; } = string.Empty;

    [Required]
    public DateTime ReleaseDate { get; set; }
}
