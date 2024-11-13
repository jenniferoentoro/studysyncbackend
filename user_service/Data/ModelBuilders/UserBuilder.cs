using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using user_service.model;

namespace source_service.Data.ModelBuilders
{
    internal class UserBuilder
    {
        public static void Build(ModelBuilder modelBuilder)
        {
            var model = modelBuilder.Entity<User>();

            model.ToTable("Users");
            model.HasIndex(x => x.Id)
                .IsUnique();

            model.Property(x => x.Id).IsRequired();

            model.Property(x => x.Email).IsRequired();
            model.Property(x => x.Name)
                .IsRequired();

            model.Property(x => x.Grade).IsRequired();

            model.Property(x => x.School).IsRequired();

            model.Property(x => x.Role).IsRequired();



        }
    }
}