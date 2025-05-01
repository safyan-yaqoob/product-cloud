using IdentityServer.Data;
using IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.AddNpgsqlDbContext<AuthDbContext>("authDb", null, options =>
{
  options.UseNpgsql(
      builder.Configuration.GetConnectionString("authDb"),
      npgsql => npgsql.MigrationsAssembly(typeof(AuthDbContext).Assembly.GetName().Name)
  );

  options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
         .EnableDetailedErrors();

  options.UseOpenIddict();
});

builder.Services.RegisterServices(builder.Configuration);
 
var app = builder.Build();

app.ConfigurePipeline();
app.Run();
