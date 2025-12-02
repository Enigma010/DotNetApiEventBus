using DotNetApiLogging;
using DotNetApiUnitOfWork;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.TransactionScopes;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

namespace DotNetApiEventBus
{
    public interface IEventPublisher : IUnitOfWork
    {
        Task Publish(IEnumerable<object> events);
    }

    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class EventPublisher : IEventPublisher, IDisposable
    {
        /// <summary>
        /// The event busx
        /// </summary>
        private readonly IBus _bus;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<IEventPublisher> _logger;
        private TransactionScope? _transactionScope;
        private bool disposedValue;

        /// <summary>
        /// Creates a new event publisher
        /// </summary>
        /// <param name="bus">The event bus</param>
        /// <param name="logger">The logger</param>
        public EventPublisher(IBus bus, ILogger<IEventPublisher> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public Task Begin()
        {
            if (_transactionScope == null)
            {
                _transactionScope = new TransactionScope();
                _transactionScope.EnlistRebus();
            }
            return Task.CompletedTask;    
        }

        public Task Commit()
        {
            if(_transactionScope != null)
            {
                _transactionScope.Complete();
            }
            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            if (_transactionScope != null)
            {
                _transactionScope.Dispose();
                _transactionScope = null;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Publishes events to the event bus
        /// </summary>
        /// <param name="events">The events</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if the events are null</exception>
        public async Task Publish(IEnumerable<object> events)
        {
            _logger.LogInformationCaller("Start publishing events");
            if (events == null)
            {
                throw new ArgumentNullException("The event cannot be null");
            }
            foreach (var @event in events)
            {
                _logger.LogInformationCaller("Publishing event {@event}", args: [@event]);
                await _bus.Publish(@event);
            }
            _logger.LogInformationCaller("Finished publishing events");
        }

        /// <summary>
        /// Disapose the object
        /// </summary>
        /// <param name="disposing">Whether the object is disposing or not</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if(_transactionScope != null)
                    {
                        _transactionScope.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~EventPublisher()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        /// <summary>
        /// Dispose the current object
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
