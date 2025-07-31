import React, { useState } from 'react';
import axios from 'axios';
import './ApplyLeavePage.css';

function ApplyLeavePage() {
  const [formData, setFormData] = useState({
    leaveType: 'Vacation',
    startDate: '',
    endDate: '',
    reason: ''
  });

  const [message, setMessage] = useState('');
  const token = localStorage.getItem('token');

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const { startDate, endDate } = formData;

    if (new Date(endDate) < new Date(startDate)) {
      setMessage('❌ End date cannot be before start date.');
      return;
    }

    if (new Date(startDate) < new Date().setHours(0, 0, 0, 0)) {
      setMessage('❌ Start date cannot be in the past.');
      return;
    }

    try {
      await axios.post(
        'https://localhost:7049/api/leave/apply',
        formData,
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      );
      setMessage('✅ Leave request submitted successfully!');
      setFormData({
        leaveType: 'Vacation',
        startDate: '',
        endDate: '',
        reason: ''
      });
    } catch (error) {
      const errorMsg = error.response?.data?.message || 'Something went wrong';
      setMessage(`❌ ${errorMsg}`);
    }
  };

  return (
    <div className="apply-leave-page">
      <div className="apply-leave-container">
        <h2 className="heading">Apply for Leave</h2>
        {message && <p className="message">{message}</p>}
        <form onSubmit={handleSubmit} className="leave-form">
          <div className="form-group">
            <label>Leave Type:</label>
            <select name="leaveType" value={formData.leaveType} onChange={handleChange}>
              <option value="Vacation">Vacation</option>
              <option value="SickLeave">Sick Leave</option>
              <option value="PersonalLeave">Personal Leave</option>
              <option value="MaternityLeave">Maternity Leave</option>
              <option value="PaternityLeave">Paternity Leave</option>
            </select>
          </div>

          <div className="form-group">
            <label>Start Date:</label>
            <input type="date" name="startDate" value={formData.startDate} onChange={handleChange} required />
          </div>

          <div className="form-group">
            <label>End Date:</label>
            <input type="date" name="endDate" value={formData.endDate} onChange={handleChange} required />
          </div>

          <div className="form-group">
            <label>Reason:</label>
            <textarea name="reason" value={formData.reason} onChange={handleChange} required />
          </div>

          <button type="submit" className="submit-btn">Submit Leave Request</button>
        </form>
      </div>
    </div>
  );
}

export default ApplyLeavePage;
