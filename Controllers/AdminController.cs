using System.Security.Claims;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IEmployeeServices employeeServices;

        public AdminController(IEmployeeServices employeeServices)
        {
            this.employeeServices = employeeServices;
        }

        [HttpPost("create-employee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto request)
        {
            var employee = await employeeServices.CreateEmployeeAsync(request);

            if (employee is null)
            {
                return Conflict(new { message = "Employee code or email already exists" });
            }

            return Ok(new
            {
                message = "Employee created successfully",
                data = employee
            });
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await employeeServices.GetAllAsync();

            if (employees.Count == 0)
            {
                return NotFound(new { message = "No employee has been created yet" });
            }

            return Ok(new
            {
                message = "Employees fetched successfully",
                count = employees.Count,
                data = employees
            });
        }

        [HttpPatch("employees/{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployeeDto request)
        {
            var employee = await employeeServices.UpdateEmployeeAsync(id, request);

            if (employee is null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            return Ok(new
            {
                message = "Employee updated successfully",
                data = employee
            });
        }

        [HttpDelete("employees/{id}")]
        public async Task<IActionResult> DeactivateEmployee(Guid id)
        {
            var result = await employeeServices.DeactivateAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Employee not found" });
            }

            return Ok(new { message = "Employee deactivated successfully" });
        }

        [HttpGet("employees/{id}")]
        public async Task<IActionResult> GetSingleEmployee(Guid id)
        {
            var employee = await employeeServices.GetSingleEmployeeAsync(id);

            if (employee is null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            return Ok(new
            {
                message = "Employee data fetched successfully",
                data = employee
            });
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await employeeServices.GetDashboardAsync();

            return Ok(new
            {
                message = "Dashboard data fetched successfully",
                data = dashboard
            });
        }
    }
}
