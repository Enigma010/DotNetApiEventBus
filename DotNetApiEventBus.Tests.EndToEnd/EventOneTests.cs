using DotNetApiEventBus.Tests.EndToEnd.Events;
using Microsoft.Extensions.Hosting;
using DotNetApiEventBus.Tests.EndToEnd.Api.Client;

namespace DotNetApiEventBus.Tests.EndToEnd
{
    [Collection("EndToEnd")]
    public class EventOneTests
    {

        private IHost _host;
        private readonly HttpClient _httpClient;
        private readonly RestClient _client;
        private readonly EndToEndFixture _fixture;
        Func<IEvent, Task> _check;

        public EventOneTests(EndToEndFixture fixture)
        {
            _host = TestsConfig.CreateHost();
            _httpClient = new HttpClient();
            _client = new RestClient(TestsConfig.ApiUrl, _httpClient);
            _fixture = fixture;
            _check = async (@event) =>
            {
                await _client.EventOneGETAsync(@event.Id);
            };
        }

        [Fact]
        public async Task PublishSingleEvent()
        {
            var events = new List<IEvent> { new Events.EventOne() };
            await events.CheckEvents(_check, _host);
        }

        [Fact]
        public async Task PublishSingleEventWithTempFailure()
        {
            Events.EventOne eventOne = new Events.EventOne();
            eventOne.ThrowDuringProcessing = true;
            eventOne.SucceedOnAttemptNumber = 2;
            List<Events.EventOne> events = new List<Events.EventOne>()
            {
                eventOne
            };
            await events.CheckEvents(_check, _host);
        }

        [Fact]
        public async Task PublishMultipleEvents()
        {
            var events = new List<IEvent> { new Events.EventOne(), new Events.EventOne() };
            await events.CheckEvents(_check, _host);
        }

        [Fact]
        public async Task PublishSingleEventWithFailure()
        {
            Events.EventOne eventOne = new Events.EventOne();
            eventOne.ThrowDuringProcessing = true;
            // The max number of retries is 10 by default
            eventOne.SucceedOnAttemptNumber = 15;
            List<Events.EventOne> events = new List<Events.EventOne>()
            {
                eventOne
            };
            await events.CheckFailedEvents(_check, _host);
        }
    }
}