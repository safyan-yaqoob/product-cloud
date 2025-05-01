using BillingService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingService.Configurations
{
  public sealed class BillingTypeConfiguration : IEntityTypeConfiguration<Billing>
  {
    public void Configure(EntityTypeBuilder<Billing> entity)
    {
      entity.HasKey(e => e.Id);

      entity.Property(e => e.Id)
          .ValueGeneratedNever();

      entity.Property(e => e.TenantId)
          .IsRequired();

      entity.Property(e => e.SubscriptionId)
          .IsRequired();

      entity.Property(e => e.PaymentGatewayTransactionId)
          .IsRequired();

      entity.Property(e => e.Amount).IsRequired();
      entity.Property(e => e.Currency).IsRequired();
      entity.Property(e => e.Status).IsRequired();

      entity.Property(e => e.CreatedAt)
          .HasDefaultValueSql("CURRENT_TIMESTAMP");

      entity.Property(e => e.PaidAt);

      entity.Property(e => e.FailureReason);
    }
  }
}
