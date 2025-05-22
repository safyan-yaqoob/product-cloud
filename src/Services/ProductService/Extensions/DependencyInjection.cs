using ProductService.Database;
using ProductService.Middleware;
using ProductCloud.SharedKernal.Infrastructure;
using ProductCloud.SharedKernal.CQRS;

namespace ProductService.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddSharedInfrastructure(configuration);
			services.AddMessageBroker<ProductDbContext>(configuration, AppDomain.CurrentDomain.GetAssemblies());

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

		public static IServiceCollection AddDataSeeder(this IServiceCollection services)
		{
			services.AddScoped<ProductDataSeeder>();
			return services;
		}
	}
}
