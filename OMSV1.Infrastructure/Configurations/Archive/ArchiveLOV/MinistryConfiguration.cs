using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Ministries;
using OMSV1.Domain.Entities.GeneralDirectorates;

namespace OMSV1.Infrastructure.Configurations
{
    public class MinistryConfiguration : IEntityTypeConfiguration<Ministry>
    {
        public void Configure(EntityTypeBuilder<Ministry> builder)
        {
            builder.ToTable("Ministries");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasMany(m => m.GeneralDirectorates)
                   .WithOne(gd => gd.Ministry)
                   .HasForeignKey(gd => gd.MinistryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
