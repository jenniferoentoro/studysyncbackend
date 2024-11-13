using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using source_service.Data.ModelBuilders;
using source_service.Model;

namespace source_service.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        public DbSet<Source> Sources { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SourceBuilder.Build(modelBuilder);
            CategoryBuilder.Build(modelBuilder);
        }

    }
}