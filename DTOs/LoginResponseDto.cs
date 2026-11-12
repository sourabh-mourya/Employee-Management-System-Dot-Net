namespace EmployeeManagementSystem.Models
{

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
    }
}