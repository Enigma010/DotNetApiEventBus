using DotNetApiEventBus.Tests.EndToEnd.Api.Services;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;


namespace DotNetApiEventBus.Tests.EndToEnd.Api.Subscribers
{
    public class EventOneSubscriber : EventSubscriber<EventOne>
    {
        private readonly IEventOneSubscriberService _subscriber;

        public EventOneSubscriber(ILogger<EventSubscriber<EventOne>> logger,
            IConfiguration configuration,
            IEventOneSubscriberService subscriber): base(logger, configuration)
        {
            _subscriber = subscriber;
        }

        public override Task Respond(EventOne @event)
        {
            _logger.LogInformationCaller("Responding to event {@event}", args: [@event]);
            _subscriber.Respond(@event);
            _logger.LogInformationCaller("Responded to event {@event}", args: [@event]);
            return Task.CompletedTask;
        }
    }
}
