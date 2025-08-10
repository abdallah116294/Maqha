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
    public class TableConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> entity)
        {
            entity.ToTable("Tables");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.TableNumber)
                  .IsRequired()
                  .HasMaxLength(250)
                  .HasColumnType("int");

            entity.Property(e => e.Capactiy)
                  .IsRequired()
                  .HasColumnType("int");

            entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
              .HasColumnType("datetime")
              .IsRequired();
        }
    }
}
