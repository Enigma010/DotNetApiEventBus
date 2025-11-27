using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotNetApiLogging;

namespace DotNetApiEventBus
{
    public interface IEventConsumer<EventType> where EventType : class
    {
        Task Consume(EventType @event);
    }

    /// <summary>
    /// An abstract class for event consumers
    /// </summary>
    /// <typeparam name="EventType">The event type or contract for the event</typeparam>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public abstract class EventConsumer<EventType> : 
        IConsumer<EventType>, 
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
        public EventConsumer(ILogger<EventConsumer<EventType>> logger, IConfiguration configuration)
        {
            _logger = logger;
            ConsumerDefintion = new EventConsumerDefinition<IConsumer<EventType>>(configuration);
        }
        /// <summary>
        /// The mass transit entry point for consuming events
        /// </summary>
        /// <param name="context">The consumer context</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<EventType> context)
        {
            _logger.LogInformationCaller("Processing from queue {QueueName} message {@Message}",
                args: [ConsumerDefintion.EventBusConfig.QueueName,
                context.Message]);
            await Consume(context.Message);
            _logger.LogInformationCaller("Processed from queue {QueueName} message {@Message}",
                args: [ConsumerDefintion.EventBusConfig.QueueName,
                context.Message]);
        }
        /// <summary>
        /// The entry point for consuming events, implement this and
        /// add application logic to handle the event
        /// </summary>
        /// <param name="event">The event</param>
        /// <returns></returns>
        public abstract Task Consume(EventType @event);
        /// <summary>
        /// The consumer definition
        /// </summary>
        public EventConsumerDefinition<IConsumer<EventType>> ConsumerDefintion
        {
            get;
            private set;
        }
    }
}
