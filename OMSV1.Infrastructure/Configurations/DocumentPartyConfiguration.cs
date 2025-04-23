// Infrastructure/Configurations/DocumentPartyConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Documents;

namespace OMSV1.Infrastructure.Configurations
{
    public class DocumentPartyConfiguration : IEntityTypeConfiguration<DocumentParty>
    {
        public void Configure(EntityTypeBuilder<DocumentParty> builder)
        {
            // Primary Key
            builder.HasKey(dp => dp.Id);

            // Name
            builder.Property(dp => dp.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // PartyType enum
            builder.Property(dp => dp.PartyType)
                   .IsRequired();

            // IsOfficial flag
            builder.Property(dp => dp.IsOfficial)
                   .IsRequired()
                   .HasDefaultValue(false);

            // FK to Project
            builder.Property(dp => dp.ProjectId)
                   .IsRequired();
            builder.HasOne(dp => dp.Project)
                   .WithMany(p => p.Parties)
                   .HasForeignKey(dp => dp.ProjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Table Mapping
            builder.ToTable("DocumentParties");
        }
    }
}
