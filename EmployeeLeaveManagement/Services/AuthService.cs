using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.DTOs;
using EmployeeLeaveManagement.Models;

namespace EmployeeLeaveManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = GenerateJwtToken(user.Id, user.Email, user.Role.ToString());

            return new LoginResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = Enum.Parse<UserRole>(request.Role),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Initialize leave balances for new employee
            if (user.Role == UserRole.Employee)
            {
                await InitializeLeaveBalancesAsync(user.Id);
            }

            return true;
        }

        public string GenerateJwtToken(int userId, string email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task InitializeLeaveBalancesAsync(int userId)
        {
            var leaveTypes = Enum.GetValues<LeaveType>();
            var currentYear = DateTime.Now.Year;

            foreach (var leaveType in leaveTypes)
            {
                var totalDays = leaveType switch
                {
                    LeaveType.Vacation => 20,
                    LeaveType.SickLeave => 10,
                    LeaveType.PersonalLeave => 5,
                    LeaveType.MaternityLeave => 90,
                    LeaveType.PaternityLeave => 15,
                    _ => 0
                };

                var leaveBalance = new LeaveBalance
                {
                    UserId = userId,
                    LeaveType = leaveType,
                    TotalDays = totalDays,
                    UsedDays = 0,
                    Year = currentYear,
                    LastUpdated = DateTime.UtcNow
                };

                _context.LeaveBalances.Add(leaveBalance);
            }

            await _context.SaveChangesAsync();
        }
    }
} 