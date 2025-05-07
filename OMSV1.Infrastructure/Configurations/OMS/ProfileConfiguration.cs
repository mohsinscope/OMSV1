using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Infrastructure.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        // Primary key
        builder.HasKey(p => p.Id);

        // Configure the relationship with IdentityUser
        builder.HasOne<ApplicationUser>() // Specify the integer key type for IdentityUser
               .WithOne()
               .HasForeignKey<Profile>(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        // Configure properties
        builder.Property(p => p.FullName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(p => p.Position)
               .IsRequired()
               .HasConversion<int>(); // Convert enum to int for database storage

        // Relationships with Office and Governorate
        builder.HasOne(p => p.Office)
               .WithMany()
               .HasForeignKey(p => p.OfficeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Governorate)
               .WithMany()
               .HasForeignKey(p => p.GovernorateId)
               .OnDelete(DeleteBehavior.Restrict);

        // Table name
        builder.ToTable("Profiles");
    }
}
