using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.DamagedPassport;

namespace OMSV1.Infrastructure.Configurations;

public class DamagedTypeConfiguration : IEntityTypeConfiguration<DamagedType>
{
    public void Configure(EntityTypeBuilder<DamagedType> builder)
    {
        builder.HasKey(dt => dt.Id); // Assuming `Entity` has a primary key property `Id`.

        builder.Property(dt => dt.Name)
            .IsRequired()
            .HasMaxLength(100); // Adjust length as per your requirement.

        builder.Property(dt => dt.Description)
            .HasMaxLength(500); // Optional, adjust length as per your requirement.

        // Table Mapping
        builder.ToTable("DamagedTypes");
    }
}
