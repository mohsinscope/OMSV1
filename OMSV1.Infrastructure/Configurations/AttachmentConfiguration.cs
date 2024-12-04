using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Attachments;

namespace OMSV1.Infrastructure.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<AttachmentCU>
{
    public void Configure(EntityTypeBuilder<AttachmentCU> builder)
    {
        builder.HasKey(a => a.Id); // Assuming `Entity` has an `Id` as the primary key.

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Url)
            .IsRequired()
            .HasMaxLength(200); // Adjust length as needed.

        builder.Property(a => a.DamagedDeviceId)
            .IsRequired();

        // Relationship
        // builder.HasOne(a => a.DamagedDevice)
        //       .WithMany(dd => dd.AttachmentCUs)
        //       .HasForeignKey(a => a.DamagedDeviceId)
        //       .OnDelete(DeleteBehavior.Restrict);

        // Table Mapping
        builder.ToTable("AttachmentCUs");
    }
}