namespace Dotnet.MVC.Razor.Keycloak.Persistance.Paging;

public interface ICursorPagedService<TEntity, TResponse, TFilter> where TFilter : class
    where TResponse : class
{
    Task<PagedResult<TResponse>> GetPagedAsync(PagedRequest<TFilter> request, CancellationToken cancellation = default);
    Task<IndexedPagedResult<TResponse>> GetIndexedPageAsync(IndexedPagedRequest<TFilter> request, CancellationToken cancellationToken = default);
}
