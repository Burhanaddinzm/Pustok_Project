using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok_Project.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; } = null!;
        public List<Product>? Products { get; set; }
        public string ImageUrl { get; set; } = null!;
        [NotMapped]
        public IFormFile Image { get; set; } = null!;
    }
}
