using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using SubscriptionService.Database;
using SubscriptionService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<SubscriptionDbContext>("subscriptionDb", null, options =>
{
  options.UseNpgsql(
      builder.Configuration.GetConnectionString("subscriptionDb"),
      npgsql => npgsql.MigrationsAssembly(typeof(SubscriptionDbContext).Assembly.GetName().Name)
  );

  options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
         .EnableDetailedErrors();
});

builder.Services
  .AddServices(builder.Configuration)
  .AddOpenApi();

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

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<SubscriptionDbContext>();
	db.Database.Migrate(); // Apply any pending EF Core migrations
}

app.Run();
