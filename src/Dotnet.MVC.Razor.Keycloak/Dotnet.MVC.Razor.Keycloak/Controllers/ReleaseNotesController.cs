using Dotnet.MVC.Razor.Keycloak.Logics;
using Dotnet.MVC.Razor.Keycloak.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dotnet.MVC.Razor.Keycloak.Controllers;

public class ReleaseNotesController(IReleaseNoteRepository releaseNotepository, IMarkdownRenderer markdown) : Controller
{
    private readonly IReleaseNoteRepository _releaseNoteRepository = releaseNotepository;
    private readonly IMarkdownRenderer _markdown = markdown;

    public async Task<IActionResult> Index()
    {
        var notes = await _releaseNoteRepository.GetAllAsync();

        // Convert Markdown to HTML
        var viewModel = notes.Select(n => new ReleaseNoteViewModel
        {
            Id = n.Id,
            Version = n.Version,
            Title = n.Title,
            HtmlContent = _markdown.ToHtml(n.ContentMarkdown),
            ReleaseDate = n.ReleaseDate
        });

        return View(viewModel);
    }

    [Authorize] // Or allow anonymous if applicable
    [HttpPost]
    public async Task<IActionResult> Seen(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            await _releaseNoteRepository.MarkAsSeenAsync(userId, id);
        }

        return NoContent();
    }

}

