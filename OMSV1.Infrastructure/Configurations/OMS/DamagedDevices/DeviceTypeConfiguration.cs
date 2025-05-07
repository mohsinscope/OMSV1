using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.DamagedDevices;

namespace OMSV1.Infrastructure.Configurations;

public class DeviceTypeConfiguration : IEntityTypeConfiguration<DeviceType>
{
    public void Configure(EntityTypeBuilder<DeviceType> builder)
    {
        builder.HasKey(dt => dt.Id); // Assuming `Entity` has a primary key `Id`.

        builder.Property(dt => dt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(dt => dt.Description)
            .HasMaxLength(500); // Optional.

        // Table Mapping
        builder.ToTable("DeviceTypes");
    }
}