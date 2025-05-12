using BillingService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingService.Configurations
{
    public class PaymentMethodTypeConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.TenantId)
                .IsRequired();

            entity.Property(e => e.PaymentGatewayCustomerId)
                .IsRequired();

            entity.Property(e => e.CardType)
                .IsRequired();

            entity.Property(e => e.IsDefault).IsRequired();
            entity.Property(e => e.LastFourDigits).IsRequired();
            entity.Property(e => e.BillingAddress).IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.ExpiryDate).IsRequired();
        }
    }
}
