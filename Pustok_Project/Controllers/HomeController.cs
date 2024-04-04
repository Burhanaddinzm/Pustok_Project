using Microsoft.AspNetCore.Mvc;
using Pustok_Project.Data.Contexts;
using System.Diagnostics;

namespace Pustok_Project.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
