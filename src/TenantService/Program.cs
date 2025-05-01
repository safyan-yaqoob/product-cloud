using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
builder.Services.AddCors(options =>
{
  options.AddPolicy("customPolicy", builder =>
  {
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
  });
});

var app = builder.Build();

app.ConfigurePipeline(builder.Configuration);

app.UseCors();

app.Run();
