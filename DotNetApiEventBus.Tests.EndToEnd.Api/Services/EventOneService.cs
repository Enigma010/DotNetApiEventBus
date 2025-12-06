using DotNetApiEventBus.Tests.EndToEnd.Api.Repositories;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiLogging;

namespace DotNetApiEventBus.Tests.EndToEnd.Api.Services
{
    public interface IEventOneService
    {
        public List<EventOne> Get();
        public EventOne? Get(Guid id);
        public void Delete();
        public void Delete(Guid id);
        void Create(EventOne @event);
    }

    public class EventOneService : IEventOneService
    {
        private readonly ILogger<IEventOneService> _logger;
        protected readonly EventOneRepository _fileRepository;
        public EventOneService(ILogger<IEventOneService> logger) : base()
        {
            _logger = logger;
            _fileRepository = new EventOneRepository(_logger, nameof(EventOneService));
        }
        public List<EventOne> Get()
        {
            return _fileRepository.Get();
        }
        public void Create(EventOne @event)
        {
            _fileRepository.Create(@event);
        }
        public EventOne? Get(Guid id)
        {
            return _fileRepository.Get(id);
        }
        public void Delete()
        {
            _fileRepository.Delete();
        }
        public void Delete(Guid id)
        {
            _fileRepository.Delete(id);
        }
    }
}
