using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class CreateEmployeeDto
    {
        // ---- Login account (Users table) ----
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
        public string fullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        public string email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter and one number")]
        public string password { get; set; } = string.Empty;

        // ---- Employee profile (Employees table) ----
        [Required(ErrorMessage = "Employee code is required")]
        [StringLength(20, ErrorMessage = "Employee code cannot exceed 20 characters")]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Designation is required")]
        [StringLength(100, ErrorMessage = "Designation cannot exceed 100 characters")]
        public string Designation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters")]
        public string Department { get; set; } = string.Empty;

        [Range(1, 10000000, ErrorMessage = "Salary must be greater than 0")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Joining date is required")]
        public DateTime JoiningDate { get; set; }

        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10 digit phone number")]
        public string? PhoneNumber { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string? Address { get; set; }
    }
}