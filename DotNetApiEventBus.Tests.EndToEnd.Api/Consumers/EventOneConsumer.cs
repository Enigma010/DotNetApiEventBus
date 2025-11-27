using DotNetApiEventBus.Tests.EndToEnd.Api.Services;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;


namespace DotNetApiEventBus.Tests.EndToEnd.Api.Consumers
{
    public class EventOneConsumer : EventConsumer<EventOne>
    {
        private readonly IEventOneService _service;
        private readonly IEventOneConsumerService _serviceConsumer;

        public EventOneConsumer(ILogger<EventConsumer<EventOne>> logger,
            IConfiguration configuration,
            IEventOneService service,
            IEventOneConsumerService serviceConsumer): base(logger, configuration)
        {
            _service = service;
            _serviceConsumer = serviceConsumer;
        }

        public override Task Consume(EventOne @event)
        {
            _serviceConsumer.Handle(@event);
            _logger.LogInformationCaller("Creating eventOne {@event}", args: [@event]);
            _service.Create(@event);
            _logger.LogInformationCaller("Created eventOne {@event}", args: [@event]);
            return Task.CompletedTask;
        }
    }
}
