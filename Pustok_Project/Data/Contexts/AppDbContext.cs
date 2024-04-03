using Microsoft.EntityFrameworkCore;

namespace Pustok_Project.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
