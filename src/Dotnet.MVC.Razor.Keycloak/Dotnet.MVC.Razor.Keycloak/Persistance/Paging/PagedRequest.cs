namespace Dotnet.MVC.Razor.Keycloak.Persistance.Paging;

public class PagedRequest<TFilter>
{
    public string? Cursor { get; set; }
    public int PageSize { get; set; } = 25;
    public string? GlobalSearch { get; set; }
    public string? OrderBy { get; set; }
    public bool OrderDescending { get; set; } = false;
    public TFilter? Filters { get; set; }
}
