# Coupon Management System

A high-concurrency Coupon Management System built with ASP.NET Core API, Entity Framework Core, and PostgreSQL. This system is designed to prevent overselling limited-inventory coupons during high-traffic events using database-level constraints and optimistic concurrency control.

## Concurrency and Architecture Strategy

To handle high-throughput "flash sale" scenarios where multiple network requests modify shared resources simultaneously, this system implements a multi-layered validation approach:

1. Optimistic Concurrency Control (OCC): The application relies on PostgreSQL system columns (`xmin` acting as a RowVersion). When a coupon count decrements, EF Core checks that the row state has not changed since it was read. If a collision occurs, a DbUpdateConcurrencyException is caught.
2. Bounded Retry Loop: Concurrency exceptions trigger a capped retry mechanism (up to 3 attempts) inside the ClaimService. It reloads the fresh entity database state, checks the inventory limits again, and retries the allocation.
3. Unique Constraints: A composite unique index on (CouponId, UserId) acts as the final line of defense to prevent duplicate claims by the same user, ensuring database integrity even under heavy asynchronous stress.

---

## Tech Stack

- Backend: .NET 8 / ASP.NET Core Web API
- ORM: Entity Framework Core (EF Core)
- Database: PostgreSQL
- Authentication: JWT (JSON Web Tokens)

---

## Repository Structure

```text
├── CouponManagementSystem/
│   ├── Controllers/          # API Handlers (Auth, Claims, Coupons)
│   ├── Data/                 # DbContext, Migrations, Entity Configurations
│   ├── Services/             # ClaimService, CouponService (Retry Logic)
│   ├── Models/               # Core domain entities (User, Coupon, Claim)
