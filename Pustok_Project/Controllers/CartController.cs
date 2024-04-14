using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok_Project.Data.Contexts;
using Pustok_Project.ViewModels;

namespace Pustok_Project.Controllers
{
    public class CartController : Controller
    {
        readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<BasketVM>? basketVM = GetBasket();

            List<BasketItemsVM>? basketItemsVM = new List<BasketItemsVM>();

            foreach (var item in basketVM)
            {
                var product = await _context.Products
                    .Include(x => x.ProductImages)
                    .FirstOrDefaultAsync(x => x.Id == item.ProductId);

                BasketItemsVM itemsVM = new BasketItemsVM
                {
                    Count = item.Count,
                    Id = product.Id,
                    Name = product.Name,
                    Image = product.ProductImages.FirstOrDefault(m => m.IsMain).Url,
                };

                if (product.DiscountPrice != 0)
                {
                    itemsVM.Price = product.DiscountPrice;
                }
                else itemsVM.Price = product.Price;


                basketItemsVM.Add(itemsVM);
            }


            return View(basketItemsVM);
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            var existingProduct = await _context.Products.AnyAsync(m => m.Id == id);
            if (!existingProduct) return NotFound();

            List<BasketVM>? Basket = GetBasket();
            BasketVM cartVm = Basket.Find(x => x.ProductId == id);

            if (cartVm != null)
            {
                cartVm.Count++;
            }
            else
            {
                Basket.Add(new BasketVM { Count = 1, ProductId = id });
            }

            Response.Cookies.Append("Basket", JsonConvert.SerializeObject(Basket), new CookieOptions { Expires = DateTime.MaxValue });

            return RedirectToAction("Index");
        }

        public IActionResult DeleteCartItem(int? id)
        {
            List<BasketVM>? Basket = GetBasket();
            if (Basket != null)
            {
                var itemToRemove = Basket.FirstOrDefault(x => x.ProductId == id);

                if (itemToRemove != null)
                    Basket.Remove(itemToRemove);

                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(Basket), new CookieOptions { Expires = DateTime.MaxValue });
            }

            return RedirectToAction("Index");
        }

        List<BasketVM>? GetBasket()
        {
            List<BasketVM> basketVMs;
            if (Request.Cookies["Basket"] != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["Basket"]);
            }
            else basketVMs = new List<BasketVM>();

            return basketVMs;
        }
    }
}
