<div align="center">

# 🎂 Mekky's Cakes API

**A production-grade, bilingual e-commerce REST API for a custom cake ordering platform**

Built with .NET 8 · Clean Architecture · CQRS · Redis · JWT

[![Live Demo](https://img.shields.io/badge/🌐_Live_Demo-mekkys--cakes.vercel.app-blue?style=for-the-badge)](https://mekkys-cakes.vercel.app/)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF_Core-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)](https://redis.io/)

[Live Demo](https://mekkys-cakes.vercel.app/) · [API Reference](#-api-endpoints) · [Getting Started](#-getting-started) · [Architecture](#-architecture)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Live Demo](#-live-demo)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Design Patterns](#-design-patterns)
- [Project Structure](#-project-structure)
- [Features](#-features)
- [API Endpoints](#-api-endpoints)
- [Authentication & Authorization](#-authentication--authorization)
- [Localization](#-localization)
- [Caching Strategy](#-caching-strategy)
- [Testing](#-testing)
- [Getting Started](#-getting-started)
- [Database Schema](#-database-schema)

---

## 🔍 Overview

**Mekky's Cakes** is a full-featured e-commerce backend API for a custom cake ordering platform. It provides a complete set of RESTful endpoints for browsing products, managing shopping baskets, placing orders, submitting reviews, and maintaining wishlists — all with a fully separated admin dashboard API for business management.

The API is built with a focus on **enterprise-grade architecture**, **maintainability**, and **scalability**, featuring bilingual support (English & Arabic), Redis-backed caching, and JWT-secured role-based access control.

---

## 🌐 Live Demo

> **🔗 [https://mekkys-cakes.vercel.app/](https://mekkys-cakes.vercel.app/)**

The frontend is deployed on **Vercel** and consumes this API, which is hosted on **runasp.net** with **SQL Server** and **Upstash Redis** (cloud).

---

## 🏗 Architecture

The solution follows **Clean/Onion Architecture** principles with strict dependency inversion, organized into clearly separated layers:

```
┌──────────────────────────────────────────────────────────┐
│                   Presentation Layer                      │
│  ┌─────────────────────┐  ┌────────────────────────────┐ │
│  │   MekkysCakes.Web   │  │  MekkysCakes.Presentation  │ │
│  │  (Composition Root) │  │     (API Controllers)      │ │
│  └─────────┬───────────┘  └──────────┬─────────────────┘ │
├────────────┼─────────────────────────┼───────────────────┤
│            │      Application Core Layer                  │
│  ┌─────────▼───────────────────────  ▼─────────────────┐ │
│  │            MekkysCakes.Application                   │ │
│  │         (CQRS Handlers, Validators, DTOs)            │ │
│  ├──────────────────────────────────────────────────────┤ │
│  │  MekkysCakes.Services  │  MekkysCakes.Services.Abs.  │ │
│  │   (JWT, Identity,      │    (Service Interfaces)     │ │
│  │    Caching impl.)      │                             │ │
│  ├────────────────────────┴─────────────────────────────┤ │
│  │              MekkysCakes.Domain                      │ │
│  │     (Entities, Enums, Repository Contracts)          │ │
│  └──────────────────────────────────────────────────────┘ │
├──────────────────────────────────────────────────────────┤
│                  Infrastructure Layer                     │
│  ┌──────────────────────────────────────────────────────┐ │
│  │            MekkysCakes.Persistence                   │ │
│  │    (EF Core DbContext, Repos, Redis, Migrations)     │ │
│  └──────────────────────────────────────────────────────┘ │
├──────────────────────────────────────────────────────────┤
│  MekkysCakes.Shared  │  Shared DTOs, Result pattern,     │
│                      │  Pagination, Localization types    │
├──────────────────────┴───────────────────────────────────┤
│                     Tests Layer                           │
│  ┌──────────────────────────────────────────────────────┐ │
│  │              MekkysCakes.Tests                       │ │
│  │        (xUnit, Moq, FluentAssertions)                │ │
│  └──────────────────────────────────────────────────────┘ │
└──────────────────────────────────────────────────────────┘
```

---

## 🛠 Tech Stack

| Category | Technologies |
|:---|:---|
| **Runtime** | .NET 8, C# 12 |
| **ORM** | Entity Framework Core 9 (Code-First, Fluent API) |
| **Database** | SQL Server |
| **Caching & Baskets** | Redis (StackExchange.Redis), Upstash Redis (Cloud) |
| **Authentication** | JWT Bearer (HMAC-SHA256), ASP.NET Core Identity |
| **CQRS / Mediator** | MediatR 12.4 |
| **Validation** | FluentValidation 12 |
| **Object Mapping** | AutoMapper 14 |
| **API Documentation** | Swagger / OpenAPI, Scalar |
| **Testing** | xUnit, Moq, FluentAssertions |
| **Hosting** | runasp.net (API), Vercel (Frontend) |

---

## 🎨 Design Patterns

| Pattern | Implementation |
|:---|:---|
| **Clean / Onion Architecture** | Domain-centric layers with dependency inversion across 8 projects |
| **CQRS** | Separate Command and Query handlers via MediatR |
| **Repository Pattern** | Generic `IGenericRepository<TEntity, TKey>` with type-safe data access |
| **Unit of Work** | `IUnitOfWork` with lazy repository factory and transactional SaveChanges |
| **Specification Pattern** | `ISpecification` → `BaseSpecification` → `SpecificationsEvaluator` for dynamic query composition |
| **Result / Error Monad** | Custom `Result<T>` replacing exception-driven flow control with typed error categories |
| **MediatR Pipeline Behaviors** | `ValidationBehavior` (auto-validation) and `LoggingBehavior` (execution timing) as cross-cutting concerns |
| **Action Filter (Caching)** | Custom `RedisCacheAttribute` for declarative response caching |
| **Service Layer Abstraction** | Interfaces in `Services.Abstraction`, implementations in `Services` |
| **Translation Entity Pattern** | Per-entity translation tables for bilingual localization (EN/AR) |
| **Value Objects** | `ProductItemOrdered` as EF Core owned entity type |
| **Data Seeding** | `IDataInitializer` with keyed DI for store vs. identity data |

---

## 📁 Project Structure

```
MekkysCakesSolution/
├── MekkysCakes.Domain/              # Entities, Enums, Repository contracts
│   ├── Entities/                    # Product, Order, Review, Wishlist, Basket, Identity
│   └── Contracts/                   # IGenericRepository, IUnitOfWork, ISpecification
│
├── MekkysCakes.Application/         # CQRS Features, Validators, Mapping
│   ├── Features/                    # Commands & Queries per module
│   │   ├── Authentication/          # Login, Register, CheckEmail, GetCurrentUser
│   │   ├── Products/                # Product CRUD, Badges, Types, Themes
│   │   ├── Baskets/                 # Create/Update/Delete/Get basket
│   │   ├── Orders/                  # Create/Cancel order, Delivery methods, Status mgmt
│   │   ├── Reviews/                 # CRUD, Approve/Disapprove, Summary
│   │   └── Wishlists/               # Add/Remove items, Get wishlist
│   ├── Behaviors/                   # ValidationBehavior, LoggingBehavior
│   ├── MappingProfiles/             # AutoMapper profiles & resolvers
│   └── Specifications/              # Product, Order, Review, Wishlist specs
│
├── MekkysCakes.Services/            # Service implementations
│   ├── TokenService                 # JWT token generation
│   ├── IdentityService              # ASP.NET Identity wrapper
│   ├── CurrentUserService           # Claims extraction from HttpContext
│   └── CacheService                 # Redis cache operations
│
├── MekkysCakes.Services.Abstraction/ # Service interfaces
│
├── MekkysCakes.Persistence/         # Data access infrastructure
│   ├── Data/
│   │   ├── DbContexts/             # StoreDbContext (IdentityDbContext)
│   │   ├── Configurations/          # 14 Fluent API entity configurations
│   │   └── Migrations/             # 10 EF Core migrations
│   ├── Repositories/                # GenericRepository, UnitOfWork
│   ├── BasketRepository             # Redis-backed basket storage
│   └── CacheRepository              # Redis string cache
│
├── MekkysCakes.Presentation/        # API Controllers
│   ├── Controllers/
│   │   ├── Public/                  # Auth, Products, Baskets, Orders, Reviews, Wishlists
│   │   └── Admin/                   # Product mgmt, Order mgmt, Review moderation, Delivery
│   └── Attributes/                  # RedisCacheAttribute
│
├── MekkysCakes.Web/                 # Composition root & startup
│   ├── Program.cs                   # DI registration, middleware pipeline, data seeding
│   └── appsettings.json             # Connection strings, JWT config
│
├── MekkysCakes.Shared/              # Cross-cutting shared types
│   ├── CommonResult/                # Result<T>, Error, ErrorType
│   ├── Pagination/                  # PaginatedQueryParams, PaginatedResult<T>
│   └── DTOs/                        # LocalizedString, ProductDTO, OrderDTO, etc.
│
└── MekkysCakes.Tests/               # Unit tests
    ├── Domain/                      # Entity logic tests
    └── Application/                 # Handler & validator tests
```

---

## ✨ Features

### 🛒 Customer Features
- **Product Catalog** — Browse products with pagination, filtering by type/theme, and search
- **Shopping Basket** — Redis-backed basket with 24h TTL, supporting add/update/remove items
- **Order Management** — Place orders with delivery method selection, view order history, cancel pending orders
- **Product Reviews** — Submit, edit, and delete reviews with rating aggregation
- **Wishlists** — Save favorite products for later
- **User Authentication** — Register, login, and view profile with JWT tokens

### 🔧 Admin Features
- **Product Management** — Full CRUD for products, badges, types, and themes
- **Order Management** — View all orders with pagination, update order status through 7-state lifecycle
- **Review Moderation** — Approve or disapprove user reviews
- **Delivery Methods** — Configure delivery options with localized names and descriptions

### 🌍 Platform Features
- **Bilingual Support** — Full English & Arabic localization across all product-facing entities
- **Response Caching** — Redis-based response caching with intelligent cache-key generation
- **Global Error Handling** — Custom exception middleware returning RFC 7807 ProblemDetails
- **API Documentation** — Interactive Swagger UI and Scalar API reference
- **Auto Data Seeding** — Database seeded from JSON files on startup with roles and default users

---

## 🔌 API Endpoints

### Public Endpoints

| Method | Endpoint | Description |
|:---:|:---|:---|
| `POST` | `/api/Authentication/login` | User login |
| `POST` | `/api/Authentication/register` | User registration |
| `GET` | `/api/Authentication/currentUser` | Get current user profile 🔒 |
| `GET` | `/api/Product` | Get all products (paginated) |
| `GET` | `/api/Product/{id}` | Get product by ID |
| `GET` | `/api/Product/types` | Get all product types |
| `GET` | `/api/Product/themes` | Get all product themes |
| `GET` | `/api/Product/badges` | Get all badges |
| `GET` | `/api/Basket` | Get user basket 🔒 |
| `POST` | `/api/Basket` | Create/update basket 🔒 |
| `DELETE` | `/api/Basket/{basketId}` | Delete basket 🔒 |
| `POST` | `/api/Order/Create` | Place a new order 🔒 |
| `GET` | `/api/Order/{id}` | Get order by ID 🔒 |
| `GET` | `/api/Order` | Get user's orders 🔒 |
| `PUT` | `/api/Order/CancelOrder` | Cancel a pending order 🔒 |
| `GET` | `/api/Order/DeliveryMethods` | Get delivery methods |
| `GET` | `/api/Review/product/{productId}` | Get product reviews (paginated) |
| `GET` | `/api/Review/product/{productId}/summary` | Get review summary |
| `POST` | `/api/Review` | Submit a review 🔒 |
| `PUT` | `/api/Review/{reviewId}` | Update a review 🔒 |
| `DELETE` | `/api/Review/{reviewId}` | Delete a review 🔒 |
| `GET` | `/api/Wishlist` | Get user wishlist 🔒 |
| `POST` | `/api/Wishlist/{productId}` | Add to wishlist 🔒 |
| `DELETE` | `/api/Wishlist/{productId}` | Remove from wishlist 🔒 |

> 🔒 = Requires JWT Bearer token

### Admin Endpoints (requires Admin role)

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/admin/products` | Get all products (paginated) |
| `POST` | `/api/admin/products` | Create product |
| `PUT` | `/api/admin/products/{id}` | Update product |
| `DELETE` | `/api/admin/products/{id}` | Delete product |
| `POST` | `/api/admin/products/badges` | Create badge |
| `PUT` | `/api/admin/products/badges/{id}` | Update badge |
| `DELETE` | `/api/admin/products/badges/{id}` | Delete badge |
| `GET` | `/api/admin/orders` | Get all orders (paginated) |
| `PUT` | `/api/admin/orders/{orderId}/status` | Update order status |
| `GET` | `/api/admin/reviews` | Get all reviews (paginated) |
| `PATCH` | `/api/admin/reviews/{reviewId}/approve` | Approve review |
| `PATCH` | `/api/admin/reviews/{reviewId}/disapprove` | Disapprove review |
| `GET` | `/api/admin/delivery-methods` | Get delivery methods |
| `POST` | `/api/admin/delivery-methods` | Create delivery method |
| `PUT` | `/api/admin/delivery-methods/{id}` | Update delivery method |
| `DELETE` | `/api/admin/delivery-methods/{id}` | Delete delivery method |

---

## 🔐 Authentication & Authorization

- **ASP.NET Core Identity** with custom `ApplicationUser` entity extending `IdentityUser`
- **JWT Bearer** tokens (HMAC-SHA256 signing, 24-hour expiry, zero clock skew)
- **Role-based access control** with three tiers:
  - `TopTierHuman` — Full system access
  - `SuperAdmin` — Administrative access
  - `Admin` — Standard admin access
- **Policy-based authorization** — `AdminDashboard` policy protects all admin endpoints
- **Claims extraction** — `ICurrentUserService` extracts user identity from JWT claims via `HttpContext`

---

## 🌍 Localization

The API supports **bilingual content (English & Arabic)** through a **Translation Entity Pattern**:

- Each localizable entity has a corresponding translation table (e.g., `Product` → `ProductTranslation`)
- Translations implement the `ITranslation` interface (`Language`, `Name`)
- Entities implement `ITranslatableEntity<TTranslation>` with a `Translations` navigation property
- DTOs use `LocalizedString { En, Ar }` records for clean API responses
- Mapping handled via `TranslationExtensions.ToLocalized()` extension method

**Localized entities:** Products, Product Types, Product Themes, Badges, Delivery Methods

---

## ⚡ Caching Strategy

| Layer | Technology | Purpose |
|:---|:---|:---|
| **Basket Storage** | Redis (StackExchange.Redis) | Customer baskets stored as JSON with 24h TTL |
| **Response Caching** | Custom `RedisCacheAttribute` | Action filter caches GET responses with auto-generated keys |
| **General Cache** | `ICacheService` | Abstracted Redis string get/set for arbitrary data |
| **Production** | Upstash Redis (Cloud) | Managed cloud Redis for production deployment |

The `RedisCacheAttribute` builds cache keys from the request path and query parameters, with intelligent skipping when search parameters are present.

---

## 🧪 Testing

| Framework | Purpose |
|:---|:---|
| **xUnit** | Test framework |
| **Moq** | Mocking dependencies |
| **FluentAssertions** | Readable assertion syntax |
| **FluentValidation** | Validator testing |

**Test coverage includes:**
- **Domain layer** — Product entity rating calculation logic (9 tests: `IncludeReviewInRatings`, `ExcludeReviewFromRatings`, `UpdateReviewRating`)
- **Application layer** — Command handlers (`CreateProductCommandHandler`: success paths, failure paths, behavioral verification, edge cases), Query handlers (`GetProductByIdQueryHandler`), Validators (`CreateProductCommandValidator`)

```bash
dotnet test
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB or full instance)
- [Redis](https://redis.io/download) (local or cloud via [Upstash](https://upstash.com/))

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/madamaky/MekkysCakesApi.git
   cd MekkysCakesApi
   ```

2. **Update connection strings** in `MekkysCakes.Web/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your SQL Server connection string",
       "Redis": "Your Redis connection string"
     }
   }
   ```

3. **Run the application**
   ```bash
   dotnet run --project MekkysCakes.Web
   ```

4. **Access the API**
   - Swagger UI: `https://localhost:7162/swagger`
   - Scalar Reference: `https://localhost:7162/scalar`

> The database is automatically migrated and seeded on first startup with sample products, roles, and default admin users.

---

## 🗄 Database Schema

The application uses **EF Core Code-First** with **Fluent API** configurations across **10 migrations**:

### Core Entities

```
ApplicationUser (Identity)
├── Address (1:1)
├── Orders (1:N)
├── ProductReviews (1:N)
└── Wishlist (1:1)

Product
├── ProductTranslation (1:N) [EN/AR]
├── ProductBadges (M:N → Badge)
├── ProductReviews (1:N)
├── ProductType (N:1) → ProductTypeTranslation (1:N)
└── ProductTheme (N:1) → ProductThemeTranslation (1:N)

Order
├── OrderItems (1:N) → ProductItemOrdered (owned)
└── DeliveryMethod (N:1) → DeliveryMethodTranslation (1:N)

Badge → BadgeTranslation (1:N)
Wishlist → WishlistItems (1:N) → Product
CustomerBasket (Redis, not SQL)
```

### Order Status Lifecycle

```
Pending → Confirmed → Preparing → OutForDelivery → Delivered
    └──→ Cancelled
    └──→ Refunded
```

---

<div align="center">

**Built with ❤️ using .NET 8 & Clean Architecture**

</div>
