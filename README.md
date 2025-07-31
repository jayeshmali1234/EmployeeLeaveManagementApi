# Employee Leave Management System

A full-stack web application to manage employee leave requests, balances, and approvals, with a manager-employee workflow.

---
##  Features

- User Authentication (Employee & Manager roles)
- Leave Request Submission
- Manager Leave Approval / Rejection with comments
- Leave Balance Tracking
- Leave Calendar
- JWT-based token handling

---

##  Tech Stack

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

### In SQL server only thos users are login they are register only below user are register so only thos user are loggin
<img width="1366" height="768" alt="Sql" src="https://github.com/user-attachments/assets/01f3adec-1993-43a3-8440-e76739bd595c" />

##After this change we are go to main page of browser This Is Login Page Only Those User are loggin they are register

<img width="1366" height="768" alt="LoginPage" src="https://github.com/user-attachments/assets/50ef457e-63bb-4bef-9756-ad4a3ff1ce95" />

##Below user are register so only this user are login

<img width="1366" height="768" alt="LoginPerson" src="https://github.com/user-attachments/assets/3fb2fd0f-6318-45fa-b0b5-7a489b0be751" />

#Ater this we are show the dash Board Differnt types of css is add there

<img width="1366" height="768" alt="DashBoard" src="https://github.com/user-attachments/assets/91b8eb8a-49e3-41dd-8310-41ce729b17cc" />

##Apply for Leave You Can Choose

<img width="1366" height="768" alt="Apply Leave" src="https://github.com/user-attachments/assets/a0d6ad1d-5772-437e-923e-f5093c7175cf" />

##Leave Calender also Show you
<img width="1366" height="768" alt="Calender" src="https://github.com/user-attachments/assets/ecbf05d4-6078-40ea-94ca-5cc8156a7878" />
