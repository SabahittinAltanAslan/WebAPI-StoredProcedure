using Microsoft.AspNetCore.Mvc;
using UserService.Repositories;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            _userRepository.AddUser(user);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user)
        {
            _userRepository.UpdateUser(user);
            return Ok();
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            _userRepository.DeleteUser(userId);
            return Ok();
        }
    }
}
