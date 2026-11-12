using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Interfaces
{
    public interface IAuthServices
    {
        Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

        Task<(bool Success, string Message)> ChangePasswordAsync(Guid userId, UpdatePasswordDto request);
    }
}