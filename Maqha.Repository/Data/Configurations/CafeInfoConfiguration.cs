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
    public class CafeInfoConfiguration : IEntityTypeConfiguration<CafeInfo>
    {
        public void Configure(EntityTypeBuilder<CafeInfo> builder)
        {
           //PrimaryKey
           builder.HasKey(c => c.Id);
            //Name Property
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            //Description Property
            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500);
            //Address Property
            builder.Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(500);
            //PhoneNumber Property
            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
            //Email Property
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);
            //ImageUrl Property
            builder.Property(c => c.ImageUrl)
                .IsRequired()
                .HasMaxLength(300);
            //OpeningHours Property
            builder.Property(c => c.OpeningHours)
                .IsRequired()
                .HasMaxLength(100);
            //WebsitUrl Property
            builder.Property(c => c.WebsitUrl)
                .IsRequired()
                .HasMaxLength(200);
            //FacebookUrl Property
            builder.Property(c => c.FacebookUrl)
                .IsRequired()
                .HasMaxLength(200);
            //InstagramUrl Property
            builder.Property(c => c.InstagramUrl)
                .IsRequired()
                .HasMaxLength(200);
            //TwitterUrl Property
            builder.Property(c => c.TwitterUrl)
                .IsRequired()
                .HasMaxLength(200);
            //YoutubeUrl Property
            builder.Property(c => c.YoutubeUrl)
                .IsRequired()
                .HasMaxLength(200);

        }
    }
}
