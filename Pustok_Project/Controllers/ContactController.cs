using Microsoft.AspNetCore.Mvc;

namespace Pustok_Project.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
