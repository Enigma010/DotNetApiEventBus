using DotNetApiEventBus.Di;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using DotNetApiEventBusCore;
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
        public const int DefaultPort = 5772;
        public const string Domain = "EventBus";
        public const string SubDomain = "TestsEndToEnd";
        public const int MaxNumberAttempts = 5;
        public const int AttemptDelayMs = 2000;
        public const string ContainerName = "DotNetApiEventBusTestsEndToEnd";

        public static Microsoft.Extensions.Hosting.IHost CreateHost()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            DddConfig.ConfigureEnvironmentVariables(TestsConfig.Domain, TestsConfig.SubDomain);
            EventBusConfig.ConfigureEnvironmentVariables(TestsConfig.DefaultHostName, TestsConfig.DefaultUsername, 
                TestsConfig.DefaultPassword, TestsConfig.DefaultPort);
            builder.AddEventBusDependencies(Array.Empty<string>());
            return builder.Build();
        }
    }
}
