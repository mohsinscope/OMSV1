using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Attachments;
using OMSV1.Domain.Entities.DamagedPassport;

namespace OMSV1.Infrastructure.Configurations;


public class DamagedPassportConfiguration : IEntityTypeConfiguration<DamagedPassport>
{
    public void Configure(EntityTypeBuilder<DamagedPassport> builder)
    {
        builder.HasKey(dp => dp.Id); // Assuming `Entity` has a primary key property `Id`.

        builder.Property(dp => dp.PassportNumber)
            .IsRequired()
            .HasMaxLength(50); // Adjust length as per your requirement.

        builder.Property(dp => dp.Date)
            .IsRequired();

        builder.Property(dp => dp.DamagedTypeId)
            .IsRequired();

        builder.Property(a => a.Note)
            .HasMaxLength(500); 

        builder.Property(dp => dp.OfficeId)
            .IsRequired();

        builder.Property(dp => dp.GovernorateId)
            .IsRequired();

        builder.Property(dp => dp.ProfileId)
            .IsRequired();

        // Relationships
        builder.HasOne(dp => dp.Governorate)
            .WithMany() // Adjust navigation property as needed.
            .HasForeignKey(dp => dp.GovernorateId) 
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dp => dp.Office)
            .WithMany() // Adjust navigation property as needed.
            .HasForeignKey(dp => dp.OfficeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dp => dp.Profile)
            .WithMany() // Adjust navigation property as needed.
            .HasForeignKey(dp => dp.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dp => dp.DamagedType)
            .WithMany() // Adjust navigation property as needed.
            .HasForeignKey(dp => dp.DamagedTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // builder.HasMany(a => a.Attachments)
        //     .WithOne()
        //     .HasForeignKey(a => a.EntityId)
        //     .HasPrincipalKey(dp => dp.Id)
        //     .OnDelete(DeleteBehavior.Cascade)
        //     .HasConstraintName("FK_DamagedPassport_Attachments")
        //     .IsRequired(false)
        //     .HasAnnotation("EntityType", OMSV1.Domain.Enums.EntityType.DamagedPassport);


        // Table Mapping
        builder.ToTable("DamagedPassports");
    }

}
