using Microsoft.AspNetCore.Mvc;
using TTCore.StoreProvider.Middleware;
using TTCore.StoreProvider.Models;
using TTCore.StoreProvider.Services;

namespace TTCore.StoreProvider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IJwtService _userService;

        public UsersController(IJwtService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [JwtAuthorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
