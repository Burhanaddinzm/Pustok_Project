using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Areas.Admin.ViewModels;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Extensions;
using Pustok_Project.Models;
using System;
using System.Drawing;

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

        public async Task<IActionResult> Edit(int? id)
        {

            Brand? brand = await _context.Brands.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (brand == null) return NotFound();

            return View(brand);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Brand brand)
        {
            if (!ModelState.IsValid) return View(brand);

            Brand? existingBrand = await _context.Brands.FindAsync(brand.Id);

            if (existingBrand == null) return NotFound();

            if (!brand.Image.CheckFileType("image"))
            {
                ModelState.AddModelError("", "Invalid file type!");
                return View(brand);
            }
            if (!brand.Image.CheckFileSize(2))
            {
                ModelState.AddModelError("", "File size too big!");
                return View(brand);
            }
            if (await _context.Brands.Where(x => x.Id != brand.Id).AnyAsync(x => x.Name == brand.Name))
            {
                ModelState.AddModelError("Name", "This category already exists!");
                return View(brand);
            }

            string uniqueFileName = await brand.Image.SaveFileAsync(_env.WebRootPath, "client", "assets", "image", "brands");

            brand.Image.DeleteFile(_env.WebRootPath, "client", "assets", "image", "brands", brand.ImageUrl);

            existingBrand.Name = brand.Name;
            existingBrand.ImageUrl = uniqueFileName;


            _context.Brands.Update(existingBrand);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            Brand? brand = await _context.Brands.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (brand == null) return NotFound();

            return View(brand);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            if (id == null || id == 0) return BadRequest();

            Brand? brand = await _context.Brands.FindAsync(id);

            if (brand == null) return BadRequest();

            brand.IsDeleted = true;

            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
    }
}
