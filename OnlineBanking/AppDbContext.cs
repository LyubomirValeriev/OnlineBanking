using Microsoft.EntityFrameworkCore;
using OnlineBanking.Models;

namespace OnlineBanking
{
    public class AppDbContext:DbContext

    {
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                // on cascade delete???
        }

        public DbSet<BankAccount> bankAccounts { get; set; }
    }
}
