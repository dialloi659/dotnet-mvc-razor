﻿namespace Dotnet.MVC.Razor.Keycloak.Persistance.Paging;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public string? NextCursor { get; set; }
    public bool HasMore { get; set; }
}
