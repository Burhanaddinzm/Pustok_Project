using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Models;

namespace Pustok_Project.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public CategoryViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category>? categories = await _context.Categories.Include(x => x.Parent).ToListAsync();

            return View(categories);
        }
    }
}
