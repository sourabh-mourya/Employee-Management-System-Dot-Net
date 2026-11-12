namespace EmployeeManagementSystem.Models
{
    public class RegisterResponseDto
    {
        public Guid Id { get; set; }
        public string fullName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
    }

  
}