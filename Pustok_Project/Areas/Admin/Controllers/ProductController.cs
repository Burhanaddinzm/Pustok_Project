using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Areas.Admin.ViewModels;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Extensions;
using Pustok_Project.Models;

namespace Pustok_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product>? products = await _context.Products
                .Include(x => x.Category).Include(x => x.Brand).Include(x => x.ProductImages)
                .AsNoTracking().ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();
            ViewBag.Brands = await _context.Brands.AsNoTracking().ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductVM productVM)
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();
            ViewBag.Brands = await _context.Brands.AsNoTracking().ToListAsync();

            if (!ModelState.IsValid) return View(productVM);

            if (await _context.Products.AnyAsync(x => x.Name.ToLower() == productVM.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This product already exists!");
                return View(productVM);
            }

            var productImages = new List<ProductImage>();

            if (productVM.AdditionalImages != null)
            {
                if (!await ValidateAndCreateImageAsync(productVM.AdditionalImages, productImages))
                    return View(productVM);
            }

            if (!await ValidateAndCreateImageAsync(new List<IFormFile> { productVM.MainImage }, productImages))
                return View(productVM);

            if (!await ValidateAndCreateImageAsync(new List<IFormFile> { productVM.HoverImage }, productImages))
                return View(productVM);

            Product product = new Product
            {
                Name = productVM.Name,
                Description = productVM.Description,
                ExTaxPrice = productVM.ExTaxPrice,
                DiscountPrice = productVM.DiscountPrice,
                Price = productVM.Price,
                Rating = productVM.Rating,
                IsInStock = productVM.IsInStock,
                BookType = productVM.BookType,
                ProductImages = productImages,
                BrandId = productVM.BrandId,
                CategoryId = productVM.CategoryId,
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();
            ViewBag.Brands = await _context.Brands.AsNoTracking().ToListAsync();

            if (id == null || id == 0) return NotFound();

            Product? product = await _context.Products.AsNoTracking()
                .Include(x => x.ProductImages).Include(x => x.Category).Include(x => x.Brand).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            ProductEditVM vm = product;

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditVM product)
        {
            ViewBag.Categories = await _context.Categories.AsNoTracking().ToListAsync();
            ViewBag.Brands = await _context.Brands.AsNoTracking().ToListAsync();

            if (!ModelState.IsValid) return View(product);

            Product? existingProduct = await _context.Products.Include(x => x.Category).Include(x => x.Brand).Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == product.Id);

            if (existingProduct == null) return BadRequest();

            if (await _context.Products.Where(x => x.Id != product.Id).AnyAsync(x => x.Name.ToLower() == product.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This product already exists!");
                return View(product);
            }

            var productImages = new List<ProductImage>();

            if (product.AdditionalImages != null)
            {
                if (!await ValidateAndCreateImageAsync(product.AdditionalImages, productImages))
                    return View(product);
            }

            if (product.MainImage != null)
            {
                var mainImage = existingProduct.ProductImages.FirstOrDefault(x => x.IsMain);
                if (mainImage != null)
                {
                    mainImage.IsMain = false;
                }

                if (!await ValidateAndCreateImageAsync(new List<IFormFile> { product.MainImage }, productImages))
                    return View(product);
            }

            if (product.HoverImage != null)
            {
                var hoverImage = existingProduct.ProductImages.FirstOrDefault(x => x.IsHover);
                if (hoverImage != null)
                {
                    hoverImage.IsHover = false;
                }

                if (!await ValidateAndCreateImageAsync(new List<IFormFile> { product.HoverImage }, productImages))
                    return View(product);
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.ExTaxPrice = product.ExTaxPrice;
            existingProduct.DiscountPrice = product.DiscountPrice;
            existingProduct.Price = product.Price;
            existingProduct.Rating = product.Rating;
            existingProduct.BookType = product.BookType;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.BrandId = product.BrandId;
            existingProduct.IsInStock = product.IsInStock;

            foreach (var image in productImages)
            {
                existingProduct.ProductImages.Add(image);
            }

            _context.Update(existingProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            Product? product = await _context.Products
                .Include(x => x.ProductImages).Include(x => x.Brand).Include(x => x.Category)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            if (id == null || id == 0) return BadRequest();

            Product? product = await _context.Products.FindAsync(id);

            if (product == null) return BadRequest();

            product.IsDeleted = true;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id == 0) return NotFound();

            Product? product = await _context.Products
                .Include(x => x.Brand).Include(x => x.Category).Include(x => x.ProductImages)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        async Task<bool> ValidateAndCreateImageAsync(List<IFormFile> files, List<ProductImage> productImages)
        {
            foreach (var file in files)
            {
                var validationResult = file.ValidateFile();
                if (!validationResult.IsValid)
                {
                    ModelState.AddModelError(file.Name, validationResult.ErrorMessage);
                    return false;
                }
                productImages.Add(await CreateProductImageAsync(file, file.Name == "MainImage", file.Name == "HoverImage"));
            }
            return true;
        }
        async Task<ProductImage> CreateProductImageAsync(IFormFile file, bool isMain, bool isHover)
        {
            string uniqueFileName = await file.SaveFileAsync(_env.WebRootPath, "client", "assets", "image", "productss");

            ProductImage productImage = new ProductImage
            {
                Url = uniqueFileName,
                IsMain = isMain,
                IsHover = isHover
            };

            return productImage;
        }
    }
}
