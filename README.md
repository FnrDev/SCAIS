# Smart Course Advising Information System (SCAIS)

## 📋 Project Overview

The Smart Course Advising Information System (SCAIS) is a Windows desktop application designed for Bahrain Institute of Technology (BIT) to streamline the academic advising process. The system assists faculty advisers in managing and guiding their advisees by automating course recommendations and validating course selections based on academic history, prerequisites, and corequisites.

This application ensures IT students enroll in the correct courses while adhering to prerequisite rules, providing insights for academic planning and tracking specialization progress across four tracks: Programming, Networking, Cybersecurity, and Database.

## 🎯 Key Features

### For Advisers
- View and manage assigned advisees
- Access student academic history and specialization details
- Recommend courses based on eligibility and completed prerequisites
- Approve or modify student course plans
- Generate comprehensive course progression reports

### For Students
- View personal academic records
- Access list of eligible courses for upcoming semester
- Submit preferred courses for adviser review
- View approved courses and adviser feedback

### For Administrators
- Manage user accounts (advisers and students)
- Maintain course catalog with prerequisites and corequisites
- Assign advisees to advisers
- Define course offerings per semester
- Manage specialization structures and curriculum flowcharts

## 🛠️ Technology Stack

### Frontend
- **Language:** C# (.NET Framework 4.8)
- **UI Framework:** Windows Forms
- **Design Tool:** Sparx Enterprise Architect

### Backend
- **Database:** MySQL Server
- **ORM/Data Access:** ADO.NET / Entity Framework

### Architecture
- **Pattern:** Layered Architecture
  - Presentation Layer (UI)
  - Business Logic Layer
  - Data Access Layer

## 📊 Database Schema

The system includes the following key entities:

- **Users** - Base user information
- **Students** - Student-specific details and advisee information
- **Advisers** - Faculty adviser information
- **Courses** - Course catalog and details
- **Enrollments** - Student course enrollment records
- **Specializations** - Four IT tracks (Programming, Networking, Cybersecurity, Database)
- **Prerequisites** - Course dependency rules
- **Semesters** - Academic term information

## 🎓 Specializations

The system supports four IT specializations:

1. **Programming** - Focus on software development and OOP concepts
2. **Networking** - Emphasis on network infrastructure and communication
3. **Cybersecurity** - Specialization in security and defense mechanisms
4. **Database** - Concentration on database design and administration

### Course Types
- **Core Subjects** - Common courses for all students
- **Specialized Subjects** - Track-specific courses
- **Prerequisites** - Courses required before advanced topics

## ✨ Core Functionality

### Course Validation Engine
- Automatic prerequisite checking
- Corequisite validation
- Specialization-based course eligibility
- Academic progress tracking

### Role-Based Access Control
- Single unified login screen
- Role-based dashboard redirection
- Secure authentication system

### Course Status Tracking
- **Completed** - Successfully finished courses
- **In Progress** - Currently enrolled courses
- **Pending** - Eligible but not yet taken courses

## 📐 UML Diagrams

The project includes comprehensive UML documentation:

- **Use Case Diagram** - System functionality overview
- **Class Diagram** - Complete class structure with inheritance and associations
- **Activity Diagram** - Workflow and business process flows
- **Sequence Diagram** - Object interactions and message flows
- **ERD** - Database schema in 3rd Normal Form

## 🚀 Getting Started

### Prerequisites
- Windows OS (Windows 10 or later)
- .NET Framework 4.8
- MySQL Server 8.0+
- Visual Studio 2019 or later
- Sparx Enterprise Architect (for design documentation)

## 👥 User Roles

| Role | Access Level | Primary Functions |
|------|--------------|-------------------|
| Administrator | Full System Access | User management, catalog maintenance, system configuration |
| Adviser | Department Level | Advisee management, course approval, report generation |
| Student | Personal Access | View records, submit course requests, track progress |

## 🔒 Security Features

- Role-based authentication and authorization
- Secure password storage
- Input validation to prevent SQL injection
- Session management
- Data access restrictions based on user role

## 📄 License

This project is developed as part of the IT7006 Object Oriented Design course at Bahrain Polytechnic.
---

**Course:** IT7006 - Object Oriented Design  
**Institution:** Bahrain Polytechnic  
**Academic Year:** 2025
