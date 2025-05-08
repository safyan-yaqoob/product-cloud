using SubscriptionService.Middleware;
using SharedKernal.Infrastructure;
using SharedKernal.CQRS;

namespace SubscriptionService.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddSharedInfrastructure(configuration);

			services.Scan(selector =>
			{
				selector.FromAssemblies(typeof(DependencyInjection).Assembly)
				  .AddClasses(filter => filter.AssignableTo(typeof(ICommandHandler<,>)))
				  .AsImplementedInterfaces()
				  .WithScopedLifetime();
			});

			services.AddScoped<ExceptionHandlingMiddleware>();
            services.AddGrpc();

            return services;
		}
	}
}
