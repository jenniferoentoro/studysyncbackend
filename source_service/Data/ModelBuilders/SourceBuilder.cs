using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using source_service.Model;

namespace source_service.Data.ModelBuilders
{
    internal class SourceBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<Source>();

            model.ToTable("Sources");
            model.HasIndex(x => x.Id)
                .IsUnique();

            model.Property(x => x.Id).IsRequired();

            model.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            model.Property(x => x.Description).IsRequired();

            model.Property(x => x.CreatedOn)
                .IsRequired();


            model.Property(x => x.UrlFile)
                .IsRequired()
                .HasMaxLength(100);

            model.Property(x => x.CategoryId).IsRequired();

            model.Property(x => x.UserId).IsRequired();
        }
    }
}