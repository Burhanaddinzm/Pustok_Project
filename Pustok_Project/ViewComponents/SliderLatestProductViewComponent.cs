using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;

namespace Pustok_Project.ViewComponents
{
    public class SliderLatestProductViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public SliderLatestProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products.AsNoTracking()
                           .Include(x => x.ProductImages)
                           .OrderByDescending(x => x.CreatedAt)
                           .Take(12)
                           .ToListAsync();
            return View(products);
        }
    }
}
