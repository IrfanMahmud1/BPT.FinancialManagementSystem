using BPT.FMS.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BPT.FMS.UI.Controllers
{
    public class DashboardUiController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public DashboardUiController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
