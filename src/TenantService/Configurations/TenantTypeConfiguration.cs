using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TenantService.Entities;

namespace TenantService.Configurations
{
	public sealed class TenantTypeConfiguration : IEntityTypeConfiguration<Tenant>
	{
		public void Configure(EntityTypeBuilder<Tenant> entity)
		{
			entity.HasKey(e => e.Id);

			entity.Property(e => e.Id)
				.ValueGeneratedNever();

			entity.Property(e => e.Name)
				.IsRequired()
				.HasMaxLength(200);

			entity.Property(e => e.Orgnization)
				.IsRequired()
				.HasMaxLength(100);

			entity.Property(e => e.ContactEmail)
				.IsRequired()
				.HasMaxLength(150);

			entity.Property(e => e.PlanType)
				.IsRequired();

			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			entity.Property(e => e.Industry);
			entity.Property(e => e.Logo);
			entity.Property(e => e.TimeZone);
		}
	}
}
