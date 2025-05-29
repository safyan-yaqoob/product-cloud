using BillingService.Database;
using BillingService.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<BillingDbContext>("billingDb", null, options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("billingDb"),
        npgsql => npgsql.MigrationsAssembly(typeof(BillingDbContext).Assembly.GetName().Name)
    );

    options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
           .EnableDetailedErrors();
});

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.ConfigurePipeline(builder.Configuration);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BillingDbContext>();
    db.Database.Migrate();
}

app.Run();
