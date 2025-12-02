using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Persistence.FileSystem;
using Rebus.RabbitMq;
using Rebus.Routing.TypeBased;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DotNetApiEventBus.Di
{

    /// <summary>
    /// Dependency injection for the event bus
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public static class DependencyInjector
    {
        /// <summary>
        /// The delimiter used to separate the queue name parts
        /// </summary>
        public const string QueueNameDelimiter = ":";
        /// <summary>
        /// Registers dependencies for the application
        /// </summary>
        /// <param name="builder">The application host builder</param>
        public static void AddEventBusDependencies(
            this IHostApplicationBuilder builder, 
            IEnumerable<string> subscriberAssemblyNames)
        {
            EventBusConfig eventBusConfig = new EventBusConfig(builder.Configuration);
            List<Assembly> subscriberAssemblies = new List<Assembly>();
            List<Assembly> publisherAssemblies = new List<Assembly>();
            subscriberAssemblyNames.ToList().ForEach(subscriberAssemblyName =>
            {
                subscriberAssemblies.Add(Assembly.Load(subscriberAssemblyName));
            });

            builder.Services.AddRebus(
                (configure, provider) => {
                    return configure.Transport(t =>
                    t.UseRabbitMq(eventBusConfig.ConnectionString, eventBusConfig.QueueName));
                },
                onCreated: async (bus) =>
                {
                    foreach (Assembly subscriberAssembly in subscriberAssemblies)
                    {
                        foreach ((Type subscriberType, Type subscriberEventType) in subscriberAssembly.GetSubscriberAndEventTypes())
                        {
                            await bus.Subscribe(subscriberEventType);
                        }
                    }
                });
            foreach (Assembly subscriberAssembly in subscriberAssemblies)
            {
                foreach ((Type subscriberType, Type subscriberEventType) in subscriberAssembly.GetSubscriberAndEventTypes())
                {
                    builder.Services.AddRebusHandler(subscriberType);
                }
            }
            
            /*
            builder.Services.AddRebus(
                configure => configure
                .Transport(t => t.UseRabbitMq(eventBusConfig.ConnectionString, eventBusConfig.QueueName))
                .Routing(r =>
                {
                    foreach (Assembly subscriberAssembly in subscriberAssemblies)
                    {
                        foreach (Type subscriberEventType in subscriberAssembly.GetSubscriberEventTypes())
                        {
                            r.TypeBased().MapAssemblyOf(subscriberEventType, "EventBus.TestsEndToEnd");
                        }
                    }
                }));
            */
            builder.Services.AddScoped<IEventPublisher, EventPublisher>();
        }
    }
}
