// src/Pages/ManagerApprovalPage.js
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './ManagerApprovalPage.css'; 

const ManagerApprovalPage = () => {
  const [requests, setRequests] = useState([]);
  const [message, setMessage] = useState('');
  const token = localStorage.getItem('token');

  const fetchRequests = async () => {
    try {
      const res = await axios.get('https://localhost:7049/api/leave/pending', {
        headers: { Authorization: `Bearer ${token}` }
      });
      setRequests(res.data);
    } catch (err) {
      setMessage(' Failed to fetch requests.');
    }
  };

  const handleAction = async (id, approve) => {
    try {
      await axios.post(
        `https://localhost:7049/api/leave/${approve ? 'approve' : 'reject'}/${id}`,
        {},
        {
          headers: { Authorization: `Bearer ${token}` }
        }
      );
      setMessage(`✅ Request ${approve ? 'approved' : 'rejected'} successfully!`);
      fetchRequests(); // Refresh list
    } catch (err) {
      setMessage(`❌ Failed to ${approve ? 'approve' : 'reject'} request.`);
    }
  };

  useEffect(() => {
    fetchRequests();
  }, []);

  return (
    <div className="manager-approval-container">
      <h2 className="heading">Manager Approvals</h2>
      {message && <p className="message">{message}</p>}

      {requests.length === 0 ? (
        <p>No pending leave requests.</p>
      ) : (
        <table className="approval-table">
          <thead>
            <tr>
              <th>Employee</th>
              <th>Leave Type</th>
              <th>Dates</th>
              <th>Reason</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {requests.map((req) => (
              <tr key={req.id}>
                <td>{req.employeeName}</td>
                <td>{req.leaveType}</td>
                <td>{req.startDate} to {req.endDate}</td>
                <td>{req.reason}</td>
                <td>
                  <button className="btn approve" onClick={() => handleAction(req.id, true)}>Approve</button>
                  <button className="btn reject" onClick={() => handleAction(req.id, false)}>Reject</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ManagerApprovalPage;
