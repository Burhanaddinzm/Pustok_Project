using Microsoft.AspNetCore.Mvc;

namespace Pustok_Project.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View("404");
        }
    }
}
