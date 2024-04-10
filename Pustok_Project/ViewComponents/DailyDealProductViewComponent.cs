using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;

namespace Pustok_Project.ViewComponents
{
    public class DailyDealProductViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public DailyDealProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products.AsNoTracking()
                           .Include(x => x.ProductImages)
                           .OrderByDescending(x => x.CreatedAt)
                           .Where(x => x.DiscountPrice != 0 && Math.Round((x.Price - x.DiscountPrice) / x.Price * 100) < 20)
                           .Take(7)
                           .ToListAsync();
            return View(products);
        }
    }
}
