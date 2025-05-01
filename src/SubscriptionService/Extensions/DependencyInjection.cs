using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using SubscriptionService.Database;
using SubscriptionService.Middleware;

namespace SubscriptionService.Extensions
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();

      services.AddDbContext<SubscriptionDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

      services.Scan(selector =>
      {
        selector.FromAssemblies(typeof(DependencyInjection).Assembly)
        .AddClasses(filter => filter.AssignableTo(typeof(ICommandHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime();
      });

      services.AddScoped<ExceptionHandlingMiddleware>();

      return services;
    }
  }
}
