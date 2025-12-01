using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JJConsulting.Html.Example.Models;

namespace JJConsulting.Html.Example.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}