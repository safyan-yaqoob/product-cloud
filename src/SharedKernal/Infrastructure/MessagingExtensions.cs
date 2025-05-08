using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernal.Messaging;
using SharedKernal.Messaging.Abstractions;

namespace SharedKernal.Infrastructure
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly handlerAssembly = null)
        {

            services.Configure<BrokerOptions>(configuration);

            services.AddSingleton<IMessageBus>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BrokerOptions>>().Value;
                return new RabbitMessageBus(options, sp);
            });

            if (handlerAssembly != null)
            {
                services.Scan(cf =>
                {
                    cf.FromAssemblies(handlerAssembly);
                });
                services.RegisterMessageHandlers(handlerAssembly);
            }

            services.AddHostedService<MessageBusBackgroundService>();

            return services;
        }

        private static void RegisterMessageHandlers(this IServiceCollection services, Assembly assembly = null)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(i => i.GetInterfaces()
                    .Any(x => x.IsGenericType && 
                    x.GetGenericTypeDefinition() == typeof(IMessageHandler<>)));


            foreach (var item in handlerTypes)
            {
                var serviceType = item.GetInterfaces().First(e => e.IsGenericType && e.GetGenericTypeDefinition() == typeof(IMessageHandler<>));
                services.AddTransient(serviceType, item);
            }
        }
    }
}
