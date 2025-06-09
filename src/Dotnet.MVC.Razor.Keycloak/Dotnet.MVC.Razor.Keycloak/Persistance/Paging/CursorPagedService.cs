using Dotnet.MVC.Razor.Keycloak.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Paging;

/// <summary>
/// Base service providing cursor-based and indexed paging for entities.
/// </summary>
public abstract class CursorPagedService<TEntity, TResponse, TFilter>(AppDbContext ctx)
    : ICursorPagedService<TEntity, TResponse, TFilter>
    where TEntity : EntityBase
    where TResponse : class
    where TFilter : class
{
    protected readonly AppDbContext _ctx = ctx;
    protected readonly DbSet<TEntity> _set = ctx.Set<TEntity>();

    protected abstract IQueryable<TEntity> IncludeNavigation(DbSet<TEntity> set);
    protected abstract IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query, TFilter filter);
    protected abstract IQueryable<TEntity> ApplyGlobalSearch(IQueryable<TEntity> query, string term);
    protected abstract Expression<Func<TEntity, object>> OrderBySelector(string orderBy);
    protected abstract TResponse MapToDto(TEntity entity);

    /// <summary>
    /// Builds the base query applying includes, filters, global search, and ordering.
    /// </summary>
    /// <param name="filters">Filter object or null.</param>
    /// <param name="searchTerm">Search term or null.</param>
    /// <param name="orderBy">Order by field or null.</param>
    /// <param name="orderDescending">Whether to order descending.</param>
    /// <returns>An <see cref="IQueryable{TEntity}"/> ready for paging operations.</returns>
    private IQueryable<TEntity> BuildQuery(
        TFilter? filters,
        string? searchTerm,
        string? orderBy,
        bool orderDescending)
    {
        IQueryable<TEntity> query = IncludeNavigation(_set).AsNoTracking();

        if (filters is not null)
            query = ApplyFilters(query, filters);

        if (!string.IsNullOrEmpty(searchTerm))
            query = ApplyGlobalSearch(query, searchTerm!);

        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderByExpr = OrderBySelector(orderBy!);
            query = orderDescending
                ? query.OrderByDescending(orderByExpr)
                : query.OrderBy(orderByExpr);
        }
        else
        {
            // Fallback ordering to ensure consistent results
            query = query.OrderBy(e => EF.Property<DateTime>(e, nameof(EntityBase.CreationDate)));
        }

        return query;
    }

    /// <inheritdoc />
    public async Task<PagedResult<TResponse>> GetPagedAsync(PagedRequest<TFilter> request, CancellationToken cancellation = default)
    {
        var query = BuildQuery(request.Filters, request.GlobalSearch, request.OrderBy, request.OrderDescending);

        // Apply cursor filter if present
        if (!string.IsNullOrEmpty(request.Cursor)
            && DateTime.TryParse(request.Cursor, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal,
                out var cursor))
        {
            query = query.Where(e => EF.Property<DateTime>(e, nameof(EntityBase.CreationDate)) > cursor);
        }

        var list = await query.Take(request.PageSize + 1).ToListAsync(cancellation);

        var result = new PagedResult<TResponse>
        {
            HasMore = list.Count > request.PageSize
        };

        if (result.HasMore)
        {
            var last = list[request.PageSize];
            result.NextCursor = last.CreationDate.ToString("o");
            list.RemoveAt(request.PageSize);
        }

        result.Items = [.. list.Select(MapToDto)];
        return result;
    }

    /// <summary>
    /// Retrieves an indexed page of results (zero-based), applying filters, global search, and ordering.
    /// </summary>
    /// <param name="request">The indexed paging request containing page index, page size, and filtering/sorting options.</param>
    /// <returns>
    /// An <see cref="IndexedPagedResult{TResponse}"/> containing the page items, total record count, and whether more pages exist.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="request.PageIndex"/> is less than zero or <paramref name="request.PageSize"/> is not positive.</exception>
    public async Task<IndexedPagedResult<TResponse>> GetIndexedPageAsync(IndexedPagedRequest<TFilter> request, CancellationToken cancellationToken = default)
    {
        if (request.PageIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "PageIndex must be zero or greater.");
        }
        if (request.PageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "PageSize must be greater than zero.");
        }

        var baseQuery = BuildQuery(request.Filters, request.GlobalSearch, request.OrderBy, request.OrderDescending);

        // Count total records
        var total = await baseQuery.CountAsync(cancellationToken: cancellationToken);

        // Fetch one extra to determine HasMore
        var skip = request.PageIndex * request.PageSize;
        var items = await baseQuery.Skip(skip).Take(request.PageSize).ToListAsync(cancellationToken);

        return new IndexedPagedResult<TResponse>
        {
            Items = [.. items.Select(MapToDto)],
            TotalRecords = total
        };
    }
}
