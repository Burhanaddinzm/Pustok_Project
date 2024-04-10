using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Enums;

namespace Pustok_Project.ViewComponents
{
    public class TypeProductViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public TypeProductViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products.AsNoTracking()
                           .Include(x => x.ProductImages)
                           .OrderByDescending(x => x.CreatedAt)
                           .Where(x => x.BookType == ((BookType)1))
                           .Take(6)
                           .ToListAsync();
            return View(products);
        }
    }
}
