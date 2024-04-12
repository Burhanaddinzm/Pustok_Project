using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Models;
using Pustok_Project.ViewModels;

namespace Pustok_Project.Controllers
{
    public class ProductController : Controller
    {
        readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId, string? searchedName, int page = 1, int pageSize = 12)
        {
            ViewData["CategoryId"] = categoryId;
            ViewData["PageSize"] = pageSize;
            ViewData["SearchedName"] = searchedName;

            var query = _context.Products
                        .Include(x => x.Category)
                        .Include(x => x.ProductImages);

            int pageCount = 0;

            List<Product>? products;

            PaginateVM paginateVM = new PaginateVM
            {
                CurrentPage = page,
            };

            if (categoryId != null)
            {
                products = await query
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .Where(x => x.CategoryId == categoryId)
                     .OrderByDescending(x => x.CreatedAt)
                     .AsNoTracking()
                     .ToListAsync();

                pageCount = GetPageCount(pageSize, categoryId);
                paginateVM.TotalPageCount = pageCount;

                paginateVM.Products = products;
            }
            else if (searchedName != null)
            {
                products = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Where(x => x.Name.ToLower().StartsWith(searchedName.ToLower().Trim()))
                    .OrderByDescending(x => x.CreatedAt)
                    .AsNoTracking()
                    .ToListAsync();

                pageCount = GetPageCount(pageSize, 0, searchedName);
                paginateVM.TotalPageCount = pageCount;

                paginateVM.Products = products;
            }
            else
            {
                products = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.CreatedAt)
                    .AsNoTracking()
                    .ToListAsync();

                pageCount = GetPageCount(pageSize);
                paginateVM.TotalPageCount = pageCount;

                paginateVM.Products = products;
            }

            if (paginateVM.TotalPageCount == 0) return RedirectToAction("ErrorNotFound", "Home");

            return View(paginateVM);
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

        public int GetPageCount(int pageSize, int? categoryId = 0, string? search = null)
        {
            int productCount = 0;

            if (categoryId != 0)
            {
                productCount = _context.Products.AsNoTracking().Where(x => x.CategoryId == categoryId).Count();
            }
            else if (search != null)
            {
                productCount = _context.Products.AsNoTracking().Where(x => x.Name.ToLower().StartsWith(search.ToLower().Trim())).Count();
            }
            else
            {
                productCount = _context.Products.AsNoTracking().Count();
            }

            return (int)Math.Ceiling((decimal)productCount / pageSize);
        }
    }
}
