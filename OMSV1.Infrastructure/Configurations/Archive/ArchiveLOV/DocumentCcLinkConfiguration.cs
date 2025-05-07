// Infrastructure/Configurations/DocumentCcLinkConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Documents;

namespace OMSV1.Infrastructure.Configurations;

public class DocumentCcLinkConfiguration : IEntityTypeConfiguration<DocumentCcLink>
{
    public void Configure(EntityTypeBuilder<DocumentCcLink> builder)
    {
        builder.HasKey(l => new { l.DocumentId, l.DocumentCcId });

        builder.HasOne(l => l.Document)
               .WithMany(d => d.CcLinks)
               .HasForeignKey(l => l.DocumentId);

        builder.HasOne(l => l.DocumentCc)
               .WithMany(cc => cc.DocumentLinks)
               .HasForeignKey(l => l.DocumentCcId);

        builder.ToTable("DocumentCcLinks");
    }
}
