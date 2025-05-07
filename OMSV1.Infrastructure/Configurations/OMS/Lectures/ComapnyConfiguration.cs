using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Companies;

namespace OMSV1.Infrastructure.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(ddt => ddt.Id); // Assuming `Entity` has a primary key `Id`.

            builder.Property(ddt => ddt.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Table Mapping
            builder.ToTable("Company");
        }
    }
}
