using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsRealm.Models;

namespace NewsRealm.Controllers
{
    public class FrontendController : Controller
    {
        private readonly ILogger<FrontendController> _logger;

        public FrontendController(ILogger<FrontendController> logger)
        {
            _logger = logger;
        }

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
}
