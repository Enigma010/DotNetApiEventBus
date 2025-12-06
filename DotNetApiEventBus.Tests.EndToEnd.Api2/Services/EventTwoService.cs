
using DotNetApiEventBus.Tests.EndToEnd.Api.Repositories;
using DotNetApiEventBus.Tests.EndToEnd.Events;

namespace DotNetApiEventBus.Tests.EndToEnd.Api2.Services
{
    public interface IEventTwoService
    {
        public List<EventTwo> Get();
        public EventTwo? Get(Guid id);
        public void Delete();
        public void Delete(Guid id);
        void Create(EventTwo @event);
    }

    public class EventTwoService : IEventTwoService
    {
        private readonly ILogger<IEventTwoService> _logger;
        private readonly EventTwoRepository _fileRepository;
        public EventTwoService(ILogger<IEventTwoService> logger) : base()
        {
            _logger = logger;
            _fileRepository = new EventTwoRepository(_logger, nameof(EventTwoService));
        }
        public List<EventTwo> Get()
        {
            return _fileRepository.Get();
        }
        public void Create(EventTwo @event)
        {
            _fileRepository.Create(@event);
        }
        public EventTwo? Get(Guid id)
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
