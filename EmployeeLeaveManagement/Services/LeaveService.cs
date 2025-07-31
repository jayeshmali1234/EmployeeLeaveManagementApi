using Microsoft.EntityFrameworkCore;
using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.DTOs;
using EmployeeLeaveManagement.Models;

namespace EmployeeLeaveManagement.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ApplicationDbContext _context;

        public LeaveService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveRequestDTO> CreateLeaveRequestAsync(int employeeId, CreateLeaveRequestDTO request)
        {
            // Validate dates
            if (request.StartDate < DateTime.Today)
                throw new InvalidOperationException("Start date cannot be in the past");

            if (request.EndDate < request.StartDate)
                throw new InvalidOperationException("End date cannot be before start date");

            // Check leave balance
            var leaveBalance = await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.UserId == employeeId && 
                                         lb.LeaveType == request.LeaveType && 
                                         lb.Year == DateTime.Now.Year);

            if (leaveBalance == null)
                throw new InvalidOperationException("Leave balance not found");

            var requestedDays = (request.EndDate - request.StartDate).Days + 1;
            if (leaveBalance.RemainingDays < requestedDays)
                throw new InvalidOperationException($"Insufficient leave balance. Available: {leaveBalance.RemainingDays} days");

            // Get manager for approval
            var manager = await _context.Users
                .FirstOrDefaultAsync(u => u.Role == UserRole.Manager && u.IsActive);

            var leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                ManagerId = manager?.Id,
                LeaveType = request.LeaveType,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Reason = request.Reason,
                Status = LeaveStatus.Pending,
                RequestedAt = DateTime.UtcNow
            };

            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();

            return await GetLeaveRequestDTOAsync(leaveRequest);
        }

        public async Task<LeaveRequestDTO> UpdateLeaveRequestAsync(int requestId, int managerId, UpdateLeaveRequestDTO request)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.Manager)
                .FirstOrDefaultAsync(lr => lr.Id == requestId);

            if (leaveRequest == null)
                throw new InvalidOperationException("Leave request not found");

            if (leaveRequest.Status != LeaveStatus.Pending)
                throw new InvalidOperationException("Leave request has already been processed");

            leaveRequest.Status = request.Status;
            leaveRequest.ManagerComments = request.ManagerComments;
            leaveRequest.ProcessedAt = DateTime.UtcNow;

            // Update leave balance if approved
            if (request.Status == LeaveStatus.Approved)
            {
                await UpdateLeaveBalanceAsync(leaveRequest.EmployeeId, leaveRequest.LeaveType, leaveRequest.TotalDays);
            }

            await _context.SaveChangesAsync();

            return await GetLeaveRequestDTOAsync(leaveRequest);
        }

        public async Task<List<LeaveRequestDTO>> GetEmployeeLeaveRequestsAsync(int employeeId)
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.Manager)
                .Where(lr => lr.EmployeeId == employeeId)
                .OrderByDescending(lr => lr.RequestedAt)
                .ToListAsync();

            return leaveRequests.Select(lr => new LeaveRequestDTO
            {
                Id = lr.Id,
                EmployeeName = lr.Employee.Name,
                ManagerName = lr.Manager?.Name,
                LeaveType = lr.LeaveType,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                Reason = lr.Reason,
                Status = lr.Status,
                ManagerComments = lr.ManagerComments,
                RequestedAt = lr.RequestedAt,
                ProcessedAt = lr.ProcessedAt,
                TotalDays = lr.TotalDays
            }).ToList();
        }

        public async Task<List<LeaveRequestDTO>> GetPendingLeaveRequestsAsync(int managerId)
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.Manager)
                .Where(lr => lr.Status == LeaveStatus.Pending)
                .OrderBy(lr => lr.RequestedAt)
                .ToListAsync();

            return leaveRequests.Select(lr => new LeaveRequestDTO
            {
                Id = lr.Id,
                EmployeeName = lr.Employee.Name,
                ManagerName = lr.Manager?.Name,
                LeaveType = lr.LeaveType,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                Reason = lr.Reason,
                Status = lr.Status,
                ManagerComments = lr.ManagerComments,
                RequestedAt = lr.RequestedAt,
                ProcessedAt = lr.ProcessedAt,
                TotalDays = lr.TotalDays
            }).ToList();
        }

        public async Task<List<LeaveBalanceDTO>> GetEmployeeLeaveBalancesAsync(int employeeId)
        {
            var leaveBalances = await _context.LeaveBalances
                .Where(lb => lb.UserId == employeeId && lb.Year == DateTime.Now.Year)
                .ToListAsync();

            return leaveBalances.Select(lb => new LeaveBalanceDTO
            {
                LeaveType = lb.LeaveType,
                TotalDays = lb.TotalDays,
                UsedDays = lb.UsedDays,
                RemainingDays = lb.RemainingDays,
                Year = lb.Year
            }).ToList();
        }

        public async Task<List<LeaveCalendarDTO>> GetLeaveCalendarAsync(DateTime startDate, DateTime endDate)
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Where(lr => lr.Status == LeaveStatus.Approved &&
                            lr.StartDate <= endDate &&
                            lr.EndDate >= startDate)
                .ToListAsync();

            return leaveRequests.Select(lr => new LeaveCalendarDTO
            {
                Id = lr.Id,
                EmployeeName = lr.Employee.Name,
                LeaveType = lr.LeaveType,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                Status = lr.Status
            }).ToList();
        }

        public async Task<LeaveRequestDTO> GetLeaveRequestByIdAsync(int requestId)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .Include(lr => lr.Manager)
                .FirstOrDefaultAsync(lr => lr.Id == requestId);

            if (leaveRequest == null)
                throw new InvalidOperationException("Leave request not found");

            return await GetLeaveRequestDTOAsync(leaveRequest);
        }

        public async Task<bool> CancelLeaveRequestAsync(int requestId, int employeeId)
        {
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(lr => lr.Id == requestId && lr.EmployeeId == employeeId);

            if (leaveRequest == null)
                throw new InvalidOperationException("Leave request not found");

            if (leaveRequest.Status != LeaveStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be cancelled");

            leaveRequest.Status = LeaveStatus.Cancelled;
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<LeaveRequestDTO> GetLeaveRequestDTOAsync(LeaveRequest leaveRequest)
        {
            return new LeaveRequestDTO
            {
                Id = leaveRequest.Id,
                EmployeeName = leaveRequest.Employee.Name,
                ManagerName = leaveRequest.Manager?.Name,
                LeaveType = leaveRequest.LeaveType,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Reason = leaveRequest.Reason,
                Status = leaveRequest.Status,
                ManagerComments = leaveRequest.ManagerComments,
                RequestedAt = leaveRequest.RequestedAt,
                ProcessedAt = leaveRequest.ProcessedAt,
                TotalDays = leaveRequest.TotalDays
            };
        }

        private async Task UpdateLeaveBalanceAsync(int userId, LeaveType leaveType, int usedDays)
        {
            var leaveBalance = await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.UserId == userId && 
                                         lb.LeaveType == leaveType && 
                                         lb.Year == DateTime.Now.Year);

            if (leaveBalance != null)
            {
                leaveBalance.UsedDays += usedDays;
                leaveBalance.LastUpdated = DateTime.UtcNow;
            }
        }
    }
} 