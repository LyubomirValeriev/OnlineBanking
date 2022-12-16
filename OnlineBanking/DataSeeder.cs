namespace OnlineBanking
{
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
            SetupRoles(context);
            setupAccounts(context);
        }

        private static void SetupRoles(AppDbContext context)
        {
            if (context.roles.FirstOrDefault() == null)
            {
                context.roles.Add(new Models.Role
                {
                    role = "User"
                });
               
                context.roles.Add(new Models.Role
                {
                    role = "Admin"
                });
            }
                

            context.SaveChanges();
        }

        private static void setupAccounts(AppDbContext context)
        {
            if (context.users.Where(u => u.UserUsername.Equals("admin")).FirstOrDefault() == null)
            {
                var adminRole = context.roles.Where(r => r.role.Equals("Admin")).FirstOrDefault();
                context.users.Add(new Models.User
                {
                    UserFirstName = "Admin",
                    UserLastName = "ADmin",
                    UserUsername = "admin",
                    password = BCrypt.Net.BCrypt.HashPassword("admin"),
                    email = "admin@bank.com",
                    Active = true,
                    verificationCode = "",
                    role = adminRole,
                    bankAccount = null

                });
            }
        }
    }
}
