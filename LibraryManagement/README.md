# Library Management – Backend (ASP.NET Core + EF Core + Clean Architecture)

This is the **backend** for the Library Management System. It provides:

- **Authentication & Authorization** (JWT-based)
- **Book & Category CRUD** for Admins  
- **Borrowing Flow** for Users  
- **Approval Flow** for Admins  
- Swagger API docs for testing

---

## How to Run It Locally

### 1. Clone the repo

```bash
git clone https://github.com/minh18ndh/r2e-library-management.git
cd r2e-library-management/LibraryManagement
```

### 2. Set up SQL Server

Make sure you have a local SQL Server instance running.  
Update your connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Apply migrations & seed data

```bash
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run
```

Then open:  
`https://localhost:5159/swagger/index.html` to explore the API.

---

## Auth Overview

- Uses **JWT** tokens
- Includes role-based access:
  - `User`: Can view books, request borrow
  - `Admin`: Can manage books/categories and approve/reject requests
- Roles are encoded in the JWT claims
- Login via:
  ```http
  POST /api/auth/login
  ```

---

## API Features

### Books & Categories (Admin Only)
- `GET /api/books`
- `POST /api/books`
- `PUT /api/books/{id}`
- `DELETE /api/books/{id}`

Supports search, sort, and filter via query params.

### Borrowing Requests
#### For Normal Users
- `GET /api/my-borrow-requests`
- `GET /api/my-borrow-requests/{id}`
- `POST /api/my-borrow-requests`

#### For Admins
- `GET /api/borrow-requests`
- `GET /api/borrow-requests/{id}`
- `PUT /api/borrow-requests/{id}/status` (Approve/Reject)

---

## Test Accounts

You can:
- Register as a user via `/api/auth/register`
- Login as admin:
  - Email: `admin@1`
  - Password: `string`

---

## Notes

- Global exception handler returns structured JSON errors
- Uses manual mapping with `ToEntity` / `ToResponseDto` extension methods (no AutoMapper)
- Book quantities are updated **immediately** on borrow, and **restored** if rejected
- Borrow requests limited to max **5 distinct books**
- Borrow requests limited to max **3 requests per month**
