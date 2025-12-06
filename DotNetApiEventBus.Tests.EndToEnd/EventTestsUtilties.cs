using DotNetApiEventBus.Tests.EndToEnd.Events;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetApiEventBus.Tests.EndToEnd
{
    public static class EventTestsUtilties
    {
        public static async Task CheckEvents(this IEnumerable<IEvent> expectedEvents, 
            Func<IEvent, Task> checkFound,
            Microsoft.Extensions.Hosting.IHost host
            )
        {
            await expectedEvents.CheckEventsNotFound(checkFound);
            await expectedEvents.PublishEvents(host);
            await expectedEvents.CheckEventsFound(checkFound);
        }

        public static async Task CheckFailedEvents(this IEnumerable<IEvent> expectedEvents,
            Func<IEvent, Task> checkFound,
            Microsoft.Extensions.Hosting.IHost host
    )
        {
            await expectedEvents.CheckEventsNotFound(checkFound);
            await expectedEvents.PublishEvents(host);
            await expectedEvents.CheckEventsNotFound(checkFound, TestsConfig.MaxNumberAttempts);
        }

        public static async Task CheckEventsNotFound(this IEnumerable<IEvent> expectedEvents, Func<IEvent, Task> checkNotFound, int maxNumberAttempts = 1)
        {
            foreach (var expectedEvent in expectedEvents)
            {
                for (int attemptNumber = 0; attemptNumber < maxNumberAttempts; attemptNumber++)
                {
                    try
                    {
                        await checkNotFound(expectedEvent);
                        Assert.Fail();
                    }
                    catch (Api2.Client.ApiException<Api2.Client.ProblemDetails>)
                    {
                    }
                    catch (Api.Client.ApiException<Api.Client.ProblemDetails>)
                    {
                    }
                    catch (Exception)
                    {
                        Assert.Fail();
                    }
                    Thread.Sleep(TestsConfig.AttemptDelayMs);
                }
            }
        }

        public static async Task PublishEvents(this IEnumerable<IEvent> expectedEvents, Microsoft.Extensions.Hosting.IHost host)
        {
            IEventPublisher publisher = host.Services.GetRequiredService<IEventPublisher>();
            await publisher.Publish(expectedEvents);
        }

        public static async Task CheckEventsFound(this IEnumerable<IEvent> expectedEvents, Func<IEvent, Task> checkFound)
        {
            foreach (var expectedEvent in expectedEvents.ToList())
            {
                bool found = false;
                for (int attemptNumber = 0; attemptNumber < TestsConfig.MaxNumberAttempts && !found; attemptNumber++)
                {
                    try
                    {
                        await checkFound(expectedEvent);
                        found = true;
                    }
                    catch
                    {
                    }
                    if (!found)
                    {
                        Thread.Sleep(TestsConfig.AttemptDelayMs);
                    }
                }
                Assert.True(found);
            }
        }
    }
}
