using DotNetApiEventBus.Tests.EndToEnd.Api.Services;
using DotNetApiEventBus.Tests.EndToEnd.Events;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApiEventBus.Tests.EndToEnd.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class EventOneController : ControllerBase
    {
        private readonly IEventOneService _service;
        public EventOneController(IEventOneService service)
        {
            _service = service;
        }
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<EventOne>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventOne), StatusCodes.Status200OK)]
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
