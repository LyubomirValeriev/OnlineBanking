using Microsoft.EntityFrameworkCore;
using OnlineBanking.Models;

namespace OnlineBanking
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
    }
}
