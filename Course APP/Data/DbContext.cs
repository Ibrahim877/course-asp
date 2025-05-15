using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Course_APP.Models;

namespace Course_APP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
