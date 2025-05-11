using SubscriptionService.Middleware;
using SharedKernal.Infrastructure;
using SharedKernal.CQRS;
using SubscriptionService.Database;

namespace SubscriptionService.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddSharedInfrastructure(configuration);
			services.AddMessageBroker<SubscriptionDbContext>(configuration, AppDomain.CurrentDomain.GetAssemblies());

			services.Scan(selector =>
			{
				selector.FromAssemblies(typeof(DependencyInjection).Assembly)
				  .AddClasses(filter => filter.AssignableTo(typeof(ICommandHandler<,>)))
				  .AsImplementedInterfaces()
				  .WithScopedLifetime();
			});

			services.AddScoped<ExceptionHandlingMiddleware>();
            services.AddGrpcClient<SharedKernal.Protos.PlanGrpc.PlanGrpcClient>(o =>
            {
                o.Address = new Uri("https://localhost:7182/");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

            return services;
		}
	}
}
