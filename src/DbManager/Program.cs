using BillingService.Database;
using IdentityServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using product_cloud.DbManager.Auth;
using product_cloud.DbManager.Billing;
using product_cloud.DbManager.Product;
using product_cloud.DbManager.Subscription;
using product_cloud.DbManager.Tenant;
using ProductService.Database;
using SubscriptionService.Database;
using TenantService.Database;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

//tenant
builder.AddNpgsqlDbContext<TenantDbContext>("tenantDb", null, options =>
{
  options.UseNpgsql(
      builder.Configuration.GetConnectionString("tenantDb"),
      npgsql => npgsql.MigrationsAssembly(typeof(TenantDbContext).Assembly.GetName().Name)
  );

  options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
         .EnableDetailedErrors();
});
builder.Services.AddSingleton<TenantDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<TenantDbInitializer>());
builder.Services.AddHealthChecks()
  .AddCheck<TenantDbHealthCheckInitializer>("TenantDbInitializer", null);

//billing
builder.AddNpgsqlDbContext<BillingDbContext>("billingDb", null, options =>
{
  options.UseNpgsql(
      builder.Configuration.GetConnectionString("billingDb"),
      npgsql => npgsql.MigrationsAssembly(typeof(BillingDbContext).Assembly.GetName().Name)
  );

  options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
         .EnableDetailedErrors();
});
builder.Services.AddSingleton<BillingDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<BillingDbInitializer>());
builder.Services.AddHealthChecks()
  .AddCheck<BillingDbHealthCheckInitializer>("BillingDbInitializer", null);

//product
builder.AddNpgsqlDbContext<ProductDbContext>("productDb", null, options =>
{
  options.UseNpgsql(
      builder.Configuration.GetConnectionString("productDb"),
      npgsql => npgsql.MigrationsAssembly(typeof(ProductDbContext).Assembly.GetName().Name)
  );

  options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
         .EnableDetailedErrors();
});
builder.Services.AddSingleton<ProductDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<ProductDbInitializer>());
builder.Services.AddHealthChecks()
  .AddCheck<ProductDbHealthCheckInitializer>("ProductDbInitializer", null);

//subscription
builder.AddNpgsqlDbContext<SubscriptionDbContext>("subscriptionDb", null, options =>
{
  options.UseNpgsql(
      builder.Configuration.GetConnectionString("subscriptionDb"),
      npgsql => npgsql.MigrationsAssembly(typeof(SubscriptionDbContext).Assembly.GetName().Name)
  );

  options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
         .EnableDetailedErrors();
});
builder.Services.AddSingleton<SubscriptionDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<SubscriptionDbInitializer>());
builder.Services.AddHealthChecks()
  .AddCheck<SubscriptionDbHealthCheckInitializer>("SubscriptionDbInitializer", null);

//auth
builder.AddNpgsqlDbContext<AuthDbContext>("authDb", null, options =>
{
  options.UseNpgsql(
      builder.Configuration.GetConnectionString("authDb"),
      npgsql => npgsql.MigrationsAssembly(typeof(AuthDbContext).Assembly.GetName().Name)
  );

  options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
         .EnableDetailedErrors();
});
builder.Services.AddSingleton<AuthDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<AuthDbInitializer>());
builder.Services.AddHealthChecks()
  .AddCheck<AuthDbHealthCheckInitializer>("AuthDbInitializer", null);

var app = builder.Build();

app.MapHealthChecks("/health");
app.Run();
