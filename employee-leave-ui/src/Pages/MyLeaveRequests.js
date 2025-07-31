// src/Pages/MyLeaveRequests.js
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './MyLeaveRequests.css';

const MyLeaveRequests = () => {
  const [leaveRequests, setLeaveRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const token = localStorage.getItem('token');

  useEffect(() => {
    axios.get('https://localhost:7049/api/leave/my-requests', {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
      .then(res => setLeaveRequests(res.data))
      .catch(() => setError('Failed to fetch leave requests.'))
      .finally(() => setLoading(false));
  }, [token]);

  return (
    <div className="leave-requests-container">
      <h2 className="title">My Leave Requests</h2>

      {loading && <p>Loading...</p>}
      {error && <p className="error">{error}</p>}

      {leaveRequests.length === 0 ? (
        <p>No leave requests found.</p>
      ) : (
        <table className="leave-table">
          <thead>
            <tr>
              <th>Leave Type</th>
              <th>Start Date</th>
              <th>End Date</th>
              <th>Status</th>
              <th>Manager Comments</th>
            </tr>
          </thead>
          <tbody>
            {leaveRequests.map((req, index) => (
              <tr key={index}>
                <td>{req.leaveType}</td>
                <td>{req.startDate?.substring(0, 10)}</td>
                <td>{req.endDate?.substring(0, 10)}</td>
                <td className={`status ${req.status.toLowerCase()}`}>{req.status}</td>
                <td>{req.managerComment || '—'}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default MyLeaveRequests;
