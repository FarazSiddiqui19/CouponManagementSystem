
# Coupon Management System

A high-concurrency Coupon Management System built with ASP.NET Core API, Entity Framework Core, and PostgreSQL. This system is designed to prevent overselling limited-inventory coupons during high-traffic events using database-level constraints and optimistic concurrency control.

---

## Tech Stack

* **Backend:** .NET 10 / ASP.NET Core Web API
* **ORM:** Entity Framework Core (EF Core)
* **Database:** PostgreSQL
* **Authentication:** JWT (JSON Web Tokens)

---

## Concurrency and Architecture Strategy

To handle high-throughput "flash sale" scenarios where multiple network requests modify shared resources simultaneously, this system implements a multi-layered validation approach:

* **Optimistic Concurrency Control (OCC):** The application relies on PostgreSQL system columns (`xmin` acting as a `RowVersion`). When a coupon count decrements, EF Core checks that the row state has not changed since it was read. If a collision occurs, a `DbUpdateConcurrencyException` is caught.
* **Bounded Retry Loop:** Concurrency exceptions trigger a capped retry mechanism (up to 3 attempts) inside the `ClaimService`. It reloads the fresh entity database state, checks the inventory limits again, and retries the allocation.
* **Unique Constraints:** A composite unique index on `(CouponId, UserId)` acts as the final line of defense to prevent duplicate claims by the same user, ensuring database integrity even under heavy asynchronous stress.

---

## Repository Structure

```text
├── CouponManagementSystem/
│   ├── Controllers/          # API Handlers (Auth, Claims, Coupons)
│   ├── Data/                 # DbContext, Migrations, Entity Configurations
│   ├── Services/             # ClaimService, CouponService (Retry Logic)
│   ├── Models/               # Core domain entities (User, Coupon, Claim)

```

---

## Getting Started

### 1. Install the Prerequisites

| Tool | Why you need it | Download link |
| --- | --- | --- |
| **.NET 10 SDK** | To build & run the API | [Download .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0) |
| **PostgreSQL** | The application database | [Download PostgreSQL](https://www.postgresql.org/download/) |

### 2. Set Up PostgreSQL

#### Option A: pgAdmin (GUI)

1. Open **pgAdmin**.
2. Create a new database named `CouponManagement`.
3. Note down your username and password (Default: `postgres` / `postgres`).

#### Option B: Command Line (psql)

Run the following commands in your terminal:

```bash
psql -U postgres
CREATE DATABASE "CouponManagement";
\q

```

### 3. Configure the Connection String

Open `CouponManagementSystem/appsettings.json` and update the connection details under `ConnectionStrings`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=CouponManagement;Username=postgres;Password=YOUR_PASSWORD"
}

```

> [!WARNING]
> Replace `postgres` and `YOUR_PASSWORD` with your actual PostgreSQL credentials. Do not commit sensitive passwords to production repositories.

### 4. Run the Project

Navigate to the project root directory and run the application:

```bash
cd CouponManagementSystem
dotnet run

```

#### What happens automatically on startup:

* **EF Core** applies all database migrations automatically (no manual `dotnet ef` commands needed).
* The API starts hosting on `http://localhost:5284`.
* Swagger UI becomes accessible at `http://localhost:5284/swagger`.

---

## Testing the API

The easiest way to test the endpoints is via **Swagger UI**:

1. Open `http://localhost:5284/swagger` in your browser.
2. Register a new account using `POST /api/auth/register`.
3. Log in with those credentials via `POST /api/auth/login` to receive a JWT bearer token.
4. Click the **Authorize** button at the top right of the Swagger page and paste your token.
5. Begin testing protected routes like `GET /api/coupons` or `POST /api/coupons`.

---

## Troubleshooting

### ❌ "Connection refused" on PostgreSQL

* Ensure the PostgreSQL service is actively running (check **Services** on Windows or `systemctl` / Activity Monitor on Linux/macOS).
* Double-check your credentials and port configuration inside `appsettings.json`.

### ❌ "dotnet is not recognized"

* The .NET 10 SDK might not be installed correctly or is missing from your system environment variables (`PATH`).
* Run `dotnet --version` in a fresh terminal instance to verify your installation.

### ❌ Port 5284 already in use

* Another process is occupying the port. You can change the hosting port in `Properties/launchSettings.json`, or terminate the process currently occupying port 5284.
