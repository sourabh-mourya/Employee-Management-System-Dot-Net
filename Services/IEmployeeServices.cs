
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Interfaces
{
    public interface IEmployeeServices
    {
        public Task<EmployeeResponseDto?> CreateEmployeeAsync(CreateEmployeeDto request);
        Task<List<EmployeeResponseDto>> GetAllAsync();
        Task<EmployeeResponseDto?> GetSingleEmployeeAsync(Guid id); Task<EmployeeResponseDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto request);
        Task<bool> DeactivateAsync(Guid id);

        Task<EmployeeResponseDto?> UpdateProfile(Guid userId, UpdateProfileDto request);
        Task<DashboardResponseDto> GetDashboardAsync();
    }
}