using DotNetApiEventBus.Tests.EndToEnd.Api.Repositories;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;

namespace DotNetApiEventBus.Tests.EndToEnd.Api.Services
{
    public interface IEventOneSubscriberService
    {
        void Respond(EventOne @event);
    }
    public class EventOneSubscriberService : IEventOneSubscriberService
    {
        private static object _lockObject = new object();
        private readonly ILogger<IEventOneService> _logger;
        private readonly IEventOneService _eventOneService;
        private readonly EventOneRepository _fileRepository;
        public EventOneSubscriberService(IEventOneService eventOneService,
            ILogger<IEventOneService> logger) : base()
        {
            _logger = logger;
            _fileRepository = new EventOneRepository(_logger, nameof(EventOneSubscriberService));
            _eventOneService = eventOneService;
        }
        public void Respond(EventOne @event)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Handling eventOne {@event}", args: [@event]))
                {
                    EventOne existingEvent = _fileRepository.Get(@event.Id) ?? @event;       
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
                    _eventOneService.Create(@event);
                }
            }
        }
    }
}
