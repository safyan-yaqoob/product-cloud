using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernal.Messaging.Abstractions;

namespace SharedKernal.Messaging
{
    public class MessageBusBackgroundService(IMessageBus bus, 
        IServiceProvider serviceProvider, 
        ILogger<MessageBusBackgroundService> logger) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceProvider.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<object>()
                .Where(e => e.GetType().GetInterfaces().Any(e => e.IsGenericType && e.GetGenericTypeDefinition() == typeof(IMessageHandler<>)));

            foreach (var handler in handlers)
            {
                var messageType = handler.GetType().GetInterfaces()
                    .First(i => i.IsGenericType &&
                               i.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
                    .GetGenericArguments()[0];


            }

            return Task.CompletedTask;
        }
    }
}
