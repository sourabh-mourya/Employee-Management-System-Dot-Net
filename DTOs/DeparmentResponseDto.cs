namespace EmployeeManagementSystem.Models
{   
    public class DashboardResponseDto
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }
        public List<DepartmentStatDto> Departments { get; set; } = new();
    }
}