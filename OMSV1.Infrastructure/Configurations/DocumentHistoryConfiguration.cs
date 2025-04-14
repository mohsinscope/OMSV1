using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.DocumentHistories;
using OMSV1.Domain.Enums;

namespace OMSV1.Infrastructure.Configurations
{
    public class DocumentHistoryConfiguration : IEntityTypeConfiguration<DocumentHistory>
    {
        public void Configure(EntityTypeBuilder<DocumentHistory> builder)
        {
            // Primary Key - assuming "Entity" base class defines Id.
            builder.HasKey(dh => dh.Id);

            // Property configurations
            builder.Property(dh => dh.DocumentId)
                .IsRequired();

            // Change from UserId to ProfileId
            builder.Property(dh => dh.ProfileId)
                .IsRequired();

            builder.Property(dh => dh.ActionType)
                .IsRequired();

            builder.Property(dh => dh.ActionDate)
                .IsRequired();

            builder.Property(dh => dh.Notes)
                .HasMaxLength(1000); // Adjust the length as needed.

            // Relationships

            // Relationship: DocumentHistory -> Document (Required)
            builder.HasOne(dh => dh.Document)
                .WithMany() // Adjust navigation if Document has a collection of histories.
                .HasForeignKey(dh => dh.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: DocumentHistory -> Profile (Required)
            builder.HasOne(dh => dh.Profile)
                .WithMany() // If Profile has a collection of document histories, replace with the proper navigation.
                .HasForeignKey(dh => dh.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table Mapping
            builder.ToTable("DocumentHistories");
        }
    }
}
