using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SubscriptionService.Database
{
    public class SubscriptionDbContextDesignFactory : IDesignTimeDbContextFactory<SubscriptionDbContext>
    {
        public SubscriptionDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "test";

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory ?? "")
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var connectionStringSectionValue = configuration.GetConnectionString("subscriptionDb");

            if (string.IsNullOrWhiteSpace(connectionStringSectionValue))
            {
                throw new InvalidOperationException($"Could not find a value for Default Connection section.");
            }

            Console.WriteLine($"ConnectionString  section value is : {connectionStringSectionValue}");

            var optionsBuilder = new DbContextOptionsBuilder<SubscriptionDbContext>()
                .UseNpgsql(
                    connectionStringSectionValue,
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(GetType().Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                    }
                )
                .UseSnakeCaseNamingConvention();

            return (SubscriptionDbContext)Activator.CreateInstance(typeof(SubscriptionDbContext), optionsBuilder.Options)!;
        }
    }
}
