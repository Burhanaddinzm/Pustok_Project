using Pustok_Project.Models.BaseModels;

namespace Pustok_Project.Models
{
    public class Brand : BaseModel
    {
        public string Name { get; set; } = null!;
        public List<Product>? Products { get; set; }
    }
}
