using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMSV1.Domain.Entities.Directorates;

namespace OMSV1.Infrastructure.Configurations
{
    public class DirectorateConfiguration : IEntityTypeConfiguration<Directorate>
    {
        public void Configure(EntityTypeBuilder<Directorate> builder)
        {
            builder.ToTable("Directorates");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(d => d.GeneralDirectorate)
                   .WithMany(gd => gd.Directorates)
                   .HasForeignKey(d => d.GeneralDirectorateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Departments)
                   .WithOne(dep => dep.Directorate)
                   .HasForeignKey(dep => dep.DirectorateId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
