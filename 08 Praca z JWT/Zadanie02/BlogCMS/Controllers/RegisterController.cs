using BlogCMS.Models;
using BlogCMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisterController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        public RegisterController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Password = request.Password,
                Role = "Admin"
            };

            var userId = await _userRepository.AddAsync(user);

            return Ok(new
            {
                id = userId,
                username = user.Username,
                role = user.Role
            });
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}