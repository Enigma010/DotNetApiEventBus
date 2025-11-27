using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            string queueNamePrefix,
            IEnumerable<string> publisherAssemblyNames,
            IEnumerable<string> consumerAssemblyNames)
        {
            EventBusConfig eventBusConfig = new EventBusConfig(builder.Configuration);
            List<Assembly> consumerAssemblies = new List<Assembly>();
            List<Assembly> publisherAssemblies = new List<Assembly>();
            if (string.IsNullOrEmpty(queueNamePrefix))
            {
                throw new ArgumentNullException("The queue name prefix cannot be null or empty");
            }
            consumerAssemblyNames.ToList().ForEach(consumerAssemblyName =>
            {
                consumerAssemblies.Add(Assembly.Load(consumerAssemblyName));
            });
            publisherAssemblyNames.ToList().ForEach(publisherAssemblyName =>
            {
                publisherAssemblies.Add(Assembly.Load(publisherAssemblyName));
            });
            builder.Services.AddMassTransit(mt =>
            {
                mt.SetKebabCaseEndpointNameFormatter();
                consumerAssemblies.ForEach(entryAssembly =>
                {
                    mt.AddConsumers(entryAssembly);
                    mt.AddActivities(entryAssembly);
                });
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(eventBusConfig.Host, "/", h =>
                    {
                        h.Username(eventBusConfig.Username);
                        h.Password(eventBusConfig.Password);
                    });
                    foreach(Assembly publisherAssembly in publisherAssemblies)
                    {
                        foreach (Type eventType in publisherAssembly.GetAppEventTypes())
                        {
                            MethodInfo configureTopology = typeof(DependencyInjector).GetMethod(nameof(DependencyInjector.ConfigureTopology), BindingFlags.NonPublic | BindingFlags.Static) 
                            ?? throw new InvalidOperationException();
                            configureTopology = configureTopology.MakeGenericMethod([eventType]);
                            configureTopology.Invoke(null, [cfg]);
                        }
                    }
                    foreach(Assembly consumerAssembly in consumerAssemblies)
                    {
                        foreach(Type eventConsumerType in consumerAssembly.GetEventConsumerTypes())
                        {
                            MethodInfo configureReceiveEndPoint = typeof(DependencyInjector).GetMethod(nameof(DependencyInjector.ConfigureReceiveEndPoint), BindingFlags.NonPublic | BindingFlags.Static)
                            ?? throw new InvalidOperationException();
                            configureReceiveEndPoint = configureReceiveEndPoint.MakeGenericMethod([eventConsumerType]);
                            configureReceiveEndPoint.Invoke(null, [cfg, context, $"{queueNamePrefix}:{eventConsumerType.Name}"]);
                        }
                    }
                });
            });
            builder.Services.AddScoped<IEventPublisher, EventPublisher>();
        }
        private static void ConfigureTopology<MessageType>(IRabbitMqBusFactoryConfigurator config) where MessageType : class
        {
            config.Message<MessageType>(ct => { });
            config.AutoDelete = true;
        }
        private static void ConfigureReceiveEndPoint<ConsumerType>(IRabbitMqBusFactoryConfigurator config, IBusRegistrationContext context, string queueName) where ConsumerType : class, IConsumer
        {
            config.ReceiveEndpoint(queueName, exchangeConfigurator =>
            {
                exchangeConfigurator.AutoDelete = true;
                exchangeConfigurator.Consumer<ConsumerType>(context);
            });
        }
    }
}
