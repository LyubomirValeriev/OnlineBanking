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

            modelBuilder.Entity<BankAccount>()
                .HasMany(b => b.transactions)
                .WithOne(t => t.from);
        }

        public DbSet<BankAccount> bankAccounts { get; set; }

        public DbSet<OnlineBanking.Models.Transaction> Transaction { get; set; }
    }
}
