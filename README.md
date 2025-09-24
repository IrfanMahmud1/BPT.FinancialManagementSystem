# 📘 Financial Management System (FMS)

## 1. Project Overview
Financial Management System (FMS) is a simplified system designed to manage **Chart of Accounts (COA)**, **Journal Entries**, and **Vouchers**. It provides a **secure, modular, and extensible architecture** with support for reporting and API integration.

### Key Features
- ✅ CRUD operations for Chart of Accounts (COA), Journal Entries, and Vouchers  
- ✅ JWT-based authentication for secure API access  
- ✅ Onion/Clean Architecture with CQRS  
- ✅ Repository + Unit of Work Pattern for persistence  
- ✅ Pagination using jQuery DataTable in the UI  
- ✅ Reporting using jQuery DataTable export, RDLC, or Crystal Reports  
- ✅ SQL Server 2019/2022 for data persistence  

---

## 2. Architecture Explanation
The project follows **Onion/Clean Architecture** with **CQRS** and **Unit of Work + Repository Pattern**.

### Layers
**Core Layer**
- Domain models: `ChartOfAccount`, `JournalEntry`, `Voucher`, `User`  
- Business rules and validation logic  

**Application Layer**
- CQRS Handlers: `CommandHandlers`, `QueryHandlers`  
- DTOs and service contracts  
- Interfaces for repositories  

**Infrastructure Layer**
- EF Core `DbContext`  
- Repository + Unit of Work implementations  
- SQL Server integration (Stored Procedures optional)  
- JWT authentication utilities  

**API Project**
- ASP.NET Web API controllers  
- Handles JWT authentication  
- Exposes endpoints for COA, Journal, and Voucher  

**UI Project**
- ASP.NET MVC with Razor Views  
- Uses jQuery DataTable for listing and pagination  
- Calls API endpoints using JWT  

### Architecture Diagram
[UI Project (ASP.NET MVC, Razor Views)]
|
(sends requests via JWT-authenticated HTTP)
|
[API Project]
|
-------------------------
| Application |
| (CQRS Handlers, |
| DTOs, Services) |
-------------------------
|
-------------------------
| Infrastructure |
| (EF Core, UoW, Repo, |
| JWT, SQL Server) |
-------------------------
|
-------------------------
| Core |
| (Domain Models, |
| Business Rules) |
-------------------------


---

## 3. Unit of Work with Repository Pattern
```csharp
public interface IApplicationUnitOfWork : IUnitOfWork
{
    IChartOfAccountRepository ChartOfAccounts { get; }
    IVoucherRepository Vouchers { get; }
    IUserRepository Users { get; }
}

4. Setup Instructions
Prerequisites

Visual Studio 2022 (with .NET Framework 4.8 support)

SQL Server 2019/2022

Postman (for API testing)
git clone <repo-url>
cd FinancialManagementSystem

5. API Documentation
🔐 Authentication

POST /api/auth/login

Request
{
  "username": "admin",
  "password": "password123"
}
{
  "token": "<jwt-token>"
}
Authorization: Bearer <jwt-token>
📂 Users

POST /api/User

Example
{
  "email": "admin@example.com",
  "password": "password123",
  "userName": "Admin",
  "accessLevel": "Admin",
  "createdDate": "2025-09-24T10:30:00Z",
  "createdBy": null,
  "isActive": true
}

📂 Chart of Accounts (COA)

GET /api/ChartOfAccount/all/{childId}
→ Get all parent accounts excluding the account with the given ID

GET /api/ChartOfAccount/paginated
→ Get paginated COA list

GET /api/ChartOfAccount/{id}
→ Get a single COA by ID

POST /api/ChartOfAccount
{
  "accountName": "Cash",
  "parentId": null,
  "accountType": "Asset",
  "isActive": true,
  "createdAt": "2025-09-24T10:30:00Z"
}
PUT /api/ChartOfAccount/{id}
→ Update an existing COA

DELETE /api/ChartOfAccount/{id}
→ Delete a COA

🧾 Voucher Entries

GET /api/Voucher/paginated/{journalId}
→ Get all journal entry lines for a specific Journal

GET /api/Voucher/paginated
→ Get paginated list of Journals

GET /api/Voucher/{id}
→ Get a single Journal by ID

POST /api/Voucher

{
  "type": "Payment",
  "date": "2025-09-24",
  "referenceNo": "REF-1001",
  "entries": [
    {
      "chartOfAccountId": "11111111-1111-1111-1111-111111111111",
      "debit": 500.00,
      "credit": 0.00
    },
    {
      "chartOfAccountId": "22222222-2222-2222-2222-222222222222",
      "debit": 0.00,
      "credit": 500.00
    }
  ]
}
DELETE /api/Voucher/{id}
→ Delete a Voucher by ID

6. Pagination Implementation

API supports query parameters:
/api/ChartOfAccount/paginated?page=1&pageSize=10
/api/Journal/paginated?page=1&pageSize=10
/api/Voucher/paginated?page=1&pageSize=10


UI uses jQuery DataTable for:

Pagination

Sorting

Filtering

7. Reporting Features

Interactive Reports via jQuery DataTable export (Excel, PDF, CSV, Print)

Advanced Reports (optional): RDLC or Crystal Reports

Reports support filtering by:

Date ranges

Account type

Transaction type

8. Database Schema
1. ChartOfAccounts
Column	Type	Notes
Id	uniqueidentifier (PK)	Primary Key
AccountName	nvarchar	Name of the account
ParentId	uniqueidentifier (FK)	Self-referencing for parent-child hierarchy
AccountType	nvarchar	Asset, Liability, Income, etc.
IsActive	bit	Whether the account is active
CreatedAt	datetime	When the account was created
2. Users
Column	Type	Notes
Id	uniqueidentifier (PK)	Primary Key
Email	nvarchar	User’s email
Password	nvarchar	Plain password (⚠ should be hashed in production)
UserName	nvarchar	Display/user name
AccessLevel	nvarchar	Role or permission level
CreatedDate	datetime	Account creation date
CreatedBy	uniqueidentifier (FK)	Refers to another User
IsActive	bit	Whether the user is active
3. Vouchers
Column	Type	Notes
Id	uniqueidentifier (PK)	Primary Key
Date	datetime	Date of the voucher
ReferenceNo	nvarchar	Reference number
Type	nvarchar	Type of voucher (e.g., Payment, Receipt, Journal)
4. VoucherEntries
Column	Type	Notes
Id	uniqueidentifier (PK)	Primary Key
VoucherId	uniqueidentifier (FK)	References Vouchers.Id
ChartOfAccountId	uniqueidentifier (FK)	References ChartOfAccounts.Id
Debit	decimal	Debit amount
Credit	decimal	Credit amount
