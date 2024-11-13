using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using source_service.Model;

namespace source_service.Data.ModelBuilders
{
    internal class CategoryBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<Category>();

            model.ToTable("Categories");
            model.HasIndex(x => x.Id)
                .IsUnique();

            model.Property(x => x.Id).IsRequired();

            model.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);



        }
    }
}