using DotNetApiEventBus.Di;
using DotNetApiEventBus.Tests.EndToEnd.Api2.Services;
using DotNetApiLogging.Di;
using DotNetApiLogging;

namespace DotNetApiEventBus.Tests.EndToEnd.Api2.Di
{
    public static class DependencyInjector
    {
        public static void AddDependencies(this IHostApplicationBuilder builder)
        {
            builder.AddEventBusDependencies(
                "TestsApiEndToEnd",
                ["DotNetApiEventBus.Tests.EndToEnd.Api2"],
                ["DotNetApiEventBus.Tests.EndToEnd.Api2"]);
            builder.Services.AddScoped<IEventTwoService, EventTwoService>();
            builder.Services.AddScoped<IEventTwoConsumerService, EventTwoConsumerService>();
            var logConfig = new LogConfig();
            builder.Configuration.GetSection(nameof(LogConfig)).Bind(logConfig);
            builder.AddLoggerDependencies(logConfig);
        }
    }
}
