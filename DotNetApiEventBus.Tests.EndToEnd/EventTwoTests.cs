using DotNetApiEventBus.Tests.EndToEnd.Events;
using Microsoft.Extensions.Hosting;

namespace DotNetApiEventBus.Tests.EndToEnd
{
    [Collection("EndToEnd")]
    public class EventTwoTests
    {
        private IHost _host;
        private readonly HttpClient _httpClient;
        private readonly Api2.Client.RestClient _client;
        private readonly EndToEndFixture _fixture;
        Func<IEvent, Task> _check;

        public EventTwoTests(EndToEndFixture fixture)
        {
            _host = TestsConfig.CreateHost();
            _httpClient = new HttpClient();
            _client = new Api2.Client.RestClient(TestsConfig.Api2Url, _httpClient);
            _fixture = fixture;
            _check = async (@event) =>
            {
                await _client.EventTwoGETAsync(@event.Id);
            };
        }

        [Fact]
        public async Task PublishSingleEvent()
        {
            var events = new List<IEvent> { new Events.EventTwo() };
            await events.CheckEvents(_check, _host);
        }

        [Fact]
        public async Task PublishMultipleEvents()
        {
            var events = new List<IEvent> { new Events.EventTwo(), new Events.EventTwo() };
            await events.CheckEvents(_check, _host);
        }
    }
}
