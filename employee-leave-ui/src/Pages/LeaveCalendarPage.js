// src/Pages/LeaveCalendarPage.js
import React, { useState, useEffect } from 'react';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import axios from 'axios';
import './LeaveCalendarPage.css';

const LeaveCalendarPage = () => {
  const [leaves, setLeaves] = useState([]);
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [loading, setLoading] = useState(false);
  const token = localStorage.getItem('token');

  useEffect(() => {
    fetchLeaves();
  }, []);

  const fetchLeaves = async () => {
    setLoading(true);
    try {
      const response = await axios.get(`https://localhost:7049/api/leave/calendar?startDate=2025-01-01&endDate=2025-12-31`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      setLeaves(response.data);
    } catch (err) {
      console.error('Error fetching calendar data', err);
    } finally {
      setLoading(false);
    }
  };

  const isOnLeave = (date) => {
    return leaves.some(leave =>
      new Date(leave.startDate) <= date && date <= new Date(leave.endDate)
    );
  };

  return (
    <div className="calendar-container">
      <h2 className="calendar-title">Leave Calendar</h2>
      {loading ? (
        <p>Loading calendar data...</p>
      ) : (
        <Calendar
          value={selectedDate}
          onChange={setSelectedDate}
          tileClassName={({ date }) => isOnLeave(date) ? 'leave-date' : null}
        />
      )}
    </div>
  );
};

export default LeaveCalendarPage;
