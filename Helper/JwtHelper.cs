using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagementSystem.Models;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementSystem.Helper
{
    public class TokenHelper
    {
        private readonly IConfiguration configuration;

        public TokenHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, user.Roles)
            };

            var tokenKey = configuration["AppSettings:Token"];
            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new InvalidOperationException("AppSettings:Token is missing in configuration");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["AppSettings:Issuer"],
                audience: configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}