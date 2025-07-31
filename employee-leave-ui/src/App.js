import React from 'react';
import { BrowserRouter as Router, Route, Routes, useLocation } from 'react-router-dom';
import LoginPage from './Pages/LoginPage';
import Dashboard from './Pages/Dashboard';
import ApplyLeavePage from './Pages/ApplyLeavePage';
import MyLeaveRequests from './Pages/MyLeaveRequests';
import ManagerApprovalPage from './Pages/ManagerApprovalPage';
import LeaveCalendarPage from './Pages/LeaveCalendarPage';
import PrivateRoute from './components/PrivateRoute';
import Navbar from './components/Navbar';

const Layout = () => {
  const location = useLocation();
  const hideNavbar = location.pathname === '/';

  return (
    <>
      {!hideNavbar && <Navbar />}

      <Routes>
        <Route path="/" element={<LoginPage />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/apply-leave" element={<ApplyLeavePage />} />
        <Route path="/my-leaves" element={<MyLeaveRequests />} />
        <Route path="/manager-approvals" element={<ManagerApprovalPage />} />
        <Route path="/leave-calendar" element={
          <PrivateRoute>
            <LeaveCalendarPage />
          </PrivateRoute>
        } />
      </Routes>
    </>
  );
};

function App() {
  return (
    <Router>
      <Layout />
    </Router>
  );
}

export default App;
