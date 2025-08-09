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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
           entity.ToTable("Categories");
            entity.HasKey(e => e.Id);
            //Name property configuration
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");
            //CreatedAt property configuration
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");
            //MenuItems navigation property configuration
            entity.HasMany(c => c.MenuItems)
                .WithOne(m => m.Category)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
