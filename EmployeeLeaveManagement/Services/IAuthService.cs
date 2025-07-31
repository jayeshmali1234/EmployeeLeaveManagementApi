using EmployeeLeaveManagement.DTOs;

namespace EmployeeLeaveManagement.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
        string GenerateJwtToken(int userId, string email, string role);
    }
} 