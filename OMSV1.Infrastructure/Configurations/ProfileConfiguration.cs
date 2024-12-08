using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Profiles;

namespace OMSV1.Infrastructure.Configurations;

public class ProfileConfiguration
{
    // public void Configure(EntityTypeBuilder<Profile> builder)
    // {
    //     // Primary Key Configuration
    //     builder.HasKey(p => p.Id);

    //     // One-to-one relationship between Profile and ApplicationUser
    //     builder.HasOne(p => p.User)  // Profile has one User
    //         .WithOne() // ApplicationUser has one Profile
    //         .HasForeignKey<Profile>(p => p.UserId) // Foreign key is UserId
    //         .OnDelete(DeleteBehavior.Cascade); // Optional cascading delete

    //     // Configure additional properties if necessary
    //     builder.Property(p => p.FirstName)
    //         .IsRequired()  // FirstName is required
    //         .HasMaxLength(100);

    //     builder.Property(p => p.LastName)
    //         .IsRequired()  // LastName is required
    //         .HasMaxLength(100);
    // }
}