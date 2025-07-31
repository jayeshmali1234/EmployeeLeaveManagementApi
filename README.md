# 🏢 Employee Leave Management System

A full-stack web application to manage employee leave requests, balances, and approvals, with a manager-employee workflow.

---
## ✅ Features

- User Authentication (Employee & Manager roles)
- Leave Request Submission
- Manager Leave Approval / Rejection with comments
- Leave Balance Tracking
- Leave Calendar
- JWT-based token handling

---

## 💻 Tech Stack

### 🔧 Backend (ASP.NET Core Web API)
- ASP.NET Core 6+
- Entity Framework Core (Code-First)
- SQL Server
- JWT Authentication
- Role-based Authorization
- RESTful APIs

### 🖥️ Frontend (React)
- React.js (with Hooks)
- Axios (for API requests)
- React Router
- Tailwind CSS
- Protected Routes with Role Checks

---

## ☁️ Server & Database

| Component     | Stack                  |
|---------------|------------------------|
| **API Server** | ASP.NET Core Web API  |
| **Frontend Dev Server** | Vite / React Scripts |
| **Database**   | Microsoft SQL Server (LocalDB or Azure SQL)

---

## ⚙️ How to Run Locally

### 🛠 Backend Setup

1. Open the solution in **Visual Studio** or use CLI.
2. Update your DB connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EmployeeLeaveManagement;Trusted_Connection=True;"
   }

######Complete Test to files in Swagger 

<img width="1366" height="768" alt="Swagger" src="https://github.com/user-attachments/assets/4b219b5d-b318-46c7-bfc9-8b5420414597" />

##then after Postman Succesfully Complete Leave Request

<img width="1366" height="768" alt="Postman" src="https://github.com/user-attachments/assets/548f189f-aec4-4611-b9cb-965daf55a529" />

### In SQL 
