using Pustok_Project.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok_Project.Models
{
    public class ProductImage : BaseModel
    {
        public string Url { get; set; } = null!;
        [NotMapped]
        public IFormFile File { get; set; } = null!;
        public bool IsMain { get; set; }
        public bool IsHover { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
