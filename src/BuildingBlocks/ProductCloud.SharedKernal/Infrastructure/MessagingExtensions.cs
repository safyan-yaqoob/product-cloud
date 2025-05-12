using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductCloud.SharedKernal.Messaging.Abstractions;

namespace ProductCloud.SharedKernal.Infrastructure
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessageBroker<TDbContext>(this IServiceCollection services, 
            IConfiguration configuration, Assembly[]? handlerAssemblies = null) where TDbContext : DbContext
        {

            services.Configure<BrokerOptions>(configuration.GetSection("MessageBroker"));

            services.AddMassTransit(mt =>
            {
                if (handlerAssemblies != null)
                {
                    mt.AddConsumers(handlerAssemblies);
                }
                
                //mt.AddEntityFrameworkOutbox<TDbContext>(o =>
                //{
                //    o.UseBusOutbox();
                //    o.UsePostgres();
                //    o.QueryDelay = TimeSpan.FromSeconds(10);
                //});

                mt.UsingRabbitMq((ctx, cfg) =>
                {
                    var options = ctx.GetRequiredService<IOptions<BrokerOptions>>().Value;
                    cfg.Host(new Uri(options.Host));

                    cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(
                        includeNamespace: false,
                        prefix: typeof(TDbContext).Name.Replace("DbContext", "")
                    ));

                    cfg.UseMessageRetry(r => r.Interval(options.RetryCount, options.RetryCount));
                });
            });

            return services;
        }
    }
}
