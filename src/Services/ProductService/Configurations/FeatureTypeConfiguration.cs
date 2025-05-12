using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Entities;

namespace ProductService.Configurations
{
	public sealed class FeatureTypeConfiguration : IEntityTypeConfiguration<Feature>
	{
		public void Configure(EntityTypeBuilder<Feature> builder)
		{
			builder.ToTable("features");
			builder.HasKey(f => f.Id);

			builder.Property(f => f.Name)
				.IsRequired()
				.HasMaxLength(100);
			builder.Property(f => f.Description)
				.IsRequired()
				.HasMaxLength(500);
			builder.Property(f => f.PlanId)
				.IsRequired();
			builder.HasOne(f => f.Plan)
				.WithMany(p => p.Features)
				.HasForeignKey(f => f.PlanId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
