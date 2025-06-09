using Dotnet.MVC.Razor.Keycloak.Data.Entities;
using Dotnet.MVC.Razor.Keycloak.Logics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.MVC.Razor.Keycloak.Controllers;

//[Authorize(Roles = "Admin")]
public class AdminReleaseNotesController(IReleaseNoteRepository service) : Controller
{
    private readonly IReleaseNoteRepository _service = service;

    public async Task<IActionResult> Index()
    {
        var notes = await _service.GetAllAsync();
        return View(notes);
    }

    public IActionResult Create()
    {
        return View(new ReleaseNote());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ReleaseNote note)
    {
        if (!ModelState.IsValid) return View(note);

        await _service.AddAsync(note);

        TempData["Success"] = "Release note created successfully.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var note = await _service.GetByIdAsync(id);
        return note == null ? NotFound() : View(note);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var note = await _service.GetByIdAsync(id);
        return note == null ? NotFound() : View(note);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ReleaseNote note)
    {
        if (!ModelState.IsValid) return View(note);

        await _service.UpdateAsync(note);
        TempData["Success"] = "Release note updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var note = await _service.GetByIdAsync(id);
        return note == null ? NotFound() : View(note);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        await _service.DeleteAsync(id);
        TempData["Success"] = "Release note deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
