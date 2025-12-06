using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace DotNetApiEventBus
{
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class EventBusConfig
    {
        /// <summary>
        /// The default host for the event bus
        /// </summary>
        public const string DefaultHost = "host.docker.internal";
        /// <summary>
        /// The default user name
        /// </summary>
        public const string DefaultUsername = "guest";
        /// <summary>
        /// The default password
        /// </summary>
        public const string DefaultPassword = "guest";
        /// <summary>
        /// The port
        /// </summary>
        public const int DefaultPort = 5672;
        /// <summary>
        /// The section name in the configuration
        /// </summary>
        public const string EventBusSectionName = "EventBus";
        /// <summary>
        /// The application section name
        /// </summary>
        public const string AppSectionName = "App";
        /// <summary>
        /// The domain section name
        /// </summary>
        public const string DomainSectionName = "Domain";
        /// <summary>
        /// The sub domain section name
        /// </summary>
        public const string SubDomainSectionName = "SubDomain";
        /// <summary>
        /// The section name in the configuration
        /// </summary>
        public const string ConfigurationSectionHostName = "Host";
        /// <summary>
        /// The section name in the configuration
        /// </summary>
        public const string ConfigurationSectionUsernameName = "Username";
        /// <summary>
        /// The section name in the configuration
        /// </summary>
        public const string ConfigurationSectionPasswordName = "Password";
        /// <summary>
        /// The section name in the configuration
        /// </summary>
        public const string ConfigurationSectionQueueNameName = "QueueName";
        /// <summary>
        /// Creates a new event bus configuration
        /// </summary>
        /// <param name="configuration"></param>
        public EventBusConfig(IConfiguration configuration) 
        {
            IConfigurationSection eventBusSection = configuration.GetRequiredSection(EventBusSectionName);
            IConfigurationSection appSection = configuration.GetRequiredSection(AppSectionName);

            Host = eventBusSection[ConfigurationSectionHostName] ?? DefaultHost;
            Username = eventBusSection[ConfigurationSectionUsernameName] ?? DefaultUsername;
            Password = eventBusSection[ConfigurationSectionPasswordName] ?? DefaultPassword;

            Domain = appSection[DomainSectionName] ?? throw new InvalidOperationException("The domain is required");
            SubDomain = appSection[SubDomainSectionName] ?? throw new InvalidOperationException("The sub domain is required");
        }
        /// <summary>
        /// Creates a new event bus configuratino
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="domain">The domain</param>
        /// <param name="subDomain">The sub domain</param>
        public EventBusConfig(string host, string username, string password, string domain, string subDomain)
        {
            Host = host;
            Username = username;
            Password = password;
            Domain = domain ?? throw new InvalidOperationException("The domain is required");
            SubDomain = subDomain?? throw new InvalidOperationException("The sub domain is required");
        }

        /// <summary>
        /// The event bus host
        /// </summary>
        public string Host { get; private set; } = DefaultHost;
        /// <summary>
        /// The event bus username
        /// </summary>
        public string Username { get; private set; } = DefaultUsername;
        /// <summary>
        /// The event bus password
        /// </summary>
        public string Password { get; private set; } = DefaultPassword;
        /// <summary>
        /// The domain
        /// </summary>
        public string Domain { get; private set; } = string.Empty;
        /// <summary>
        /// The sub domain
        /// </summary>
        public string SubDomain { get; private set; } = string.Empty;
        /// <summary>
        /// The queue name
        /// </summary>
        public string QueueName
        {
            get
            {
                return $"{Domain}.{SubDomain}";
            }
        }
        /// <summary>
        /// The port
        /// </summary>
        public int Port { get; private set; } = DefaultPort;
        /// <summary>
        /// The connection string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return $"amqp://{Username}:{Password}@{Host}:{Port}";
            }
        }
    }
}
