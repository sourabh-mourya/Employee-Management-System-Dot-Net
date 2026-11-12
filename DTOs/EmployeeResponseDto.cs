
namespace EmployeeManagementSystem.Models
{
    public class EmployeeResponseDto
    {
        public Guid Id { get; set; }              // Employee table ki Id
        public Guid UserId { get; set; }          // Users table ki Id

        public string fullName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;

        public string EmployeeCode { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime JoiningDate { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}