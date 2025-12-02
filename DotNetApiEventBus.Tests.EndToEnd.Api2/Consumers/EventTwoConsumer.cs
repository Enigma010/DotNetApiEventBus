using DotNetApiEventBus.Tests.EndToEnd.Api2.Services;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;


namespace DotNetApiEventBus.Tests.EndToEnd.Api2.Consumers
{
    public class EventTwoConsumer : EventSubscriber<EventTwo>
    {
        private readonly IEventTwoService _service;
        private readonly IEventTwoConsumerService _serviceConsumer;
        public EventTwoConsumer(ILogger<EventSubscriber<EventTwo>> logger,
            IConfiguration configuration,
            IEventTwoService service,
            IEventTwoConsumerService serviceConsumer) : base(logger, configuration)
        {
            _service = service;
            _serviceConsumer = serviceConsumer;
        }
        public override Task Respond(EventTwo @event)
        {
            _logger.LogInformationCaller("Creating eventTwo {@event}", args: [@event]);
            _service.Create(@event);
            _logger.LogInformationCaller("Created eventTwo {@event}", args: [@event]);
            return Task.CompletedTask;
        }
    }
}
