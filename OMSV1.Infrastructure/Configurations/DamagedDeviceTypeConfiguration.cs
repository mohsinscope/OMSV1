using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.DamagedDevices;

namespace OMSV1.Infrastructure.Configurations;

public class DamagedDeviceTypeConfiguration : IEntityTypeConfiguration<DamagedDeviceType>
{
    public void Configure(EntityTypeBuilder<DamagedDeviceType> builder)
    {
        builder.HasKey(ddt => ddt.Id); // Assuming `Entity` has a primary key `Id`.

        builder.Property(ddt => ddt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ddt => ddt.Description)
            .HasMaxLength(500); // Optional.

        // Table Mapping
        builder.ToTable("DamagedDeviceTypes");
    }
}