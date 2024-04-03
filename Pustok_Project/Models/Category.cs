using Pustok_Project.Models.BaseModels;

namespace Pustok_Project.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
        public Category? Parent { get; set; }
        public List<Product>? Products { get; set; }
    }
}
