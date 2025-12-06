using DotNetApiEventBus.Di;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DotNetApiEventBus.Tests.EndToEnd
{
    public static class TestsConfig
    {
        public const string ApiUrl = "https://localhost:7102";
        public const string Api2Url = "https://localhost:7161";

        public const string DefaultHostName = "localhost";
        public const string DefaultUsername = "guest";
        public const string DefaultPassword = "guest";
        public const string Domain = "EventBus";
        public const string SubDomain = "TestsEndToEnd";
        public const int MaxNumberAttempts = 5;
        public const int AttemptDelayMs = 2000;

        public static Microsoft.Extensions.Hosting.IHost CreateHost()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>()
            {
                { $"{EventBusConfig.EventBusSectionName}:{EventBusConfig.ConfigurationSectionHostName}", TestsConfig.DefaultHostName },
                { $"{EventBusConfig.EventBusSectionName}:{EventBusConfig.ConfigurationSectionUsernameName}", TestsConfig.DefaultUsername },
                { $"{EventBusConfig.EventBusSectionName}:{EventBusConfig.ConfigurationSectionPasswordName}", TestsConfig.DefaultPassword },
                { $"{EventBusConfig.AppSectionName}:{EventBusConfig.DomainSectionName}", TestsConfig.Domain },
                { $"{EventBusConfig.AppSectionName}:{EventBusConfig.SubDomainSectionName}", TestsConfig.SubDomain },
            });
            builder.AddEventBusDependencies(Array.Empty<string>());
            return builder.Build();
        }
    }
}
