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
        public async Task<IActionResult> Create(CategoryVM categoryVM)
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();

            if (!ModelState.IsValid) return View(categoryVM);

            if (await _context.Categories.AnyAsync(x => x.Name.ToLower() == categoryVM.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This category already exists!");
                return View(categoryVM);
            }

            await _context.Categories.AddAsync(new Category { Name = categoryVM.Name, ParentId = categoryVM.ParentId });
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();

            var category = await _context.Categories.Include(x => x.Parent)
                           .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();

            if (!ModelState.IsValid) return View(category);

            var existingCategory = await _context.Categories.FindAsync(category.Id);

            if (existingCategory == null) return NotFound();

            if (await _context.Categories.Where(x => x.Id != category.Id).AnyAsync(x => x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "This category already exists!");
                return View(category);
            }

            existingCategory.Name = category.Name;
            existingCategory.ParentId = category.ParentId;

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();

            if (id == null || id == 0) return NotFound();

            Category? category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();

            if (id == null || id == 0) return BadRequest();

            Category? category = _context.Categories.Find(id);

            if (category == null) return BadRequest();

            category.IsDeleted = true;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}
