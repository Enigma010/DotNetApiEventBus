using DotNetApiEventBus.Tests.EndToEnd.Api.Repositories;
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
        private static object _lockObject = new object();
        private readonly ILogger<IEventTwoService> _logger;
        private readonly IEventTwoService _eventTwoService;
        private readonly EventTwoRepository _fileRepository;
        public EventTwoSubscriberService(IEventTwoService eventTwoService,
            ILogger<IEventTwoService> logger) : base()
        {
            _eventTwoService = eventTwoService;
            _logger = logger;
            _fileRepository = new EventTwoRepository(_logger, nameof(EventTwoRepository));
        }

        public void Respond(EventTwo @event)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Handling eventOne {@event}", args: [@event]))
                {
                    EventTwo existingEvent = _fileRepository.Get(@event.Id) ?? @event;
                    existingEvent.AttemptNumber += 1;
                    _logger.LogInformationCaller("Attempt number {attemptNumber}", args: [existingEvent.AttemptNumber]);
                    _logger.LogInformationCaller("Deleting event");
                    _fileRepository.Delete(existingEvent.Id);
                    _logger.LogInformationCaller("Creating event");
                    _fileRepository.Create(existingEvent);
                    if (existingEvent.ThrowDuringProcessing &&
                        existingEvent.AttemptNumber != existingEvent.SucceedOnAttemptNumber)
                    {
                        _logger.LogInformationCaller("Throwing exception");
                        throw new InvalidOperationException();
                    }
                    _eventTwoService.Create(@event);
                }
            }
        }
    }
}