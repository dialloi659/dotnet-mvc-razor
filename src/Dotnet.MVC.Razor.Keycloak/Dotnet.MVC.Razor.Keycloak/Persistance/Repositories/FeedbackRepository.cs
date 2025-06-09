using Dotnet.MVC.Razor.Keycloak.Persistance.Entities;
using Dotnet.MVC.Razor.Keycloak.Persistance.Filters;
using Dotnet.MVC.Razor.Keycloak.Persistance.Paging;
using Dotnet.MVC.Razor.Keycloak.Persistance.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dotnet.MVC.Razor.Keycloak.Persistance.Repositories;

public class FeedbackRepository(AppDbContext appDbContext) : CursorPagedService<FeedbackReport, FeedbackRequest, FeedbackFilter>(appDbContext), IFeedbackRepository
{
    private readonly AppDbContext _appDbCOntext = appDbContext;

    protected override IQueryable<FeedbackReport> IncludeNavigation(DbSet<FeedbackReport> set) => set.Include(f => f.Category);


    protected override IQueryable<FeedbackReport> ApplyFilters(
        IQueryable<FeedbackReport> q, FeedbackFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Email))
            q = q.Where(f => f.Email.Contains(filter.Email));
        if (filter.StartDate.HasValue)
            q = q.Where(f => f.CreationDate >= filter.StartDate);
        if (filter.EndDate.HasValue)
            q = q.Where(f => f.CreationDate <= filter.EndDate);
        if (filter.Status.HasValue)
            q = q.Where(f => f.Status == filter.Status);
        if (filter.CategoryId.HasValue)
            q = q.Where(f => f.CategoryId == filter.CategoryId);

        if(filter is { IsArchived: not null})
        {
            q = q.Where(f => f.IsArchived == filter.IsArchived.Value);
        }
        return q;
    }

    protected override IQueryable<FeedbackReport> ApplyGlobalSearch(
        IQueryable<FeedbackReport> q, string term)
    {
        return q.Where(f =>
            f.Email.Contains(term) ||
            f.Message.Contains(term) ||
            f.Category.Name.Contains(term));
    }

    protected override Expression<Func<FeedbackReport, object>> OrderBySelector(string orderBy)
    {
        // whitelist exactly the column keys your DataTable passes as d.columns[...].data
        return orderBy.ToLowerInvariant() switch
        {
            "email" => f => f.Email,
            "status" => f => f.Status,
            "categoryname" => f => f.Category.Name,
            "pageurl" => f => f.PageUrl,
            "message" => f => f.Message,
            "creationdate" => f => f.CreationDate,
            _ => f => f.CreationDate
        };
    }

    protected override FeedbackRequest MapToDto(FeedbackReport e) =>
        new()
        {
            Id = e.Id,
            CategoryName = e.Category.Name,
            Message = e.Message,
            Email = e.Email,
            PageUrl = e.PageUrl,
            CreationDate = e.CreationDate,
            Status = e.Status,
            IsArchived = e.IsArchived
        };

    public async Task<IEnumerable<FeedbackCategory>> GetAllCategoriesAsync()
    {
        return await _appDbCOntext.FeedbackCategories
            .AsNoTracking()
            .ToListAsync();
    }
}
