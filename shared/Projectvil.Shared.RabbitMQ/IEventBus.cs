namespace Projectvil.Shared.RabbitMQ;

public interface IEventBus
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : class;
}