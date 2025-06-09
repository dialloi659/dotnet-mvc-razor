using Dotnet.MVC.Razor.Keycloak.Persistance.Entities;
using Dotnet.MVC.Razor.Keycloak.Persistance.Filters;
using Dotnet.MVC.Razor.Keycloak.Persistance.Paging;
using Dotnet.MVC.Razor.Keycloak.Persistance.Requests;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Repositories;

public interface IFeedbackRepository
{
    Task<IEnumerable<FeedbackCategory>> GetAllCategoriesAsync();
    Task<PagedResult<FeedbackRequest>> GetPagedAsync(PagedRequest<FeedbackFilter> request, CancellationToken cancellationToken = default);
    Task<IndexedPagedResult<FeedbackRequest>> GetIndexedPageAsync(IndexedPagedRequest<FeedbackFilter> request, CancellationToken cancellationToken = default);
}
