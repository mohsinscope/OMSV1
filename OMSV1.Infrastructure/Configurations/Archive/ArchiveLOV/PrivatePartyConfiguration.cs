// Infrastructure/Configurations/PrivatePartyConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Documents;

namespace OMSV1.Infrastructure.Configurations
{
    public class PrivatePartyConfiguration : IEntityTypeConfiguration<PrivateParty>
    {
        public void Configure(EntityTypeBuilder<PrivateParty> builder)
        {
            builder.ToTable("PrivateParties");

            builder.HasKey(pp => pp.Id);

            builder.Property(pp => pp.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // One-to-many: PrivateParty -> Documents
            builder.HasMany(pp => pp.Documents)
                   .WithOne(d => d.PrivateParty)
                   .HasForeignKey(d => d.PrivatePartyId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
