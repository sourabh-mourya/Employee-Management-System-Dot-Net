using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class RegisterRequestDto
    {
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

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(Admin|Employee)$", ErrorMessage = "Role must be either Admin or Employee")]
        public string Roles { get; set; } = string.Empty;
    }

 
}