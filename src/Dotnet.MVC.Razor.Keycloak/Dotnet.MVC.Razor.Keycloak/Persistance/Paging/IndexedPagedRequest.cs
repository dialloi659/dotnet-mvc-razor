namespace Dotnet.MVC.Razor.Keycloak.Persistance.Paging;

/// <summary>
/// Request model for indexed paging (zero-based). Contains pagination and filtering/sorting options.
/// </summary>
public class IndexedPagedRequest<TFilter>
    where TFilter : class
{
    /// <summary>
    /// Zero-based index of the desired page.
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 25;

    /// <summary>
    /// Global search term.
    /// </summary>
    public string? GlobalSearch { get; set; }

    /// <summary>
    /// Property name to order by.
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Whether to apply descending order.
    /// </summary>
    public bool OrderDescending { get; set; } = false;

    /// <summary>
    /// Optional filter object.
    /// </summary>
    public TFilter? Filters { get; set; }
}