using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Areas.Admin.ViewModels;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Models;
using System.Drawing;

namespace Pustok_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories
                .Include(x => x.Products).Include(x => x.Parent)
                .AsNoTracking().ToListAsync();

            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryVM category)
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();

            if (!ModelState.IsValid) return View(category);

            if (await _context.Categories.AnyAsync(x => x.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This category already exists!");
                return View(category);
            }

            await _context.Categories.AddAsync(new Category { Name = category.Name, ParentId = category.ParentId });
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _context.Categories.Include(x => x.Parent)
                           .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound();

            return View(new CategoryVM { Category = category });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, CategoryVM category)
        {
            return RedirectToAction("index");
        }
    }
}
