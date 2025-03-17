using BusinessLayer.Interface;
using ModelLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AddressBookApplication.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var result = await _authService.Register(userDTO);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var token = await _authService.Login(userDTO);
            if (token == "Invalid credentials!")
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new { token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            var result = await _authService.ForgotPassword(forgotPasswordDTO);
            return Ok(new { message = result });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var result = await _authService.ResetPassword(resetPasswordDTO);
            return Ok(new { message = result });
        }
    }
}
