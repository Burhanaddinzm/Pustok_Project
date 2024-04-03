
namespace Pustok_Project.Models.BaseModels
{
    public class BaseModel : IAuditable
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
