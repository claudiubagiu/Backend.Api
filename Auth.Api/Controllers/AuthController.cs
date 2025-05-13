using Auth.Models.DTOs;
using Auth.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var isUserBanned = await authService.CheckBan(loginRequestDto.Email);
            if (isUserBanned)
                return Unauthorized(new { message = "You are banned!" });
            var response = await authService.Login(loginRequestDto);
            if (response != null)
                return Ok(response);
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var response = await authService.Register(registerRequestDto);
            if (response != null)
            {
                return Ok(new { message = "User was registered! Please login." });
            }
            return BadRequest();
        }
    }
}
