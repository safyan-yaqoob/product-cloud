using BillingService.Abstraction;
using BillingService.External;
using BillingService.Middleware;
using Shared.CQRS;

namespace BillingService.Extensions
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();

      services.Scan(selector =>
      {
        selector.FromAssemblies(typeof(DependencyInjection).Assembly)
        .AddClasses(filter => filter.AssignableTo(typeof(ICommandHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime();
      });

      services.AddScoped<ExceptionHandlingMiddleware>();

      services.AddHttpClient<TenantServiceClient>(o =>
      {
        o.BaseAddress = new Uri("https://tenant-service");
      });

      services.AddHttpClient<SubscriptionServiceClient>(o =>
      {
        o.BaseAddress = new Uri("https://subscription-service");
      });

      services.AddScoped<IStripeBillingService, StripeBillingService>();

      return services;
    }
  }
}
