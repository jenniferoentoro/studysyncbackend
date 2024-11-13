using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using source_service.Data.ModelBuilders;
using user_service.model;

namespace user_service.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {


        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserBuilder.Build(modelBuilder);
        }
    }
}