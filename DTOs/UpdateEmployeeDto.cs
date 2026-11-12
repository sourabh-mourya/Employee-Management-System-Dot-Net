using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class UpdateEmployeeDto
    {
        [StringLength(100, MinimumLength = 2)]
        public string? fullName { get; set; }

        [StringLength(100)]
        public string? Designation { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        [Range(1, 10000000, ErrorMessage = "Salary must be greater than 0")]
        public decimal? Salary { get; set; }

        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10 digit phone number")]
        public string? PhoneNumber { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }
    }
}