using Microsoft.AspNetCore.Mvc;
using SignalR.API.DTOs;
using SignalR.API.Services;

namespace SignalR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatServices _chatServices;

        public ChatController(ChatServices chatServices)
        {
            _chatServices = chatServices;
        }

        [HttpPost("register-user")]
        public IActionResult RegisterUser(UserDto model) 
        {
            if (_chatServices.AddUserToList(model.UserName))
            {
                return NoContent();
            }

            return BadRequest(model.UserName + " ya está asignado a otro usuario, por favor escoja uno diferente");
        }
    }
}
