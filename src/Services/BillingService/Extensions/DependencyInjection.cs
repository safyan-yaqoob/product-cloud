using BillingService.Abstraction;
using BillingService.Database;
using BillingService.Middleware;
using ProductCloud.SharedKernal.Infrastructure;
using ProductCloud.SharedKernal.CQRS;

namespace BillingService.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddSharedInfrastructure(configuration);
			services.AddMessageBroker<BillingDbContext>(configuration, AppDomain.CurrentDomain.GetAssemblies());

			services.Scan(selector =>
			{
				selector.FromAssemblies(typeof(DependencyInjection).Assembly)
				  .AddClasses(filter => filter.AssignableTo(typeof(ICommandHandler<,>)))
				  .AsImplementedInterfaces()
				  .WithScopedLifetime();
			});

			services.AddScoped<ExceptionHandlingMiddleware>();
			services.AddScoped<IStripeBillingService, StripeBillingService>();
            return services;
		}
	}
}
