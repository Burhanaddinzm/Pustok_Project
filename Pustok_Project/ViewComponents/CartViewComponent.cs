using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok_Project.Data.Contexts;
using Pustok_Project.ViewModels;
using Newtonsoft.Json;

namespace Pustok_Project.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        readonly AppDbContext _context;

        public CartViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
