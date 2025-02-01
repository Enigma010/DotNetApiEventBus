using MassTransit;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace DotNetApiEventBus
{

    /// <summary>
    /// Event consumer definition, by default all applications will have all events emitted to be listened to.
    /// </summary>
    /// <typeparam name="EventType">The event type</typeparam>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class EventConsumerDefinition<EventType> : ConsumerDefinition<EventType> 
        where EventType : class, IConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public EventConsumerDefinition(IConfiguration configuration) 
        {
            EventBusConfig = new EventBusConfig(configuration);
        }
        /// <summary>
        /// The event bus configuration
        /// </summary>
        public EventBusConfig EventBusConfig { get; set; }
    }
}
