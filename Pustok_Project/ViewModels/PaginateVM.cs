using Pustok_Project.Models;

namespace Pustok_Project.ViewModels
{
	public class PaginateVM
	{
		public int CurrentPage { get; set; }
		public int TotalPageCount { get; set; }
		public List<Product> Products { get; set; }
	}
}
