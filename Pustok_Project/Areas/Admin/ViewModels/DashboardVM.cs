using Pustok_Project.Models;

namespace Pustok_Project.Areas.Admin.ViewModels
{
    public class DashboardVM
    {
        public List<Category>? Categories { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<Product>? Products { get; set; }
    }
}
