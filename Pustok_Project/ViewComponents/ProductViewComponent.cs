using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Areas.Admin.ViewModels;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Models;
using Pustok_Project.ViewModels;

namespace Pustok_Project.ViewComponents
{
	public class ProductViewComponent : ViewComponent
	{
		readonly AppDbContext _context;

		public ProductViewComponent(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync(int? categoryId, int page = 1, int pageSize = 12)
		{
			var query = _context.Products
							.Include(x => x.Category)
							.Include(x => x.ProductImages);

			var pageCount = GetPageCount(pageSize);

			List<Product>? products;

			PaginateVM paginateVM = new PaginateVM
			{
				CurrentPage = page,
				TotalPageCount = pageCount,
			};

			if (categoryId == null)
			{
				products = await query
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.OrderByDescending(x => x.CreatedAt)
					.AsNoTracking()
					.ToListAsync();

				paginateVM.Products = products;
			}
			else
			{
				products = await query
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.Where(x => x.CategoryId == categoryId)
					.OrderByDescending(x => x.CreatedAt)
					.AsNoTracking()
					.ToListAsync();

				paginateVM.Products = products;
			}

			return View(paginateVM);
		}

		public int GetPageCount(int pageSize)
		{
			var productCount = _context.Products.Count();
			return (int)Math.Ceiling((decimal)productCount / pageSize);
		}
	}
}
