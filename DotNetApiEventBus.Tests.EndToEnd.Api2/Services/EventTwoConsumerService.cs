using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;

namespace DotNetApiEventBus.Tests.EndToEnd.Api2.Services
{
    public interface IEventTwoConsumerService
    {
        List<EventTwo> Get();
        void Create(EventTwo @event);
        EventTwo? Get(Guid id);
        void Delete();
        void Delete(Guid id);

    }
    public class EventTwoConsumerService : IEventTwoConsumerService
    {
        public const string OutputDirectory = "Output";
        private static object _lockObject = new object();
        private readonly ILogger<IEventTwoConsumerService> _logger;
        private readonly FileRepository<EventTwo> _fileRepository;
        public EventTwoConsumerService(ILogger<IEventTwoConsumerService> logger) : base()
        {
            _logger = logger;
            _fileRepository = new FileRepository<EventTwo>(OutputPath);
        }
        public List<EventTwo> Get()
        {
            lock (_lockObject)
            {
                return _fileRepository.Get();
            }
        }
        public void Create(EventTwo @event)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Creating eventTwo {@event}", args: [@event]))
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
        public EventTwo? Get(Guid id)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Getting eventTwo with {id}", args: [id]))
                {
                    var eventTwo = Get().Where(e => e.Id == id).FirstOrDefault();
                    if (eventTwo == null)
                    {
                        _logger.LogInformationCaller("No eventTwo found with {id}", args: [id]);
                    }
                    else
                    {
                        _logger.LogInformationCaller("EventOne found with {id}, {@eventTwo}", args: [id, eventTwo]);
                    }
                    return eventTwo;
                }
            }
        }
        public string OutputPath => Path.Combine(OutputDirectory, $"{nameof(EventTwoConsumerService)}.json");
        public void Delete()
        {
            lock (_lockObject)
            {
                _logger.LogInformationCaller("Deleting all eventTwos");
                _fileRepository.Save(new List<EventTwo>());
                _logger.LogInformationCaller("Deleted all eventTwos");
            }
        }
        public void Delete(Guid id)
        {
            lock (_lockObject)
            {
                using (_logger.BeginScope("Deleting eventTwo with {id}", args: [id]))
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
