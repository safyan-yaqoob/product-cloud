namespace ProductCloud.SharedKernal.Messaging.Abstractions
{
    public interface IMessageBus : IMessagePublisher
    {
        void Subscribe<T>(Func<IServiceProvider, IMessageHandler<T>> factory) where T : class;
    }
}
