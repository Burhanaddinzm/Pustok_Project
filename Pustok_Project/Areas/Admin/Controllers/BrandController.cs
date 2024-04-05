using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Areas.Admin.ViewModels;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Extensions;
using Pustok_Project.Models;

namespace Pustok_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Brand>? brands = await _context.Brands.Include(x => x.Products).AsNoTracking().ToListAsync();

            return View(brands);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BrandVM brandVM)
        {
            if (!ModelState.IsValid) return View(brandVM);

            if (!brandVM.Image.CheckFileType("image"))
            {
                ModelState.AddModelError("", "Invalid file type!");
                return View(brandVM);
            }
            if (!brandVM.Image.CheckFileSize(2))
            {
                ModelState.AddModelError("", "File size too big!");
                return View(brandVM);
            }
            if (await _context.Brands.AnyAsync(x => x.Name.ToLower() == brandVM.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This brand already exists!");
                return View(brandVM);
            }

            string uniqueFileName = await brandVM.Image.SaveFileAsync(_env.WebRootPath, "client", "assets", "image", "brands");

            Brand brand = new Brand
            {
                Name = brandVM.Name,
                ImageUrl = uniqueFileName
            };

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}
