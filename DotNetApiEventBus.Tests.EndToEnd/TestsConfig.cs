using DotNetApiEventBus.Di;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using MassTransit.Transports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DotNetApiEventBus.Tests.EndToEnd
{
    public static class TestsConfig
    {
        public const string ApiUrl = "https://localhost:7102";
        public const string Api2Url = "https://localhost:7161";

        public const string DefaultHostName = "localhost";
        public const string DefaultUsername = "";
        public const string DefaultPassword = "";
        public const string DefaultQueueName = "";
        public const int MaxNumberAttempts = 5;
        public const int AttemptDelayMs = 2000;

        public static Microsoft.Extensions.Hosting.IHost CreateHost()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            App app = new App();
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>()
            {
                { $"{EventBusConfig.ConfigurationSectionName}:{EventBusConfig.ConfigurationSectionHostName}", TestsConfig.DefaultHostName },
                { $"{EventBusConfig.ConfigurationSectionName}:{EventBusConfig.ConfigurationSectionUsernameName}", TestsConfig.DefaultUsername },
                { $"{EventBusConfig.ConfigurationSectionName}:{EventBusConfig.ConfigurationSectionPasswordName}", TestsConfig.DefaultPassword },
                { $"{EventBusConfig.ConfigurationSectionName}:{EventBusConfig.ConfigurationSectionQueueNameName}", app.Name }
            });
            builder.AddEventBusDependencies(app.Name, ["DotNetApiEventBus.Tests.Events.EndToEnd"], Array.Empty<string>());
            return builder.Build();
        }
    }
}
