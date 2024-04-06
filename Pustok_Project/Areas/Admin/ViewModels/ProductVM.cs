using Pustok_Project.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok_Project.Areas.Admin.ViewModels
{
    public class ProductVM
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal ExTaxPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsInStock { get; set; }
        public double Rating { get; set; }
        public BookType BookType { get; set; }
        public IFormFile MainImage { get; set; } = null!;
        public IFormFile HoverImage { get; set; } = null!;
        public List<IFormFile>? AdditionalImages { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }
}
