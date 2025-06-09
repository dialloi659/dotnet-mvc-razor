using Dotnet.MVC.Razor.Keycloak.Models.ViewModels;
using Dotnet.MVC.Razor.Keycloak.Persistance;
using Dotnet.MVC.Razor.Keycloak.Persistance.Entities;
using Dotnet.MVC.Razor.Keycloak.Persistance.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.MVC.Razor.Keycloak.Controllers;

[Authorize] // Only allow authenticated users
[Route("feedback")]
public class FeedbackController(AppDbContext context, ILogger<FeedbackController> logger, IHttpContextAccessor httpContextAccessor, IFeedbackRepository feedbackRepository) : Controller
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<FeedbackController> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IFeedbackRepository _feedbackRepository = feedbackRepository;

    //[HttpPost("Create")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create([FromForm] FeedbackReportViewModel model)
    //{
    //    //   if (!ModelState.IsValid)
    //    //   {
    //    //       //var errors = ModelState.Values
    //    //       //    .SelectMany(v => v.Errors)
    //    //       //    .Select(e => e.ErrorMessage);

    //    //       var errors = ModelState
    //    //.Where(ms => ms.Value.Errors.Any())
    //    //.SelectMany(kvp => kvp.Value.Errors.Select(err =>
    //    //    $"'{kvp.Key}' - {err.ErrorMessage}"
    //    //));


    //    //       return BadRequest(new { success = false, errors });
    //    //   }

    //    //if (!ModelState.IsValid)
    //    //{
    //    //    var fieldErrors = ModelState
    //    //        .Where(ms => ms.Value.Errors.Any())
    //    //        .ToDictionary(
    //    //            kvp => kvp.Key,
    //    //            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
    //    //        );

    //    //    return BadRequest(new { success = false, fieldErrors });
    //    //}

    //    if (!ModelState.IsValid)
    //    {
    //        var errors = ModelState
    //            .Where(x => x.Value.Errors.Count > 0)
    //            .SelectMany(kvp => kvp.Value.Errors.Select(e => $"{kvp.Key}:{e.ErrorMessage}"))
    //            .ToList();

    //        return BadRequest(new { success = false, errors });
    //    }


    //    try
    //    {
    //        var feedback = new FeedbackReport
    //        {
    //            Category = model.Category,
    //            Message = model.Message,
    //            PageUrl = model.PageUrl,
    //            Email = model.Email,
    //            SubmittedAt = DateTime.UtcNow,
    //            IsArchived = false
    //        };

    //        _appDbCOntext.FeedbackReports.Add(feedback);
    //        await _appDbCOntext.SaveChangesAsync();

    //        return Ok(new { success = true, message = "Thank you for your feedback!" });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error submitting feedback");
    //        return StatusCode(500, new { success = false, message = "Something went wrong while submitting your feedback." });
    //    }
    //}

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] FeedbackReportViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var fieldErrors = ModelState
                .Where(kv => kv.Value?.Errors.Count > 0)
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors = fieldErrors });
        }

        try
        {
            var feedback = new FeedbackReport
            {
                Category = new FeedbackCategory { Name = model.Category },
                Message = model.Message,
                PageUrl = model.PageUrl,
                Email = model.Email,
                SubmittedAt = DateTime.UtcNow,
                IsArchived = false
            };

            _context.FeedbackReports.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Thank you for your feedback!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting feedback");
            return StatusCode(500, new { success = false, message = "Something went wrong while submitting your feedback." });
        }
    }



    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Submit(FeedbackViewModel model)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest("Invalid feedback data.");

    //    var feedback = new Feedback
    //    {
    //        Type = model.Type,
    //        PageUrl = model.PageUrl,
    //        Email = model.Email,
    //        Message = model.Message,
    //        SubmittedAt = DateTime.UtcNow
    //    };

    //    _appDbCOntext.Feedbacks.Add(feedback);
    //    await _appDbCOntext.SaveChangesAsync();

    //    return Ok();
    //}

}
