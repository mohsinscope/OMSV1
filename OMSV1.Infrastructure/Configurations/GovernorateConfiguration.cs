using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Governorates;

namespace OMSV1.Infrastructure.Configurations
{
    public class GovernorateConfiguration : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder.HasKey(g => g.Id); // Assuming `Entity` has an `Id` as the primary key.

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.Code)
                .IsRequired()
                .HasMaxLength(50);

            // Configure the IsCountry property as nullable.
            builder.Property(g => g.IsCountry)
                .IsRequired(false);

            // Relationship
            builder.HasMany(g => g.Offices)
                .WithOne(o => o.Governorate)
                .HasForeignKey(o => o.GovernorateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table Mapping
            builder.ToTable("Governorates");
        }
    }
}
