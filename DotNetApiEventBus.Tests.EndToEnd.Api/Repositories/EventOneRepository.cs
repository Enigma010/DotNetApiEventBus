using DotNetApiEventBus.Tests.EndToEnd.Api.Services;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;

namespace DotNetApiEventBus.Tests.EndToEnd.Api.Repositories
{
    public class EventOneRepository
    {
        public const string OutputDirectory = "Output";
        private static object _lockObject = new object();
        private readonly ILogger<IEventOneService> _logger;
        protected readonly FileRepository<EventOne> _fileRepository;
        public EventOneRepository(ILogger<IEventOneService> logger, string fileName) : base()
        {
            _logger = logger;
            _fileRepository = new FileRepository<EventOne>(Path.Combine(OutputDirectory, $"{fileName}.json"));
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
