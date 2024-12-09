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
        builder.Property(a => a.ProfileId)
            .IsRequired();

        builder.HasOne(a => a.Governorate)  
            .WithMany()  
            .HasForeignKey(a => a.GovernorateId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.HasOne(a => a.Office)  
            .WithMany()  
            .HasForeignKey(a => a.OfficeId)
            .OnDelete(DeleteBehavior.Restrict);  

        builder.HasOne(a => a.Profile)  
            .WithMany()  
            .HasForeignKey(a => a.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);  

        builder.ToTable("Attendances");
    }
}
