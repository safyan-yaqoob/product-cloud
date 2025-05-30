using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ServiceDefaults;
using TenantService;
using TenantService.Database;
using TenantService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<TenantDbContext>("tenantDb", null, options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("tenantDb"),
        npgsql => npgsql.MigrationsAssembly(typeof(TenantDbContext).Assembly.GetName().Name)
    );

    options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
           .EnableDetailedErrors();
});

builder.Services.AddServices(builder.Configuration);
builder.Services.AddDataSeeder();

var app = builder.Build();
app.ConfigurePipeline(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
        db.Database.Migrate();

        var seeder = scope.ServiceProvider.GetRequiredService<TenantDataSeeder>();
        await seeder.SeedAsync();
    }
}

app.Run();
