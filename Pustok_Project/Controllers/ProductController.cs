using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Models;

namespace Pustok_Project.Controllers
{
	public class ProductController : Controller
	{
		readonly AppDbContext _context;

		public ProductController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{

			return View();
		}

		public async Task<IActionResult> Detail(int? id)
		{
			var product = await _context.Products
								.Include(x => x.Category)
								.Include(x => x.Brand)
								.Include(x => x.ProductImages)
								.AsNoTracking()
								.FirstOrDefaultAsync(x => x.Id == id);

			if (product == null) return RedirectToAction("ErrorNotFound", "Home");
			return View(product);
		}
	}
}
