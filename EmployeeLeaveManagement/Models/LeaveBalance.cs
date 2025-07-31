using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public class LeaveBalance
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        public LeaveType LeaveType { get; set; }
        
        public int TotalDays { get; set; }
        
        public int UsedDays { get; set; }
        
        public int RemainingDays => TotalDays - UsedDays;
        
        public int Year { get; set; } = DateTime.Now.Year;
        
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
} 