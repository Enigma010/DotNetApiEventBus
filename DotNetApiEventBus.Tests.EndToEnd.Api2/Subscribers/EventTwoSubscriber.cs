using DotNetApiEventBus.Tests.EndToEnd.Api2.Services;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;


namespace DotNetApiEventBus.Tests.EndToEnd.Api2.Subscribers
{
    public class EventTwoSubscriber : EventSubscriber<EventTwo>
    {
        private readonly IEventTwoSubscriberService _subscriber;
        public EventTwoSubscriber(ILogger<EventSubscriber<EventTwo>> logger,
            IConfiguration configuration,
            IEventTwoSubscriberService serviceConsumer) : base(logger, configuration)
        {
            _subscriber = serviceConsumer;
        }
        public override Task Respond(EventTwo @event)
        {
            _logger.LogInformationCaller("Responding to event {@event}", args: [@event]);
            _subscriber.Respond(@event);
            _logger.LogInformationCaller("Responded to event {@event}", args: [@event]);
            return Task.CompletedTask;
        }
    }
}
