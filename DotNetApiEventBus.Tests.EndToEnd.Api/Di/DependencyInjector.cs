using DotNetApiEventBus.Di;
using DotNetApiEventBus.Tests.EndToEnd.Api.Services;
using DotNetApiLogging;
using DotNetApiLogging.Di;

namespace DotNetApiEventBus.Tests.EndToEnd.Api.Di
{
    public static class DependencyInjector
    {
        public static void AddDependencies(this IHostApplicationBuilder builder)
        {
            builder.AddEventBusDependencies(["DotNetApiEventBus.Tests.EndToEnd.Api"]);
            builder.Services.AddScoped<IEventOneService, EventOneService>();
            builder.Services.AddScoped<IEventOneSubscriberService, EventOneSubscriberService>();
            var logConfig = new LogConfig();
            builder.Configuration.GetSection(nameof(LogConfig)).Bind(logConfig);
            builder.AddLoggerDependencies(logConfig);
        }
    }
}
