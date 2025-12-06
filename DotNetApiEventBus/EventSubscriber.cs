using DotNetApiLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetApiEventBus
{
    public interface IEventConsumer<EventType> where EventType : class
    {
        Task Respond(EventType @event);
    }

    /// <summary>
    /// An abstract class for event consumers
    /// </summary>
    /// <typeparam name="EventType">The event type or contract for the event</typeparam>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public abstract class EventSubscriber<EventType> :
        IHandleMessages<EventType>, 
        IEventConsumer<EventType> where EventType : class
    {
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILogger _logger;
        /// <summary>
        /// Createa new event consumer
        /// </summary>
        /// <param name="logger"></param>
        public EventSubscriber(ILogger<EventSubscriber<EventType>> logger, 
            IConfiguration configuration)
        {
            _logger = logger;
        }
        /// <summary>
        /// The mass transit entry point for consuming events
        /// </summary>
        /// <param name="context">The consumer context</param>
        /// <returns></returns>
        public async Task Handle(EventType @event)
        {
            _logger.LogInformationCaller("Processing event {@event}",
                args: [@event]);
            await Respond(@event);
            _logger.LogInformationCaller("Processed event {@event}",
                args: [@event]);
        }
        /// <summary>
        /// The entry point for consuming events, implement this and
        /// add application logic to handle the event
        /// </summary>
        /// <param name="event">The event</param>
        /// <returns></returns>
        public abstract Task Respond(EventType @event);
    }
}
