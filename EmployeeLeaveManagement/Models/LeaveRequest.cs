using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public enum LeaveType { Vacation, SickLeave, PersonalLeave, MaternityLeave, PaternityLeave }
    public enum LeaveStatus { Pending, Approved, Rejected, Cancelled }

    public class LeaveRequest
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }
        public User Employee { get; set; }
        
        public int? ManagerId { get; set; }
        public User Manager { get; set; }
        
        [Required]
        public LeaveType LeaveType { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
        
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        
        [StringLength(500)]
        public string ManagerComments { get; set; }
        
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ProcessedAt { get; set; }
        
        public int TotalDays => (EndDate - StartDate).Days + 1;
    }
} 