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
        }

        private static void SetupRoles(AppDbContext context)
        {
            if (context.roles.FirstOrDefault() != null) return;

            context.roles.Add(new Models.Role
            {
                role = "User"
            });
            context.roles.Add(new Models.Role
            {
                role = "Admin"
            });

            context.SaveChanges();
        }
    }
}
