using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductService;
using ProductService.Database;
using ProductService.Extensions;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ProductDbContext>("productDb", null, options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("productDb"),
        npgsql => npgsql.MigrationsAssembly(typeof(ProductDbContext).Assembly.GetName().Name)
    );

    options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
           .EnableDetailedErrors();
});

builder.Services.AddServices(builder.Configuration);
builder.Services.AddDataSeeder();

var app = builder.Build();

app.ConfigurePipeline(builder.Configuration);


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    db.Database.Migrate(); 
    var seeder = scope.ServiceProvider.GetRequiredService<ProductDataSeeder>();
    await seeder.SeedAsync();
}

app.Run();