using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProductService.Database.Entities;
using System.Reflection;

namespace ProductService.Database
{
  public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
  {
    public DbSet<Product> Products { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Feature> Features { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      // If we will use PostgreSql db then below in the name convention code
      // how it create the db table name and table properties name.
      foreach (var entity in builder.Model.GetEntityTypes())
      {
        // Replace table names
        entity.SetTableName(entity.GetTableName()?.Underscore());

        var ecommerceObjectIdentifier = StoreObjectIdentifier.Table(entity.GetTableName()?.Underscore()!, entity.GetSchema());

        // Replace column names
        foreach (var property in entity.GetProperties())
        {
          property.SetColumnName(property.GetColumnName(ecommerceObjectIdentifier)?.Underscore());
        }

        foreach (var key in entity.GetKeys())
        {
          key.SetName(key.GetName()?.Underscore());
        }

        foreach (var key in entity.GetForeignKeys())
        {
          key.SetConstraintName(key.GetConstraintName()?.Underscore());
        }
      }
    }
  }
}
