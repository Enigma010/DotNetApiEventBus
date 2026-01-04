using DotNetApiEventBusCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace DotNetApiEventBus
{
    [ExcludeFromCodeCoverage(Justification = "Core infrastructure, unit tests would at a lower level")]
    public class EventBusConfig : IEventBusConfig
    {
        public const string EventBusHostKey = "EVENT_BUS_HOST";
        public const string EventBusUsernameKey = "EVENT_BUS_USERNAME";
        public const string EventBusPasswordKey = "EVENT_BUS_PASSWORD";
        public const string EventBusPortKey = "EVENT_BUS_PORT";

        public enum Configs
        {
            [Config<string>(name: EventBusHostKey, defaultValue: EventBusConfig.DefaultHost)]
            Host,
            [Config<string>(name: EventBusUsernameKey, defaultValue: EventBusConfig.DefaultUsername)]
            UserName,
            [Config<string>(name: EventBusPasswordKey, defaultValue: EventBusConfig.DefaultPassword)]
            Password,
            [Config<int>(name: EventBusPortKey, defaultValue: EventBusConfig.DefaultPort)]
            Port
        }

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
        /// Creates a new event bus configuration
        /// </summary>
        /// <param name="configuration"></param>
        public EventBusConfig(IConfiguration configuration, IDddConfig dddConfig)
        {
            Host = Configs.Host.GetRequiredValue<string>(configuration);
            Username = Configs.UserName.GetRequiredValue<string>(configuration);
            Password = Configs.Password.GetRequiredValue<string>(configuration);
            Port = Configs.Port.GetRequiredValue<int>(configuration);
            DddConfig = dddConfig;
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
        /// The domain driven design configuration
        /// </summary>
        public IDddConfig DddConfig { get; private set; }
        /// <summary>
        /// The queue name
        /// </summary>
        public string QueueName
        {
            get
            {
                return $"{DddConfig.Domain}-{DddConfig.SubDomain}";
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
        /// <summary>
        /// Configures environment variables required for connecting to the event bus using the specified connection
        /// parameters.
        /// </summary>
        /// <remarks>This method sets environment variables for the event bus connection, which may be
        /// used by other components at runtime. Calling this method will overwrite any existing values for these
        /// environment variables.</remarks>
        /// <param name="eventBusHostName">The host name or IP address of the event bus server. Cannot be null.</param>
        /// <param name="eventBusUsername">The user name to use when authenticating with the event bus. Cannot be null.</param>
        /// <param name="eventBusPassword">The password to use when authenticating with the event bus. Cannot be null.</param>
        /// <param name="eventBusPort">The port number on which the event bus server is listening. Must be a valid TCP port number.</param>
        public static void ConfigureEnvironmentVariables(string eventBusHostName,
            string eventBusUsername, string eventBusPassword, int eventBusPort)
        {
            Environment.SetEnvironmentVariable(Configs.Host.GetKey<string>(), eventBusHostName);
            Environment.SetEnvironmentVariable(Configs.UserName.GetKey<string>(), eventBusUsername);
            Environment.SetEnvironmentVariable(Configs.Password.GetKey<string>(), eventBusPassword);
            Environment.SetEnvironmentVariable(Configs.Port.GetKey<int>(), eventBusPort.ToString());
        }
    }
}
