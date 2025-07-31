using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EmployeeLeaveManagement.DTOs;
using EmployeeLeaveManagement.Services;

namespace EmployeeLeaveManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("request")]
        public async Task<ActionResult<LeaveRequestDTO>> CreateLeaveRequest([FromBody] CreateLeaveRequestDTO request)
        {
            try
            {
                var employeeId = GetCurrentUserId();
                var result = await _leaveService.CreateLeaveRequestAsync(employeeId, request);
                return CreatedAtAction(nameof(GetLeaveRequest), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-requests")]
        public async Task<ActionResult<List<LeaveRequestDTO>>> GetMyLeaveRequests()
        {
            try
            {
                var employeeId = GetCurrentUserId();
                var requests = await _leaveService.GetEmployeeLeaveRequestsAsync(employeeId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<List<LeaveRequestDTO>>> GetPendingRequests()
        {
            try
            {
                var managerId = GetCurrentUserId();
                var requests = await _leaveService.GetPendingLeaveRequestsAsync(managerId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<LeaveRequestDTO>> ApproveLeaveRequest(int id, [FromBody] UpdateLeaveRequestDTO request)
        {
            try
            {
                var managerId = GetCurrentUserId();
                var result = await _leaveService.UpdateLeaveRequestAsync(id, managerId, request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("balance")]
        public async Task<ActionResult<List<LeaveBalanceDTO>>> GetMyLeaveBalance()
        {
            try
            {
                var employeeId = GetCurrentUserId();
                var balances = await _leaveService.GetEmployeeLeaveBalancesAsync(employeeId);
                return Ok(balances);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("calendar")]
        public async Task<ActionResult<List<LeaveCalendarDTO>>> GetLeaveCalendar(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            try
            {
                var calendar = await _leaveService.GetLeaveCalendarAsync(startDate, endDate);
                return Ok(calendar);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDTO>> GetLeaveRequest(int id)
        {
            try
            {
                var request = await _leaveService.GetLeaveRequestByIdAsync(id);
                return Ok(request);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}/cancel")]
        public async Task<ActionResult> CancelLeaveRequest(int id)
        {
            try
            {
                var employeeId = GetCurrentUserId();
                await _leaveService.CancelLeaveRequestAsync(id, employeeId);
                return Ok(new { message = "Leave request cancelled successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user token");
            }
            return userId;
        }
    }
} 