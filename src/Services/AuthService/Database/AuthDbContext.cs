using Humanizer;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AuthService.Database
{
    public class AuthDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
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
