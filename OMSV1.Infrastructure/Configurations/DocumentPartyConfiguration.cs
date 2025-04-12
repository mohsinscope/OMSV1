using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Documents;

namespace OMSV1.Infrastructure.Configurations
{
    public class DocumentPartyConfiguration : IEntityTypeConfiguration<DocumentParty>
    {
        public void Configure(EntityTypeBuilder<DocumentParty> builder)
        {
            // Primary Key (assumes Entity defines an Id property)
            builder.HasKey(dp => dp.Id);

            // Configure the Name property
            builder.Property(dp => dp.Name)
                .IsRequired()
                .HasMaxLength(200); // Adjust max length as needed

            // Table Mapping
            builder.ToTable("DocumentParties");
        }
    }
}
