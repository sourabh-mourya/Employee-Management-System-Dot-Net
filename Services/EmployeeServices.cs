using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly MyDbContext myDbContext;

        public EmployeeServices(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        public async Task<EmployeeResponseDto?> CreateEmployeeAsync(CreateEmployeeDto request)
        {
            // same email ya same employee code se 2 baar create nahi kar sakte
            if (await myDbContext.users.AnyAsync(u => u.email == request.email) ||
                await myDbContext.employees.AnyAsync(e => e.EmployeeCode == request.EmployeeCode))
            {
                return null;
            }

            // do kaam saath me ho rahe hain (users + employees), isliye transaction:
            // dono save honge to hi commit, warna kuch save nahi hoga
            using var transaction = await myDbContext.Database.BeginTransactionAsync();

            try
            {
                // 1. users table me login account
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    fullName = request.fullName,
                    email = request.email,
                    passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password),
                    Roles = "Employee",
                    isActive = true
                };
                await myDbContext.users.AddAsync(user);

                // 2. employees table me profile
                var employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    EmployeeCode = request.EmployeeCode,
                    Designation = request.Designation,
                    Department = request.Department,
                    Salary = request.Salary,
                    JoiningDate = request.JoiningDate,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address
                };
                await myDbContext.employees.AddAsync(employee);
                await myDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new EmployeeResponseDto
                {
                    Id = employee.Id,
                    UserId = user.Id,
                    fullName = user.fullName,
                    email = user.email,
                    EmployeeCode = employee.EmployeeCode,
                    Designation = employee.Designation,
                    Department = employee.Department,
                    Salary = employee.Salary,
                    JoiningDate = employee.JoiningDate,
                    PhoneNumber = employee.PhoneNumber,
                    Address = employee.Address,
                    isActive = user.isActive,
                    CreatedAt = employee.CreatedAt,
                    UpdatedAt = employee.UpdatedAt
                };
            }
            catch
            {
                // beech me koi dikkat aayi to sab rollback
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<List<EmployeeResponseDto>> GetAllAsync()
        {
            var employees = await myDbContext.employees
                .AsNoTracking()
                .Include(e => e.User)//Include(e => e.User). fullName, email aur isActive Users table me hain, to join zaroori hai. Note: jab Select me seedha e.User.fullName use karte ho to EF join khud bana leta hai, aur Include technically optional ho jata hai. Rakhne se nuksan nahi, par hata bhi sakte ho.
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    fullName = e.User.fullName,
                    email = e.User.email,
                    EmployeeCode = e.EmployeeCode,
                    Designation = e.Designation,
                    Department = e.Department,
                    Salary = e.Salary,
                    JoiningDate = e.JoiningDate,
                    PhoneNumber = e.PhoneNumber,
                    Address = e.Address,
                    isActive = e.User.isActive,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
                .ToListAsync();

            return employees;
        }


        public async Task<EmployeeResponseDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto request)
        {
            var employee = await myDbContext.employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee is null)
            {
                return null;
            }

            // jo field bheji gayi hai sirf wahi update hogi
            if (request.fullName is not null)
            {
                employee.User.fullName = request.fullName;
            }

            if (request.Designation is not null)
            {
                employee.Designation = request.Designation;
            }

            if (request.Department is not null)
            {
                employee.Department = request.Department;
            }

            if (request.Salary is not null)
            {
                employee.Salary = request.Salary.Value;
            }

            if (request.PhoneNumber is not null)
            {
                employee.PhoneNumber = request.PhoneNumber;
            }

            if (request.Address is not null)
            {
                employee.Address = request.Address;
            }

            employee.UpdatedAt = DateTime.UtcNow;

            await myDbContext.SaveChangesAsync();

            return new EmployeeResponseDto
            {
                Id = employee.Id,
                UserId = employee.UserId,
                fullName = employee.User.fullName,
                email = employee.User.email,
                EmployeeCode = employee.EmployeeCode,
                Designation = employee.Designation,
                Department = employee.Department,
                Salary = employee.Salary,
                JoiningDate = employee.JoiningDate,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                isActive = employee.User.isActive,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }



        public async Task<bool> DeactivateAsync(Guid id)
        {
            var employee = await myDbContext.employees.Include(e => e.User).FirstOrDefaultAsync(u => u.Id == id);

            if (employee is null) return false;

            employee.User.isActive = false;
            employee.UpdatedAt = DateTime.UtcNow;

            await myDbContext.SaveChangesAsync();
            return true;


        }

        public async Task<EmployeeResponseDto?> GetSingleEmployeeAsync(Guid id)
        {
            var employee = await myDbContext.employees
                .AsNoTracking()
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee is null)
            {
                return null;
            }

            return new EmployeeResponseDto
            {
                Id = employee.Id,
                UserId = employee.UserId,
                fullName = employee.User.fullName,
                email = employee.User.email,
                EmployeeCode = employee.EmployeeCode,
                Designation = employee.Designation,
                Department = employee.Department,
                Salary = employee.Salary,
                JoiningDate = employee.JoiningDate,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                isActive = employee.User.isActive,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }

        public async Task<EmployeeResponseDto?> UpdateProfile(Guid userId, UpdateProfileDto request)
        {
            // userId token se aayi hai, isliye UserId column se match karo
            var employee = await myDbContext.employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee is null)
            {
                return null;
            }

            if (request.PhoneNumber is not null)
            {
                employee.PhoneNumber = request.PhoneNumber;
            }

            if (request.Address is not null)
            {
                employee.Address = request.Address;
            }
            employee.UpdatedAt = DateTime.UtcNow;

            await myDbContext.SaveChangesAsync();

            return new EmployeeResponseDto
            {
                Id = employee.Id,
                UserId = employee.UserId,
                fullName = employee.User.fullName,
                email = employee.User.email,
                EmployeeCode = employee.EmployeeCode,
                Designation = employee.Designation,
                Department = employee.Department,
                Salary = employee.Salary,
                JoiningDate = employee.JoiningDate,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                isActive = employee.User.isActive,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }


        public async Task<DashboardResponseDto> GetDashboardAsync()
        {
            var totalEmployees = await myDbContext.employees.CountAsync();

            var activeEmployees = await myDbContext.employees
                .CountAsync(e => e.User.isActive);

            var departments = await myDbContext.employees
                .GroupBy(e => e.Department)
                .Select(g => new DepartmentStatDto
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return new DashboardResponseDto
            {
                TotalEmployees = totalEmployees,
                ActiveEmployees = activeEmployees,
                InactiveEmployees = totalEmployees - activeEmployees,
                Departments = departments
            };
        }
    }

}


