using BillingService.Abstraction;
using BillingService.Middleware;
using SharedKernal.Infrastructure;
using SharedKernal.CQRS;

namespace BillingService.Extensions
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

			services.AddScoped<IStripeBillingService, StripeBillingService>();

            services.AddGrpcClient<SharedKernal.Protos.SubscriptionGrpc.SubscriptionGrpcClient>(o =>
            {
                o.Address = new Uri("https://localhost:7276/");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

            return services;
		}
	}
}
