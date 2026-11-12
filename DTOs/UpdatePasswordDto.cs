using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage = "Current password is required")]
        public string currentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter and one number")]
        public string newPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("newPassword", ErrorMessage = "New password and confirm password do not match")]
        public string confirmPassword { get; set; } = string.Empty;
    }
}