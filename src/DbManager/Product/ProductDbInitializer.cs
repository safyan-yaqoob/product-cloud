using Microsoft.EntityFrameworkCore;
using ProductService.Database;
using System.Diagnostics;

namespace product_cloud.DbManager.Product
{
  public class ProductDbInitializer(IServiceProvider serviceProvider, ILogger<ProductDbInitializer> logger) : BackgroundService
  {
    public const string ActivitySourceName = "Migrations";

    private readonly ActivitySource _activitySource = new(ActivitySourceName);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      using var scope = serviceProvider.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

      using var activity = _activitySource.StartActivity("Initializing catalog database", ActivityKind.Client);
      await InitializeDatabaseAsync(dbContext, stoppingToken);
    }

    public async Task InitializeDatabaseAsync(ProductDbContext dbContext, CancellationToken cancellationToken = default)
    {
      var sw = Stopwatch.StartNew();

      var strategy = dbContext.Database.CreateExecutionStrategy();
      await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

      //await SeedAsync(dbContext, cancellationToken);

      logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
    }
  }
}
