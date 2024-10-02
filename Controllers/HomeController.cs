using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecureWeb.Models;

namespace SecureWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        //ViewData["username"] = "Michele";
        ViewBag.username = "Michele Winne Ananda Kuswo";
        string[] phone = new string[] { "Nokia", "Fleksi", "Xiaomi",};
        ViewBag.phone = phone;

        return View();
    }

    public IActionResult About()
    {
        ViewData["Title"] = "About Page";

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
