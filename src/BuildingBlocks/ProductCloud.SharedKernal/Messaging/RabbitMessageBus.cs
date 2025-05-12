using MassTransit;
using ProductCloud.SharedKernal.Messaging.Abstractions;

namespace ProductCloud.SharedKernal.Messaging
{
    public sealed class RabbitMessageBus(BrokerOptions config, IServiceProvider serviceProvider) : IMessageBus, IDisposable
    {
        private readonly IBus _bus;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            await _bus.Publish(message, cancellationToken);
        }

        public Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>(Func<IServiceProvider, IMessageHandler<T>> factory) where T : class
        {
            throw new NotImplementedException();
        }
    }
}