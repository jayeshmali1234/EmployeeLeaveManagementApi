import React from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import './Navbar.css'; // Create this CSS file

const Navbar = () => {
  const role = localStorage.getItem('role');
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = () => {
    localStorage.clear();
    navigate('/');
  };

  return (
    <nav className="navbar">
      <div className="navbar-links">
        <Link className={location.pathname === "/dashboard" ? "active" : ""} to="/dashboard">Dashboard</Link>
        <Link className={location.pathname === "/apply-leave" ? "active" : ""} to="/apply-leave">Apply Leave</Link>
        <Link className={location.pathname === "/my-leaves" ? "active" : ""} to="/my-leaves">My Requests</Link>
        <Link className={location.pathname === "/leave-calendar" ? "active" : ""} to="/leave-calendar">Leave Calendar</Link>
        {role === 'Manager' && (
          <Link className={location.pathname === "/manager-approvals" ? "active" : ""} to="/manager-approvals">Manager Approvals</Link>
        )}
      </div>
      <button className="logout-button" onClick={handleLogout}>Logout</button>
    </nav>
  );
};

export default Navbar;
