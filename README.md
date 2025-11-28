# Customer Registration API

## Description
This is a .NET 8 Web API project for handling customer onboarding.  
It supports the following features:

- Register new users
- Migrate existing users
- Send and verify OTPs (mobile/email)
- Set PIN for secure login
- Accept terms and conditions
- Enable biometrics
- Check customer status

This project uses **Entity Framework Core** with SQL Server as the database and **Swagger** for testing the APIs.

---

## Technologies Used
- .NET 8 Web API
- Entity Framework Core
- SQL Server (LocalDB)
- Swagger / OpenAPI for testing
- BCrypt for PIN hashing

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server LocalDB (or any SQL Server instance)

---

### Setup Instructions

1. Clone the repository:
   ```bash
   git clone <your-repo-url>
   cd CustomerRegistration

2. Update the connection string in appsettings.json if needed:
   ```bash
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CustomerRegistrationDb;Trusted_Connection=True;"
    }


3. Restore dependencies and build the project:
   ```bash
    dotnet restore
    dotnet build


4. Apply migrations to create the database:
    ```bash
    dotnet ef database update


5. Run the API:
    ```bash
    dotnet run


The API will be available at: https://localhost:5041

Access Swagger UI to test the endpoints: https://localhost:5041/swagger

## API Flow

- Register - Create a new user.

- Migrate - For existing users who need to go through the onboarding flow again.

- Send OTP - Send OTP to mobile/email.

- Verify OTP - Verify OTP code.

- Set PIN - Set a secure PIN for login.

- Accept Terms - Accept terms and conditions.

- Enable Biometrics - Optional, requires PIN set first.

- Status - Check the current status of the customer.

## Testing

- Seeded customer for testing (ICNumber: 123456789012):

    - Name: Test User

    - Mobile: 0123456789

    - Email: test@example.com

Try registering the same ICNumber → should show “Customer already exists. Use migration API.”

Use /api/customer/migrate with ICNumber to reset flow and continue testing OTP, PIN, terms, and biometrics.