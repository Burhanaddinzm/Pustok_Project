using Pustok_Project.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok_Project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
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
        [NotMapped]
        public IFormFile MainImage { get; set; } = null!;
        [NotMapped]
        public IFormFile HoverImage { get; set; } = null!;
        [NotMapped]
        public List<IFormFile>? AdditionalImages { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
