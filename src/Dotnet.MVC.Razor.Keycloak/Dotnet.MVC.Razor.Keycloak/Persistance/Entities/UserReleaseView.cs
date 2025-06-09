namespace Dotnet.MVC.Razor.Keycloak.Persistance.Entities;

public class UserReleaseView: EntityBase
{
    public string UserId { get; set; } = string.Empty;
    public int ReleaseNoteId { get; set; }
    public DateTime ViewedAt { get; set; }
}
