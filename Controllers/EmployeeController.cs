using System.Security.Claims;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]                       // <-- ye missing tha
    [Authorize(Roles = "Employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices employeeServices;

        public EmployeeController(IEmployeeServices employeeServices)
        {
            this.employeeServices = employeeServices;
        }

        [HttpGet("employees/{id}")]
        public async Task<IActionResult> GetSingleEmployee(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out var currentUserId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var employee = await employeeServices.GetSingleEmployeeAsync(id);

            if (employee is null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            if (employee.UserId != currentUserId)
            {
                return StatusCode(403, new { message = "You can not see another employee's details" });
            }

            return Ok(new
            {
                message = "Employee data fetched successfully",
                data = employee
            });
        }

        [HttpPatch("update-employee")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto request)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var employee = await employeeServices.UpdateProfile(userId, request);

            if (employee is null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            return Ok(new
            {
                message = "Profile updated successfully",
                data = employee
            });
        }
    }
}