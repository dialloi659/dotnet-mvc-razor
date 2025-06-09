namespace Dotnet.MVC.Razor.Keycloak.Models.ViewModels;

public class ReleaseNoteViewModel
{
    public int Id { get; set; }
    public string Version { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}
