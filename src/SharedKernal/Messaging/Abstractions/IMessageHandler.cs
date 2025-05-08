namespace SharedKernal.Messaging.Abstractions
{
    public interface IMessageHandler<in T> where T : class
    {
        Task HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
