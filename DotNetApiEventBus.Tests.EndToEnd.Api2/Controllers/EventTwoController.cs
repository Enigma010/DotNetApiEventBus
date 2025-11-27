using DotNetApiEventBus.Tests.EndToEnd.Api2.Services;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApiEventBus.Tests.EndToEnd.Api2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class EventTwoController : ControllerBase
    {
        private readonly IEventTwoService _service;
        public EventTwoController(IEventTwoService service)
        {
            _service = service;
        }
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<EventTwo>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventTwo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromRoute]Guid id)
        {
            var eventOne = _service.Get(id);
            return eventOne != null ? Ok(eventOne) : NotFound();
        }
        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Delete()
        {
            _service.Delete();
            return Ok();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Delete([FromRoute]Guid id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
