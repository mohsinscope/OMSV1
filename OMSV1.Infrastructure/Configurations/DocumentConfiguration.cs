using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Documents;
using OMSV1.Domain.Enums;

namespace OMSV1.Infrastructure.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            // Primary Key
            builder.HasKey(d => d.Id);

            // Ensure DocumentNumber is unique.
            builder.HasIndex(d => d.DocumentNumber).IsUnique();

            // Property configurations
            builder.Property(d => d.DocumentNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.DocumentType)
                .IsRequired();

            // Configure ResponseType as optional since not all documents may have a response.
            builder.Property(d => d.ResponseType)
                .IsRequired(true);

            builder.Property(d => d.Subject)
                .HasMaxLength(500);

            builder.Property(d => d.IsRequiresReply)
                .IsRequired();

            builder.Property(d => d.DocumentDate)
                .IsRequired();

            // New boolean flags, default to false.
            builder.Property(d => d.IsReplied)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(d => d.IsAudited)
                .HasDefaultValue(false)
                .IsRequired();

            // Relationship: Document -> Project (Required)
            builder.HasOne(d => d.Project)
                .WithMany() 
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Self-reference Relationship (Parent-Child Documents)
            builder.HasOne(d => d.ParentDocument)
                .WithMany(d => d.ChildDocuments)
                .HasForeignKey(d => d.ParentDocumentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Document -> Party (Required)
            builder.HasOne(d => d.Party)
                .WithMany()
                .HasForeignKey(d => d.PartyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Document -> CC (Optional)
            builder.HasOne(d => d.CC)
                .WithMany()
                .HasForeignKey(d => d.CCId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // New: Relationship to Profile (the main creator)
            builder.HasOne(d => d.Profile)
                .WithMany() // Adjust navigation on the Profile entity if necessary.
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // Table Mapping
            builder.ToTable("Documents");
        }
    }
}
