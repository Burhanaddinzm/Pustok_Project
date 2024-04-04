using Pustok_Project.Models;

namespace Pustok_Project.Areas.Admin.ViewModels
{
    public class CategoryVM
    {
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
        public Category? Category { get; set; }
    }
}
