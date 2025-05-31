using FCG.Application.DTO.Auth;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = FCG.Application.DTO.Auth.LoginRequest;

namespace FCG.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        /// <summary>
        /// Returns a JWT token for authentication.
        /// </summary>
        /// <returns>JWT Token</returns>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var user = _userService.ValidateCredentials(login.UserId, login.Password);

            if (user == null)
                return Unauthorized(new { Error = "User or password invalid." });

            var token = _authService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
