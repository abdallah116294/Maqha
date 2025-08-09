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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity.ToTable("OrderItems");
            entity.HasKey(oi=>oi.Id);
            entity.Property(oi => oi.OrderId)
                .IsRequired()
                .HasColumnType("int");
            entity.Property(oi => oi.MenuItemId)
                .IsRequired()
                .HasColumnType("int");
            entity.Property(e => e.Quantity)
              .IsRequired()
              .HasColumnType("int");
            entity.Property(e => e.Price)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedAt)
                  .IsRequired()
                  .HasColumnType("datetime");

        }
    }
}
