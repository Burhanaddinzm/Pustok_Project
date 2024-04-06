using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Areas.Admin.ViewModels;
using Pustok_Project.Data.Contexts;

namespace Pustok_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            DashboardVM vm = new DashboardVM
            {
                Categories = await _context.Categories.Include(x => x.Products).Include(x => x.Parent).AsNoTracking().ToListAsync(),
                Brands = await _context.Brands.Include(x => x.Products).AsNoTracking().ToListAsync(),
                Products = await _context.Products.Include(x => x.Category).Include(x => x.Brand).Include(x => x.ProductImages).AsNoTracking().ToListAsync()
            };

            return View(vm);
        }
    }
}
