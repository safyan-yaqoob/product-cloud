using Shared.CQRS;
using TenantService.Middleware;

namespace TenantService.Extensions
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

      return services;
    }
  }
}
