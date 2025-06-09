using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dotnet.MVC.Razor.Keycloak.Models.ViewModels;

public class FilterViewModel
{
    public bool ShowCategoryFilter { get; set; } = false;
    public bool ShowStatusFilter { get; set; } = false;
    public bool ShowArchivedFilter { get; set; } = false;
    public bool ShowDateFilter { get; set; } = false;

    public string? CategoryFilterId { get; set; } = "categoryFilter";
    public string? StatusFilterId { get; set; } = "statusFilter";
    public string? ArchivedFilterId { get; set; } = "archivedFilter";
    public string? DateFilterId { get; set; } = "dateFilter";

    public List<SelectListItem> Categories { get; set; } = [];
    public List<SelectListItem> StatusOptions { get; set; } = [];
    public List<CustomFilter> CustomFilters { get; set; } = [];
}

public class CustomFilter
{
    public string? Id { get; set; }
    public string? Label { get; set; }
    public string? Type { get; set; } // "select" or "input"
    public string? InputType { get; set; } = "text"; // for input type
    public string? DefaultText { get; set; } = "All";
    public string? Placeholder { get; set; }
    public List<SelectListItem> Options { get; set; } = []; // for select type
}
