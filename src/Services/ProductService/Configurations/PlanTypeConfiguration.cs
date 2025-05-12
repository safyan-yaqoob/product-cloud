using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Entities;

namespace ProductService.Configurations
{
	public class PlanTypeConfiguration : IEntityTypeConfiguration<Plan>
	{
		public void Configure(EntityTypeBuilder<Plan> builder)
		{
			builder.HasKey(p => p.Id);

			builder.Property(p => p.Name)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(p => p.Description)
				.HasMaxLength(500);

			builder.Property(p => p.MonthlyPrice)
				.HasColumnType("decimal(18,2)")
				.IsRequired();

			builder.Property(p => p.AnnaulPrice)
				.HasColumnType("decimal(18,2)")
				.IsRequired();

			builder.Property(p => p.Currency)
				.IsRequired()
				.HasMaxLength(10);

			builder.Property(p => p.BillingCycle)
				.IsRequired();

			builder.Property(p => p.IsActive)
				.HasDefaultValue(true);

			builder.Property(p => p.CreatedAt)
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.HasOne(e => e.Product)
			  .WithMany()
			  .HasForeignKey(e => e.ProductId)
			  .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
