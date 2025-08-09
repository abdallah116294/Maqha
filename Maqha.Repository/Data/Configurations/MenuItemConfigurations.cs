using Maqha.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Repository.Data.Configurations
{
    public class MenuItemConfigurations : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> entity)
        {
            entity.ToTable("MenuItems");
            entity.HasKey(e => e.Id);
            //Name property configuration
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");
            //Decription property configuration
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");
            //Price property configuration
            entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            //Image property configuration
            entity.Property(e => e.Image)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("nvarchar(255)");
            //IsAvailable property configuration
            entity.Property(e => e.IsAvailable)
                .IsRequired()
                .HasColumnType("bit")
                .HasDefaultValue(true);
            //CategoryId property configuration
            entity.HasOne(e=>e.Category).WithMany(c => c.MenuItems)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade); // Set delete behavior to cascade

            //CreatedAt property configuration
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");
        }
    }
}
