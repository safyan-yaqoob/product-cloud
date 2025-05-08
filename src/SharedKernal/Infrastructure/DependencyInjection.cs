using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal.Caching;

namespace SharedKernal.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = configuration.GetConnectionString("Redis");
			});
			services.AddScoped<IAppCache, RedisAppCache>();

            return services;
		}
	}
}
