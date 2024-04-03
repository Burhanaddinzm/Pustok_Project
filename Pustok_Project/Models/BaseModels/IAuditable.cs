namespace Pustok_Project.Models.BaseModels
{
    public interface IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
