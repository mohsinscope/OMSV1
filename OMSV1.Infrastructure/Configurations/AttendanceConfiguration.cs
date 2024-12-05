using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Attendances;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;

namespace OMSV1.Infrastructure.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasKey(a => a.Id); 

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ReceivingStaff)
            .IsRequired();

        builder.Property(a => a.AccountStaff)
            .IsRequired();

        builder.Property(a => a.PrintingStaff)
            .IsRequired();

        builder.Property(a => a.QualityStaff)
            .IsRequired();

        builder.Property(a => a.DeliveryStaff)
            .IsRequired();

        builder.Property(a => a.Note)
            .HasMaxLength(500); 

        builder.Property(a => a.Date)
            .IsRequired();

        builder.Property(a => a.WorkingHours)
            .IsRequired()
            .HasConversion<int>(); 

        builder.Property(a => a.OfficeId)
            .IsRequired();

        builder.Property(a => a.GovernorateId)
            .IsRequired();

        // Relationship Configuration: Attendance -> Governorate
        builder.HasOne(a => a.Governorate)  // Explicitly specify the navigation property here
            .WithMany()  // Assuming Governorate doesn't have a navigation property back to Attendance
            .HasForeignKey(a => a.GovernorateId)
            .OnDelete(DeleteBehavior.Restrict);  // Adjust delete behavior if necessary

        // Relationship Configuration: Attendance -> Office
        builder.HasOne(a => a.Office)  // Explicitly specify the navigation property here
            .WithMany()  // Assuming Office doesn't have a navigation property back to Attendance
            .HasForeignKey(a => a.OfficeId)
            .OnDelete(DeleteBehavior.Restrict);  // Adjust delete behavior if necessary

        builder.ToTable("Attendances");
    }
}
