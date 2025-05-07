// Infrastructure/Configurations/DocumentTagLinkConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Documents;

namespace OMSV1.Infrastructure.Configurations.Documents
{
    public class DocumentTagLinkConfiguration 
        : IEntityTypeConfiguration<DocumentTagLink>
    {
        public void Configure(EntityTypeBuilder<DocumentTagLink> builder)
        {
            builder.HasKey(l => new { l.DocumentId, l.TagId });

            builder.HasOne(l => l.Document)
                   .WithMany(d => d.TagLinks)
                   .HasForeignKey(l => l.DocumentId);

            builder.HasOne(l => l.Tag)
                   .WithMany(t => t.TagLinks)
                   .HasForeignKey(l => l.TagId);

            builder.ToTable("DocumentTagLinks");
        }
    }
}

