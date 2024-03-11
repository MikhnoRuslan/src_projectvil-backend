using MassTransit;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectvil.Shared.RabbitMQ;

public class EventBus : IEventBus, ITransientDependence
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILanguageHandler _languageHandler;

    public EventBus(IPublishEndpoint publishEndpoint, ILanguageHandler handler)
    {
        _publishEndpoint = publishEndpoint;
        _languageHandler = handler;
    }

    public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
    {
        await _publishEndpoint.Publish(message, h => AddHeaders(h), cancellationToken);
    }
    
    private void AddHeaders(SendContext context)
    {
        var language = _languageHandler.GetLanguageString();
        context.Headers.Set("Accept-Language", language);
    }
}