# Project & Financial Management System

A role-based web application developed for the internal operations of a contracting firm. The system helps manage projects, employees, financial accounts, expenses, fund transfers, employee expenses, pending bills, approvals, and financial reports.

Sensitive business data and production credentials have been removed from this portfolio version.

## Features

### User & Access Management

- Role-based access for Admin, Accountant, and Employee
- Secure login system
- Session-based user identification
- Password change functionality
- Admin can reset employee passwords

### Project Management

- Add, edit, and delete project information
- Store project name, location, and status
- Track running and completed projects

### Employee Management

- Add, edit, and delete employee information
- Track employee-related financial transactions
- Employee-specific expense management

### Account Management

- Manage multiple financial accounts
- Deposit money into accounts
- Transfer money between accounts
- Track account transactions

### Voucher & Financial Transactions

The system supports four types of financial transactions:

- **প্রদান** — Allocate money from an account to an employee
- **খরচ** — Record expenses made by an Admin or Accountant
- **জমা** — Deposit money into an account
- **ট্রান্সফার** — Transfer money between accounts

The logged-in user's identity is automatically recorded with each transaction.

### Employee Expense Management

Employees can:

- Submit project-related expenses
- Select project and expense category
- Provide expense description and amount
- Return unused money to an account

Employee returned amounts follow an approval workflow handled by Admin or Accountant.

### Reports

The system provides:

- Account Reports
- Employee Reports
- Project Reports

Reports support multiple filters, including:

- Date
- Expense category
- Employee
- Project

Filtered reports can also be downloaded as PDF files.

### Pending Bills

Admin and Accountant users can save information about pending bills for future reference and tracking.

## Technologies Used

- C#
- ASP.NET Web Forms
- Microsoft SQL Server
- HTML
- CSS
- JavaScript

## Screenshots

### Admin Panel

The following screenshots demonstrate the main features and interfaces available to Admin users.

![Admin Screenshot 1](screenshots/1.png)
![Admin Screenshot 2](screenshots/2.png)
![Admin Screenshot 3](screenshots/3.png)
![Admin Screenshot 4](screenshots/4.png)
![Admin Screenshot 5](screenshots/5.png)
![Admin Screenshot 6](screenshots/6.png)
![Admin Screenshot 7](screenshots/7.png)
![Admin Screenshot 8](screenshots/8.png)
![Admin Screenshot 9](screenshots/9.png)
![Admin Screenshot 10](screenshots/10.png)
![Admin Screenshot 11](screenshots/11.png)
![Admin Screenshot 12](screenshots/12.png)
![Admin Screenshot 13](screenshots/13.png)
![Admin Screenshot 14](screenshots/14.png)
![Admin Screenshot 15](screenshots/15.png)

### Employee Panel

The following screenshots demonstrate the interfaces and functionality available to Employee users.

![Employee Screenshot 16](screenshots/16.png)
![Employee Screenshot 17](screenshots/17.png)
![Employee Screenshot 18](screenshots/18.png)
![Employee Screenshot 19](screenshots/19.png)

## Deployment & Maintenance

The application was deployed to a live hosting environment and is used for internal business operations.

I also provide ongoing technical support and maintenance, including troubleshooting hosting-related issues and deploying required updates.

## My Role

- Designed and developed the application
- Implemented the database and business logic
- Developed role-based functionality
- Implemented financial transaction workflows
- Developed filtering and reporting features
- Implemented PDF report generation
- Deployed the application to a live hosting environment
- Continue to provide maintenance and technical support

## Note

This is a portfolio presentation of a real-world business application. Production credentials, confidential configuration, and real business data have been excluded from this repository.

## Author

**Md. Zunead Rahman**

GitHub: [itszunaed](https://github.com/itszunaed)
