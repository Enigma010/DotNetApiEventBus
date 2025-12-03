using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;

namespace DotNetApiEventBus.Tests.EndToEnd.Api2.Services
{
    public interface IEventTwoSubscriberService
    {
        void Respond(EventTwo @event);

    }
    public class EventTwoSubscriberService : IEventTwoSubscriberService
    {
        public const string OutputDirectory = "Output";
        private static object _lockObject = new object();
        private readonly ILogger<IEventTwoSubscriberService> _logger;
        private readonly IEventTwoService _eventTwoService;
        public EventTwoSubscriberService(IEventTwoService eventTwoService,
            ILogger<IEventTwoSubscriberService> logger) : base()
        {
            _eventTwoService = eventTwoService;
            _logger = logger;
        }


        public void Respond(EventTwo @event)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Handling eventOne {@event}", args: [@event]))
                {
                    EventTwo existingEvent = _eventTwoService.Get(@event.Id) ?? @event;
                    existingEvent.AttemptNumber += 1;
                    _logger.LogInformationCaller("Attempt number {attemptNumber}", args: [existingEvent.AttemptNumber]);
                    _logger.LogInformationCaller("Deleting event");
                    _eventTwoService.Delete(existingEvent.Id);
                    _logger.LogInformationCaller("Creating event");
                    _eventTwoService.Create(existingEvent);
                    if (existingEvent.ThrowDuringProcessing &&
                        existingEvent.AttemptNumber != existingEvent.SucceedOnAttemptNumber)
                    {
                        _logger.LogInformationCaller("Throwing exception");
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}