using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Entities;

namespace ProductService.Configurations
{
  public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.HasKey(p => p.Id);

      builder.Property(p => p.Name)
          .IsRequired()
          .HasMaxLength(100);

      builder.Property(p => p.Description)
          .HasMaxLength(500);

      builder.Property(p => p.IsActive)
          .HasDefaultValue(true);

      builder.Property(p => p.CreatedAt)
          .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
  }
}
