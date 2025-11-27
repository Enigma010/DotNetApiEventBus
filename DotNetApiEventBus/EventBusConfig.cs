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
        /// The default password
        /// </summary>
        public const string DefaultQueueName = "eventbusqueue";
        /// <summary>
        /// The section name in the configuration
        /// </summary>
        public const string ConfigurationSectionName = "EventBus";
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
            IConfigurationSection section = configuration.GetSection(ConfigurationSectionName);

            Host = section[ConfigurationSectionHostName] ?? DefaultHost;
            Username = section[ConfigurationSectionUsernameName] ?? DefaultUsername;
            Password = section[ConfigurationSectionPasswordName] ?? DefaultPassword;
            QueueName = section[ConfigurationSectionQueueNameName] ?? DefaultQueueName;
        }
        /// <summary>
        /// Creates a new event bus configuratino
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="queueName">The queue name</param>
        public EventBusConfig(string host, string username, string password, string queueName)
        {
            Host = host;
            Username = username;
            Password = password;
            QueueName = queueName;
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
        /// The event bus queue name
        /// </summary>
        public string QueueName { get; private set; } = DefaultQueueName;
    }
}
