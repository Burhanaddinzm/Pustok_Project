using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;

namespace Pustok_Project.ViewComponents
{
    public class SliderNondiscountedProductViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public SliderNondiscountedProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products.AsNoTracking()
                           .Include(x => x.ProductImages)
                           .OrderByDescending(x => x.CreatedAt)
                           .Where(x => x.DiscountPrice == 0)
                           .Take(12)
                           .ToListAsync();
            return View(products);
        }
    }
}
