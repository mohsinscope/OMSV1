using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Profiles;

namespace OMSV1.Infrastructure.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            // Primary key
            builder.HasKey(p => p.Id);

            // Configure the relationship with IdentityUser
            builder.HasOne<IdentityUser>()
                   .WithOne()
                   .HasForeignKey<Profile>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configure properties
            builder.Property(p => p.FirstName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.LastName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.DateOfBirth)
                   .IsRequired();

            // Table name
            builder.ToTable("Profiles");
        }
    }
