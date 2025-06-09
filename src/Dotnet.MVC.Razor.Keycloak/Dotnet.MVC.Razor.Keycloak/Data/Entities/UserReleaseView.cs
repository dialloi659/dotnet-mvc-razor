namespace Dotnet.MVC.Razor.Keycloak.Data.Entities;

public class UserReleaseView
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ReleaseNoteId { get; set; }
    public DateTime ViewedAt { get; set; }
}
