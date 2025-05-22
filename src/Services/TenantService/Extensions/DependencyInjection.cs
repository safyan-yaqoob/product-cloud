using System.Reflection;
using System.Threading.RateLimiting;
using ProductCloud.SharedKernal.Infrastructure;
using ProductCloud.SharedKernal.CQRS;
using TenantService.Database;
using TenantService.Middleware;

namespace TenantService.Extensions
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();

			services.AddSharedInfrastructure(configuration);
			services.AddMessageBroker<TenantDbContext>(configuration, AppDomain.CurrentDomain.GetAssemblies());

			services.AddCors(options =>
			{
				options.AddPolicy("customPolicy", builder =>
				{
					builder.AllowAnyOrigin()
						 .AllowAnyHeader()
						 .AllowAnyMethod();
				});
			});

			services.AddRateLimiter(options =>
			{
				options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
				{
					return RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: context.Request.Headers["X-Client-Id"].ToString() ?? 
						              context.Connection.RemoteIpAddress?.ToString(),
						factory: _ => new FixedWindowRateLimiterOptions
						{
							PermitLimit = 100,
							Window = TimeSpan.FromMinutes(1),
							QueueProcessingOrder = QueueProcessingOrder.OldestFirst
						});
				});

				options.AddPolicy("TenantApi", context =>
				{
					return RateLimitPartition.GetTokenBucketLimiter(
						partitionKey: context.User.Identity?.Name ?? "anonymous",
						factory: _ => new TokenBucketRateLimiterOptions
						{
							TokenLimit = 10,
							TokensPerPeriod = 10,
							ReplenishmentPeriod = TimeSpan.FromSeconds(15),
							AutoReplenishment = true
						});
				});
			});
			
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

        public static IServiceCollection AddDataSeeder(this IServiceCollection services)
        {
            services.AddScoped<TenantDataSeeder>();
            return services;
        }
    }
}
