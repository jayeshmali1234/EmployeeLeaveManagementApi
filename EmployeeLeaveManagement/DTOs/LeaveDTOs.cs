using System.ComponentModel.DataAnnotations;
using EmployeeLeaveManagement.Models;

namespace EmployeeLeaveManagement.DTOs
{
    public class CreateLeaveRequestDTO
    {
        [Required]
        public LeaveType LeaveType { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
    }

    public class LeaveRequestDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerName { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }
        public string ManagerComments { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int TotalDays { get; set; }
    }

    public class UpdateLeaveRequestDTO
    {
        public LeaveStatus Status { get; set; }
        public string ManagerComments { get; set; }
    }

    public class LeaveBalanceDTO
    {
        public LeaveType LeaveType { get; set; }
        public int TotalDays { get; set; }
        public int UsedDays { get; set; }
        public int RemainingDays { get; set; }
        public int Year { get; set; }
    }

    public class LeaveCalendarDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; }
    }
} 