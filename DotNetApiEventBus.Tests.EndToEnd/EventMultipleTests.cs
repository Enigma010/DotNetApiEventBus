using DotNetApiEventBus.Tests.EndToEnd.Events;
using Microsoft.Extensions.Hosting;

namespace DotNetApiEventBus.Tests.EndToEnd
{
    [Collection("EndToEnd")]
    public class EventMultipleTests
    {
        private IHost _host;
        private readonly HttpClient _httpClient;
        private readonly Api.Client.RestClient _clientOne;
        private readonly Api2.Client.RestClient _clientTwo;
        private readonly EndToEndFixture _fixture;
        private readonly Func<IEvent, Task> _check;
        public EventMultipleTests(EndToEndFixture fixture)
        {
            _host = TestsConfig.CreateHost();
            _httpClient = new HttpClient();
            _clientOne = new Api.Client.RestClient(TestsConfig.ApiUrl, _httpClient);
            _clientTwo = new Api2.Client.RestClient(TestsConfig.Api2Url, _httpClient);
            _fixture = fixture;
            _check = async (@event) =>
            {
                if (@event is EventOne eventOne)
                {
                    await _clientOne.EventOneGETAsync(eventOne.Id);
                }
                else if (@event is EventTwo eventTwo)
                {
                    await _clientTwo.EventTwoGETAsync(eventTwo.Id);
                }
                else
                {
                    // Assert false here because we're not handling this event
                    Assert.False(true);
                }
            };
        }

        [Fact]
        public async Task PublishMultipleDifferentEvents()
        {
            List<IEvent> events = new List<IEvent>()
            {
                new Events.EventOne(),
                new Events.EventTwo()
            };
            await events.CheckEvents(_check, _check, _host);
        }
    }
}
