using Pustok_Project.Enums;
using Pustok_Project.Models;

namespace Pustok_Project.Areas.Admin.ViewModels
{
    public class ProductEditVM
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal ExTaxPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsInStock { get; set; }
        public double Rating { get; set; }
        public BookType BookType { get; set; }
        public IFormFile? MainImage { get; set; }
        public IFormFile? HoverImage { get; set; }
        public List<IFormFile>? AdditionalImages { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public static implicit operator ProductEditVM(Product product)
        {
            return new ProductEditVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                Rating = product.Rating,
                BookType = product.BookType,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                IsInStock = product.IsInStock,
                ExTaxPrice = product.ExTaxPrice,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
            };
        }
    }
}
