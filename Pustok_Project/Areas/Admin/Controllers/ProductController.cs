using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Models;

namespace Pustok_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product>? products = await _context.Products
                .Include(x => x.Category).Include(x => x.Brand).Include(x => x.ProductImages)
                .AsNoTracking().ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();
            ViewBag.Brands = await _context.Brands.AsNoTracking().ToListAsync();

            return View();
        }
    }
}
