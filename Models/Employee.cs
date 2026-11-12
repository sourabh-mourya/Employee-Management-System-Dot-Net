using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }          // FK -> Users.Id
        public User User { get; set; } = null!;

        [Required, StringLength(20)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        public DateTime JoiningDate { get; set; }

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}