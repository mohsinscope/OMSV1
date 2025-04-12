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

            // Property configurations
            builder.Property(d => d.DocumentNumber)
                .IsRequired()
                .HasMaxLength(100); // Adjust the max length as needed.

            builder.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(200); // Adjust as required.

            builder.Property(d => d.DocumentType)
                .IsRequired();

            builder.Property(d => d.Subject)
                .HasMaxLength(500); // Optional property with max length.

            builder.Property(d => d.IsRequiresReply)
                .IsRequired();

            builder.Property(d => d.DocumentDate)
                .IsRequired();

            // Relationship: Document -> Project (Required)
            builder.HasOne(d => d.Project)
                .WithMany() // Adjust navigation property in Project if necessary.
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Self-reference Relationship (Parent-Child Documents)
            builder.HasOne(d => d.ParentDocument)
                .WithMany(d => d.ChildDocuments)
                .HasForeignKey(d => d.ParentDocumentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Document -> Party (Required)
            builder.HasOne(d => d.Party)
                .WithMany() // Adjust navigation property in DocumentParty if necessary.
                .HasForeignKey(d => d.PartyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Document -> CC (Optional)
            builder.HasOne(d => d.CC)
                .WithMany() // Adjust navigation property in DocumentParty if necessary.
                .HasForeignKey(d => d.CCId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Configure Attachments
            // Note: This configuration assumes that the AttachmentCU entity has a shadow property "EntityId"
            // used as a foreign key to Document's Id. Adjust as needed if AttachmentCU
            // instead contains a direct navigation property to Document.
            // Table Mapping
            builder.ToTable("Documents");
        }
    }
}
