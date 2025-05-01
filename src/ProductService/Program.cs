using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using ProductService.Database;
using ProductService.Extensions;

internal class Program
{
  private static void Main(string[] args)
  {
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

    app.Run();
  }
}
