namespace Dotnet.MVC.Razor.Keycloak.Persistance.Paging;

/// <summary>
/// Result model for indexed paging. Contains the items, total record count, and paging state.
/// </summary>
public class IndexedPagedResult<T>
{
    /// <summary>
    /// The items in the current page.
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// Total number of records matching the criteria.
    /// </summary>
    public int TotalRecords { get; set; }
}
