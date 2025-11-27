using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;

namespace DotNetApiEventBus.Tests.EndToEnd.Api.Services
{

    public interface IEventOneConsumerService
    {
        public List<EventOne> Get();
        public EventOne? Get(Guid id);
        public string OutputPath { get; }
        public void Delete();
        public void Delete(Guid id);
        void Create(EventOne @event);
        void Handle(EventOne @event);
    }
    public class EventOneConsumerService : IEventOneConsumerService
    {
        public const string OutputDirectory = "Output";
        private static object _lockObject = new object();
        private readonly ILogger<IEventOneService> _logger;
        private readonly FileRepository<EventOne> _fileRepository;
        public EventOneConsumerService(ILogger<IEventOneService> logger) : base()
        {
            _logger = logger;
            _fileRepository = new FileRepository<EventOne>(OutputPath);
        }
        public void Handle(EventOne @event)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Handling eventOne {@event}", args: [@event]))
                {
                    EventOne existingEvent = Get(@event.Id) ?? @event;       
                    existingEvent.AttemptNumber += 1;
                    _logger.LogInformationCaller("Attempt number {attemptNumber}", args: [existingEvent.AttemptNumber]);
                    _logger.LogInformationCaller("Deleting event");
                    Delete(existingEvent.Id);
                    _logger.LogInformationCaller("Creating event");
                    Create(existingEvent);
                    if (existingEvent.ThrowDuringProcessing &&
                        existingEvent.AttemptNumber != existingEvent.SucceedOnAttemptNumber)
                    {
                        _logger.LogInformationCaller("Throwing exception");
                        throw new InvalidOperationException();
                    }
                }
            }
        }
        public List<EventOne> Get()
        {
            lock (_lockObject)
            {
                return _fileRepository.Get();
            }
        }
        public void Create(EventOne @event)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Creating eventOne {@event}", args: [@event]))
                {
                    _logger.LogInformationCaller("Getting events");
                    var events = Get();
                    _logger.LogInformationCaller("Adding event");
                    events.Add(@event);
                    _logger.LogInformationCaller("Saving events");
                    _fileRepository.Save(events);
                    _logger.LogInformationCaller("Saved events");
                }
            }
        }
        public EventOne? Get(Guid id)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Getting eventOne with {id}", args: [id]))
                {
                    var eventOne = Get().Where(e => e.Id == id).FirstOrDefault();
                    if (eventOne == null)
                    {
                        _logger.LogInformationCaller("No eventOne found with {id}", args: [id]);
                    }
                    else
                    {
                        _logger.LogInformationCaller("EventOne found with {id}, {@eventOne}", args: [id, eventOne]);
                    }
                    return eventOne;
                }
            }
        }
        public string OutputPath => Path.Combine(OutputDirectory, $"{nameof(EventOneConsumerService)}.json");
        public void Delete()
        {
            lock (_lockObject)
            {
                _logger.LogInformationCaller("Deleting all eventOnes");
                _fileRepository.Save(new List<EventOne>());
                _logger.LogInformationCaller("Deleted all eventOnes");
            }
        }
        public void Delete(Guid id)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Deleting eventOne with {id}", args: [id]))
                {
                    _logger.LogInformationCaller("Getting all events");
                    var events = Get();
                    _logger.LogInformationCaller("Removing events");
                    events.RemoveAll(e => e.Id == id);
                    _logger.LogInformationCaller("Saving events");
                    _fileRepository.Save(events);
                    _logger.LogInformationCaller("Saved events");
                }
            }
        }
    }
}
