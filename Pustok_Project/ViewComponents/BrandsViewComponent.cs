using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Models;

namespace Pustok_Project.ViewComponents
{
    public class BrandsViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public BrandsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Brand>? brands = await _context.Brands.AsNoTracking().ToListAsync();

            return View(brands);
        }
    }
}
