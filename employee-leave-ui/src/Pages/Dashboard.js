// src/Pages/Dashboard.js
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Dashboard.css';

const Dashboard = () => {
  const [leaveBalances, setLeaveBalances] = useState([]);
  const token = localStorage.getItem('token');

  useEffect(() => {
    axios
      .get('https://localhost:7049/api/leave/balance', {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => setLeaveBalances(res.data))
      .catch((err) => console.error('Error fetching leave balances', err));
  }, [token]);

  return (
    <div className="dashboard-container">
      <h2>Welcome to Your Dashboard</h2>

      <div className="balance-grid">
        {leaveBalances.map((balance, index) => (
          <div key={index} className="balance-card">
            <h3>{balance.leaveType}</h3>
            <p><strong>Remaining:</strong> {balance.remainingDays} days</p>
            <p><strong>Total:</strong> {balance.totalDays} days</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Dashboard;
