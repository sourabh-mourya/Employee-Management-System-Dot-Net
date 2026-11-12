using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
        public string fullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        public string email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password hash is required")]
        [StringLength(255)]
        public string passwordHash { get; set; } = string.Empty;

        public bool isActive { get; set; } = true;

        [Required(ErrorMessage = "Roles is required")]
        [RegularExpression("^(Admin|Employee)$", ErrorMessage = "Role must be either Admin or Employee")]
        public string Roles { get; set; } = string.Empty;
        public Employee? Employee { get; set; }
    }
}