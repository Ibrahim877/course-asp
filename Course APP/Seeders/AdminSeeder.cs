using Course_APP.Models;
using Course_APP.Data;


namespace Course_APP.Seeders
{
    public static class AdminSeeder
    {
        public static void SeedAdminUser(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var adminUser = new User
                {
                    Name = "Zeynəb",
                    Surname = "Əsədzadə",
                    Email = "admin@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Role = "Admin"
                };

                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }

    }
}
