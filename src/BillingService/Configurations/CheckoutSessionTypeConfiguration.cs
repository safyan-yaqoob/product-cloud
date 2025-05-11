using BillingService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingService.Configurations
{
    public class CheckoutSessionTypeConfiguration : IEntityTypeConfiguration<CheckoutSession>
    {
        public void Configure(EntityTypeBuilder<CheckoutSession> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
            .ValueGeneratedNever();

            entity.Property(e => e.TenantId)
            .IsRequired();

            entity.Property(e => e.ExpiresAt)
            .IsRequired();

            entity.Property(e => e.Url)
            .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
