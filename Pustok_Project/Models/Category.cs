
using Pustok_Project.Areas.Admin.ViewModels;

namespace Pustok_Project.Models
{
    public class Category
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
        public Category? Parent { get; set; }
        public List<Product>? Products { get; set; }
    }
}
