// Infrastructure/Configurations/DocumentCCConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Documents;

namespace OMSV1.Infrastructure.Configurations;

public class DocumentCCConfiguration : IEntityTypeConfiguration<DocumentCC>
{
    public void Configure(EntityTypeBuilder<DocumentCC> builder)
    {
        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.RecipientName)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasMany(cc => cc.DocumentLinks)
               .WithOne(l  => l.DocumentCc)
               .HasForeignKey(l => l.DocumentCcId);

        builder.ToTable("DocumentCCs");
    }
}
