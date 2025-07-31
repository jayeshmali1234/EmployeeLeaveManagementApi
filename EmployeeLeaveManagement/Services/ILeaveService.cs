using EmployeeLeaveManagement.DTOs;

namespace EmployeeLeaveManagement.Services
{
    public interface ILeaveService
    {
        Task<LeaveRequestDTO> CreateLeaveRequestAsync(int employeeId, CreateLeaveRequestDTO request);
        Task<LeaveRequestDTO> UpdateLeaveRequestAsync(int requestId, int managerId, UpdateLeaveRequestDTO request);
        Task<List<LeaveRequestDTO>> GetEmployeeLeaveRequestsAsync(int employeeId);
        Task<List<LeaveRequestDTO>> GetPendingLeaveRequestsAsync(int managerId);
        Task<List<LeaveBalanceDTO>> GetEmployeeLeaveBalancesAsync(int employeeId);
        Task<List<LeaveCalendarDTO>> GetLeaveCalendarAsync(DateTime startDate, DateTime endDate);
        Task<LeaveRequestDTO> GetLeaveRequestByIdAsync(int requestId);
        Task<bool> CancelLeaveRequestAsync(int requestId, int employeeId);
    }
} 