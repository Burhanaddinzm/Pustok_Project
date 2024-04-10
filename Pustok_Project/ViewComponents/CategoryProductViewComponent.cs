using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;

namespace Pustok_Project.ViewComponents
{
    public class CategoryProductViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public CategoryProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            ViewBag.CategoryName = categories[0].Name;

            var products = await _context.Products.AsNoTracking()
               .Include(x => x.ProductImages)
               .OrderByDescending(x => x.CreatedAt)
               .Where(x => x.Category.Name == categories[0].Name)
               .Take(7)
               .ToListAsync();
            return View(products);
        }
    }
}
