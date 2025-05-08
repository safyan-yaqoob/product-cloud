namespace SharedKernal.Messaging.Abstractions
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
        Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class;
    }
}
