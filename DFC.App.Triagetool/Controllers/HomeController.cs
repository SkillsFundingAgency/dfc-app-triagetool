using DFC.App.Triagetool.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DFC.App.Triagetool.Controllers
{
    public class HomeController : Controller
    {
        public const string ThisViewCanonicalName = "home";

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View("~/Views/Pages/Index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
