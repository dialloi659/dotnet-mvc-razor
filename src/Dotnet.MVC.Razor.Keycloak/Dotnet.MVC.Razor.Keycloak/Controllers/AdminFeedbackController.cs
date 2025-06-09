using Dotnet.MVC.Razor.Keycloak.Models;
using Dotnet.MVC.Razor.Keycloak.Models.ViewModels;
using Dotnet.MVC.Razor.Keycloak.Persistance;
using Dotnet.MVC.Razor.Keycloak.Persistance.Filters;
using Dotnet.MVC.Razor.Keycloak.Persistance.Paging;
using Dotnet.MVC.Razor.Keycloak.Persistance.Repositories;
using Dotnet.MVC.Razor.Keycloak.Persistance.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.MVC.Razor.Keycloak.Controllers;

//[Authorize(Roles = "Admin,Support")]
[Route("admin/feedback")]
public class AdminFeedbackController(AppDbContext context, IFeedbackRepository feedbackRepository) : Controller
{
    private readonly AppDbContext _context = context;
    private readonly IFeedbackRepository _feedbacReposiotry = feedbackRepository;

    public async Task<IActionResult> Index()
    {
        // Pass categories for filter dropdown
        var categories = await _feedbacReposiotry.GetAllCategoriesAsync(); // define this in your repo
        var dashboardModelView = new FeedbackDashboardViewModel
        {
            Categories = categories
        };

        return View(dashboardModelView);
    }

    [HttpGet("FeedbackReports")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetFeedbackReports([FromQuery] PagedRequest<FeedbackFilter> request, CancellationToken cancellationToken = default)
    {
        if (request == null) return BadRequest("Invalid request payload.");

        // Fetch cursor-paged, filtered, searched, ordered data
        var page = await _feedbacReposiotry.GetPagedAsync(request, cancellationToken);

        // DataTables expects { draw, recordsTotal, recordsFiltered, data }
        return Json(new
        {
            draw = request.Filters?.Draw ?? 0,
            recordsTotal = page.Items.Count,    // DataTables wants a number; true total omitted for performance
            recordsFiltered = page.Items.Count, // same as above
            data = page.Items,
            nextCursor = page.NextCursor,
            hasMore = page.HasMore
        });
    }

    [HttpGet("FeedbackReports/Index")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetFeedbackReportsBaseIndex([FromQuery] IndexedPagedRequest<FeedbackFilter> request, CancellationToken cancellationToken = default)
    {
        if (request == null) return BadRequest("Invalid request payload.");

        var pageBaseIndex = await _feedbacReposiotry.GetIndexedPageAsync(request, cancellationToken);

        // DataTables expects { draw, recordsTotal, recordsFiltered, data }
        return Json(new
        {
            draw = request.Filters?.Draw ?? 0,
            recordsTotal = pageBaseIndex.TotalRecords,
            recordsFiltered = pageBaseIndex.TotalRecords,
            data = pageBaseIndex.Items
        });
    }

    [HttpPost("UpdateStatus")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(ChangeStatusRequest request)
    {
        var feedback = await _context.FeedbackReports.FindAsync(request.Id);
        if (feedback == null) return NotFound();

        feedback.Status = request.Status;
        ViewBag.NewFeedbackCount = await _context.FeedbackReports.CountAsync(f => f.Status == FeedbackStatus.New);
        await _context.SaveChangesAsync();

        return Ok(new { status = true });
    }

    [HttpPost("Archive")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Archive(int id, bool? isArchived)
    {
        var feedback = await _context.FeedbackReports.FindAsync(id);
        if (feedback is null) return NotFound();
        if (isArchived is null) return BadRequest("Invalid archive status.");

        feedback.IsArchived = isArchived.Value;

        //ViewBag.NewFeedbackCount = await _context.FeedbackReports.CountAsync(f => !f.IsArchived);
        await _context.SaveChangesAsync();

        return Ok(new { status = true });
    }
}
