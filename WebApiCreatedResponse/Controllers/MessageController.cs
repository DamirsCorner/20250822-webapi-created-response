using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using WebApiCreatedResponse.Models;

namespace WebApiCreatedResponse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private ConcurrentDictionary<Guid, Message> _messages = new();

        private Message Create(Message message)
        {
            return _messages.AddOrUpdate(
                message.Id,
                message,
                (id, msg) => throw new ArgumentException()
            );
        }

        [HttpPost("empty")]
        public IActionResult CreateEmpty(Message request)
        {
            try
            {
                var message = Create(request);
                return Created();
            }
            catch (ArgumentException)
            {
                return Conflict();
            }
        }

        [HttpPost("location")]
        public IActionResult CreateWithLocation(Message request)
        {
            try
            {
                var message = Create(request);
                return Created(Url.Action("GetMessage", new { id = message.Id }), null);
            }
            catch (ArgumentException)
            {
                return Conflict();
            }
        }

        [HttpPost("workaround")]
        public IActionResult CreateWithWorkaround(Message request)
        {
            try
            {
                var message = Create(request);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (ArgumentException)
            {
                return Conflict();
            }
        }

        [HttpGet("{id:guid}", Name = "GetMessage")]
        public IActionResult Get(Guid id)
        {
            if (_messages.TryGetValue(id, out var message))
            {
                return Ok(message);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
