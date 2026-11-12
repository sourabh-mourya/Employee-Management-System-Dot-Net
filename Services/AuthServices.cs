using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Helper;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly MyDbContext myDbContext;
        private readonly TokenHelper tokenHelper;

        public AuthServices(MyDbContext myDbContext, TokenHelper tokenHelper)
        {
            this.myDbContext = myDbContext;
            this.tokenHelper = tokenHelper;
        }

        public async Task<RegisterResponseDto?> RegisterAsync(RegisterRequestDto request)
        {
            // 1. email already exists?
            if (await myDbContext.users.AnyAsync(u => u.email == request.email))
            {
                return null;
            }

            // 2. role check (galat role aaye to Employee)
            var allowedRoles = new[] { "Admin", "Employee" };
            string role = allowedRoles.Contains(request.Roles) ? request.Roles : "Employee";

            // 3. user banao, password hash karke
            var user = new User
            {
                Id = Guid.NewGuid(),
                fullName = request.fullName,
                email = request.email,
                passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password),
                Roles = role,
                isActive = true
            };

            await myDbContext.users.AddAsync(user);
            await myDbContext.SaveChangesAsync();

            // 4. entity nahi, response DTO return karo (passwordHash ke bina)
            return new RegisterResponseDto
            {
                Id = user.Id,
                fullName = user.fullName,
                email = user.email,
                Roles = user.Roles
            };
        }


        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await myDbContext.users.FirstOrDefaultAsync(u => u.email == request.email);

            if (user is null || !user.isActive)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(request.password, user.passwordHash))
            {
                return null;
            }

            return new LoginResponseDto
            {
                Token = tokenHelper.CreateToken(user),
                email = user.email,
                Roles = user.Roles
            };
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(Guid userId, UpdatePasswordDto request)
        {
            var user = await myDbContext.users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null || !user.isActive)
            {
                return (false, "User not found");
            }

            // 1. purana password sahi hai?
            if (!BCrypt.Net.BCrypt.Verify(request.currentPassword, user.passwordHash))
            {
                return (false, "Current password is incorrect");
            }

            // 2. naya password purane jaisa to nahi?
            if (BCrypt.Net.BCrypt.Verify(request.newPassword, user.passwordHash))
            {
                return (false, "New password cannot be the same as the current password");
            }

            user.passwordHash = BCrypt.Net.BCrypt.HashPassword(request.newPassword);
            await myDbContext.SaveChangesAsync();

            return (true, "Password changed successfully");
        }

    }
}