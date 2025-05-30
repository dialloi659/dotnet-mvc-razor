using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dotnet.MVC.Razor.Keycloak.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dotnet.MVC.Razor.Keycloak.Controllers;

[Authorize]
public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
