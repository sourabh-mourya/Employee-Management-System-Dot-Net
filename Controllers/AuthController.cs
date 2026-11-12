using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices authServices;

        public AuthController(IAuthServices authServices)
        {
            this.authServices = authServices;
        }

        
        [HttpPost("register")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var user = await authServices.RegisterAsync(request);

            if (user is null)
            {
                return Conflict(new { message = "User already exists with the same email" });
            }

            return Ok(new
            {
                message = "User registered successfully",
                data = user
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await authServices.LoginAsync(request);

            if (result is null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(new
            {
                message = "Login successful",
                data = result
            });
        }

        [HttpPost("change-password")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordDto request)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var result = await authServices.ChangePasswordAsync(userId, request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(new { message = result.Message });

        }

    }
}