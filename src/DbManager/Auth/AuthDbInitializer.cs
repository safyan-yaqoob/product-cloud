using BillingService.Database;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using product_cloud.DbManager.Billing;
using System.Diagnostics;

namespace product_cloud.DbManager.Auth
{
  public class AuthDbInitializer(IServiceProvider serviceProvider, ILogger<BillingDbInitializer> logger) : BackgroundService
  {
    public const string ActivitySourceName = "Migrations";

    private readonly ActivitySource _activitySource = new(ActivitySourceName);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      using var scope = serviceProvider.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

      using var activity = _activitySource.StartActivity("Initializing catalog database", ActivityKind.Client);
      await InitializeDatabaseAsync(dbContext, stoppingToken);
    }

    public async Task InitializeDatabaseAsync(AuthDbContext dbContext, CancellationToken cancellationToken = default)
    {
      var sw = Stopwatch.StartNew();

      var strategy = dbContext.Database.CreateExecutionStrategy();
      await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

      logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
    }
  }
}
