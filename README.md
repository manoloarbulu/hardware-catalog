# Hardware Catalog - Full-Stack Demo Portal

A complete production-ready demo application showcasing a modern full-stack architecture with a **.NET 10 Web API backend** and **React 18 TypeScript frontend** for managing a hardware component catalog.

## 🎯 Project Overview

The Hardware Catalog is a comprehensive demo application that demonstrates:

- **Clean Architecture** in the backend with CQRS pattern
- **Type-safe React** components with TypeScript
- **RESTful API** design with validation and error handling
- **Natural language** product search
- **Responsive UI** with Tailwind CSS

### Key Features

**Backend Features:**

- ✅ CQRS pattern with MediatR (5 commands, 3 queries)
- ✅ FluentValidation with automatic pipeline validation
- ✅ Entity Framework Core with SQL Server LocalDB
- ✅ Swagger/OpenAPI documentation
- ✅ CORS configured for frontend
- ✅ Auto-migration and data seeding
- ✅ 12 pre-seeded brands and 38 products
- ✅ **Comprehensive XUnit test suite with 21 tests**

**Frontend Features:**

- ✅ React 18 with TypeScript (strict mode)
- ✅ Vite for fast development and builds
- ✅ Tailwind CSS for responsive design
- ✅ Axios HTTP client with typed DTOs
- ✅ Custom React hooks for state management
- ✅ Computer CRUD operations
- ✅ Product search with natural language support

---

## 📋 Table of Contents

1. [Quick Start](#-quick-start) - Get running in 5 minutes
2. [Technology Stack](#-technology-stack) - What we're using
3. [Project Structure](#-project-structure) - File organization
4. [Running the Application](#-running-the-application) - Step-by-step guide
5. [Testing](#-testing) - Running and writing tests
6. [API Documentation](#-api-documentation) - Available endpoints
7. [Features & Components](#-features--components) - What you can do
8. [Database Schema](#-database-schema) - Data model
9. [Troubleshooting](#-troubleshooting) - Common issues
10. [Development](#-development) - Extending the project

---

## 🚀 Quick Start

### Prerequisites

- **Node.js** v20.19.0 or v22.12.0+ (check with `node --version`)
- **.NET 10 SDK** (check with `dotnet --version`)
- **SQL Server LocalDB** (included with Visual Studio)

### Start Backend (Terminal 1)

```bash
cd backend
dotnet run --project HardwareCatalog.WebApi
```

✅ Backend at: `https://localhost:7152`
✅ Swagger docs at: `https://localhost:7152/swagger/index.html`

### Start Frontend (Terminal 2)

```bash
cd frontend
npm install  # Only needed first time
npm run dev
```

✅ Frontend at: `http://localhost:5173`

**That's it!** Both services will be running and communicating with each other.

---

## 💻 Technology Stack

### Backend

| Technology                    | Version  | Purpose                        |
| ----------------------------- | -------- | ------------------------------ |
| .NET                          | 10.0     | Web API framework              |
| Entity Framework Core         | 10.0     | ORM and database access        |
| MediatR                       | 12.5.0   | CQRS pattern implementation    |
| FluentValidation              | 11.12.0  | Request validation             |
| SQL Server LocalDB            | Latest   | Database                       |
| Swagger                       | Built-in | API documentation              |
| **XUnit**                     | 2.7.1    | **Unit testing framework**     |
| **Moq**                       | 4.20.70  | **Mocking library**            |
| **FluentAssertions**          | 6.12.0   | **Readable assertions**        |
| **EF Core InMemory Database** | 10.0.0   | **In-memory testing database** |

### Frontend

| Technology   | Version | Purpose                   |
| ------------ | ------- | ------------------------- |
| React        | 18.3.1  | UI library                |
| TypeScript   | 5.5.0   | Type safety               |
| Vite         | 5.1.6   | Build tool and dev server |
| Tailwind CSS | 3.4.10  | Styling                   |
| Axios        | 1.7.8   | HTTP client               |
| ESLint       | 8.57.0  | Code linting              |

---

## 📁 Project Structure

```
hardware-catalog/
│
├── README.md                    # This file
├── QUICK_START.md              # Quick reference
├── SETUP_AND_RUN.md            # Detailed setup guide
│
├── backend/                    # .NET 10 Web API
│   ├── HardwareCatalog.Domain/
│   │   ├── Entities/           # Brand, Product, Computer, ComputerProduct
│   │   └── Enums/              # ProductCategory, UnitOfMeasure, WeightUnit, ComputerType
│   │
│   ├── HardwareCatalog.Application/
│   │   ├── Commands/           # CreateComputer, UpdateComputer, DeleteComputer
│   │   ├── Queries/            # GetAllComputers, GetComputerById, SearchProducts
│   │   ├── Handlers/           # Command and query handlers
│   │   ├── Validators/         # FluentValidation validators
│   │   ├── DTOs/               # ProductDto, ComputerDto, ComputerProductDto
│   │   ├── Behaviors/          # ValidationBehavior (MediatR pipeline)
│   │   └── Extensions/         # ServiceCollectionExtensions
│   │
│   ├── HardwareCatalog.Infrastructure/
│   │   ├── Persistence/        # ApplicationDbContext, entity configurations
│   │   ├── Seeding/            # DataSeeder (12 brands, 38 products)
│   │   └── Extensions/         # ServiceCollectionExtensions
│   │
│   ├── HardwareCatalog.WebApi/
│   │   ├── Controllers/        # ComputersController, ProductsController
│   │   ├── Program.cs          # Startup configuration
│   │   ├── appsettings.json    # Configuration and connection string
│   │   └── launchSettings.json # Port and environment settings
│   │
│   ├── HardwareCatalog.Tests/
│   │   ├── Validators/         # Validator tests (CreateComputerCommandValidatorTests)
│   │   ├── Handlers/           # Handler tests (SearchProductsQueryHandlerTests)
│   │   ├── Entities/           # Entity tests (ComputerEntityTests, ProductEntityTests)
│   │   └── HardwareCatalog.Tests.csproj  # XUnit, Moq, FluentAssertions setup
│   │
│   └── HardwareCatalog.slnx    # Solution file
│
└── frontend/                   # React TypeScript SPA
    ├── src/
    │   ├── components/
    │   │   ├── ComputerList.tsx     # Table with Edit/Delete actions
    │   │   ├── ComputerForm.tsx     # CRUD form
    │   │   └── ProductSearch.tsx    # Search interface
    │   │
    │   ├── hooks/
    │   │   ├── useComputers.ts      # Computer CRUD logic
    │   │   └── useProductSearch.ts  # Search logic
    │   │
    │   ├── services/
    │   │   └── api.ts               # Axios client with types
    │   │
    │   ├── App.tsx                  # Main application
    │   ├── main.tsx                 # Entry point
    │   └── index.css                # Tailwind styles
    │
    ├── vite.config.ts              # Vite configuration with API proxy
    ├── tsconfig.json               # TypeScript settings (strict mode)
    ├── tailwind.config.js          # Tailwind CSS configuration
    ├── postcss.config.js           # PostCSS configuration
    ├── index.html                  # HTML template
    ├── package.json                # Dependencies and scripts
    └── .eslintrc.cjs               # ESLint configuration
```

---

## 🏃 Running the Application

### Prerequisites Check

```bash
# Verify Node.js version (must be 20.19.0 or 22.12.0+)
node --version

# Verify .NET version
dotnet --version
```

### Step 1: Start Backend

```bash
# Terminal 1
cd backend
dotnet run --project HardwareCatalog.WebApi
```

**Expected output:**

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7152
```

The database will be:

1. Created on SQL Server LocalDB (ProductsDemo)
2. Migrated with all tables
3. Seeded with 12 brands and 38 products

### Step 2: Start Frontend

```bash
# Terminal 2
cd frontend
npm install  # Skip if already done
npm run dev
```

**Expected output:**

```
  VITE v5.1.6  ready in XXX ms

  ➜  Local:   http://localhost:5173/
  ➜  press h to show help
```

### Step 3: Open Browser

Navigate to `http://localhost:5173`

You should see:

- Navigation bar with "Hardware Catalog"
- Buttons for: Computers, Add Computer, Search Products
- Default view: Computer list (empty on first run)

---

## 🧪 Testing

The backend includes a comprehensive XUnit test suite with **21 tests** covering validators, handlers, and entities.

### Running Tests

**Run all tests:**

```bash
cd backend
dotnet test
```

**Run with detailed output:**

```bash
dotnet test --verbosity detailed
```

**Run specific test file:**

```bash
dotnet test HardwareCatalog.Tests/HardwareCatalog.Tests.csproj
```

### Test Structure

The test project (`HardwareCatalog.Tests`) includes:

1. **Validator Tests** (`Validators/CreateComputerCommandValidatorTests.cs`)
   - Valid command scenarios
   - Invalid weight (zero or negative)
   - Missing products
   - Invalid quantity values

2. **Handler Tests** (`Handlers/SearchProductsQueryHandlerTests.cs`)
   - Query execution with matching products
   - Empty result sets
   - Keyword extraction and search

3. **Entity Tests** (`Entities/EntityTests.cs`)
   - Computer entity initialization
   - Product navigation properties
   - ComputerProduct relationships
   - Type theory tests (all enum values)

### Test Technologies

- **XUnit 2.7.1** - Testing framework
- **Moq 4.20.70** - Mocking framework
- **FluentAssertions 6.12.0** - Readable assertions
- **EntityFrameworkCore.InMemory 10.0.0** - In-memory database for tests

### Test Results Example

```
Test summary: total: 21; failed: 0; succeeded: 21; skipped: 0; duration: 5.3s
```

---

## 🔌 API Documentation

All endpoints return JSON and support CORS requests from the frontend.

### Computers Endpoints

#### Get All Computers

```http
GET /api/computers
```

**Response:** `200 OK`

```json
[
  {
    "id": "uuid",
    "creationDate": "2024-01-15T10:30:00Z",
    "type": "Desktop",
    "weight": 12.5,
    "weightUnit": "Kilograms",
    "description": "Gaming PC",
    "manufacturer": "Custom",
    "products": [
      {
        "productId": "uuid",
        "quantity": 1,
        "product": {
          "id": "uuid",
          "name": "Product Name",
          "category": "Processor",
          "brandName": "Intel"
        }
      }
    ]
  }
]
```

#### Get Computer by ID

```http
GET /api/computers/{id}
```

**Response:** `200 OK` (or `404 Not Found`)

#### Create Computer

```http
POST /api/computers
Content-Type: application/json

{
  "type": "Desktop",
  "weight": 15.5,
  "weightUnit": "Kilograms",
  "description": "Gaming PC",
  "serialNumber": "ABC123",
  "manufacturer": "Dell",
  "products": [
    {
      "productId": "00000000-0000-0000-0000-000000000001",
      "quantity": 1
    }
  ]
}
```

**Response:** `201 Created` with created computer object

#### Update Computer

```http
PUT /api/computers/{id}
Content-Type: application/json

{
  "id": "{id}",
  "type": "Desktop",
  "weight": 16.0,
  "weightUnit": "Kilograms",
  "products": [...]
}
```

**Response:** `200 OK` with updated computer

#### Delete Computer

```http
DELETE /api/computers/{id}
```

**Response:** `204 No Content`

### Products Endpoints

#### Search Products

```http
GET /api/products/search?query=<natural-language-query>
```

**Example queries:**

- `"16GB memory"` - Find 16GB RAM modules
- `"gaming graphics card"` - Find high-end GPUs
- `"storage drives"` - Find storage products
- `"Intel processor"` - Find Intel CPUs

**Response:** `200 OK`

```json
[
  {
    "id": "uuid",
    "name": "Product Name",
    "category": "Memory",
    "unitOfMeasure": "GB",
    "model": "DDR5-4800",
    "brandId": "uuid",
    "brandName": "Kingston"
  }
]
```

---

## 🎨 Features & Components

### Dashboard / Computer List

**File:** `frontend/src/components/ComputerList.tsx`

Displays all computers in a responsive table with:

- Computer type (Laptop, Desktop, Server, BladeServer)
- Weight and weight unit
- Creation date
- Number of products
- Edit button (loads into form)
- Delete button (with confirmation)

### Computer Form (Create/Edit)

**File:** `frontend/src/components/ComputerForm.tsx`

Form for creating and editing computers with:

- Computer type dropdown
- Weight input (decimal support)
- Weight unit selection
- Optional manufacturer, serial number, description
- Product multi-select with quantity inputs
- Form validation (weight > 0, at least 1 product)
- Submit and Cancel buttons

### Product Search

**File:** `frontend/src/components/ProductSearch.tsx`

Natural language product search with:

- Search input box
- Search button
- Results displayed in responsive grid
- Product card showing: name, brand, model, category, unit
- "Add to Computer" button for each product

### Custom Hooks

**File:** `frontend/src/hooks/`

#### useComputers

Manages computer state and API calls:

- `fetchComputers()` - Load all computers
- `addComputer(data)` - Create new computer
- `updateComputer(id, data)` - Update computer
- `deleteComputer(id)` - Delete computer
- Error handling and loading state

#### useProductSearch

Manages product search state:

- `search(query)` - Search products
- Returns results, loading, and error states

---

## 🗄️ Database Schema

### Tables

#### Brands

```sql
CREATE TABLE Brands (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name VARCHAR(MAX) NOT NULL
);
```

#### Products

```sql
CREATE TABLE Products (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Category INT NOT NULL,           -- Enum
    Name VARCHAR(MAX) NOT NULL,
    UnitOfMeasure INT NOT NULL,      -- Enum
    BrandId UNIQUEIDENTIFIER NOT NULL,
    Model VARCHAR(MAX) NOT NULL,
    FOREIGN KEY (BrandId) REFERENCES Brands(Id)
);
```

#### Computers

```sql
CREATE TABLE Computers (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    CreationDate DATETIME2 NOT NULL,
    Type INT NOT NULL,               -- Enum
    Weight DECIMAL(18,2) NOT NULL,
    WeightUnit INT NOT NULL,         -- Enum
    Description NVARCHAR(MAX),
    SerialNumber NVARCHAR(MAX),
    ManufactureDate DATETIME2,
    Manufacturer NVARCHAR(MAX)
);
```

#### ComputerProducts (Junction Table)

```sql
CREATE TABLE ComputerProducts (
    ComputerId UNIQUEIDENTIFIER NOT NULL,
    ProductId UNIQUEIDENTIFIER NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    PRIMARY KEY (ComputerId, ProductId),
    FOREIGN KEY (ComputerId) REFERENCES Computers(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
```

### Sample Data

**12 Brands:** Intel, AMD, NVIDIA, Kingston, Western Digital, Seagate, Corsair, MSI, Dell, HP, Lenovo, IBM

**38 Products** across 6 categories:

- Processors (8): Intel i5, i7, Celeron; AMD FX, Athlon
- Graphics (7): NVIDIA GTX series; AMD Radeon series
- Memory (4): Kingston DDR5 modules 8GB-32GB
- Storage (7): SSD and HDD drives from 512GB-4TB
- Power Supplies (5): 450W-1000W units
- Ports (3): USB 3.0, USB 2.0, USB-C

---

## 🔧 Troubleshooting

### Frontend Issues

#### Node Version Error

**Problem:** `Error: The engine "node" is incompatible`
**Solution:** Upgrade Node.js to v20.19.0 or v22.12.0+

```bash
# Check current version
node --version

# Download from https://nodejs.org/
```

#### Cannot Find Module Errors

**Problem:** `Cannot find module '@vitejs/plugin-react'`
**Solution:** Reinstall dependencies

```bash
cd frontend
rm -r node_modules package-lock.json
npm install
```

#### Frontend Can't Connect to Backend

**Problem:** Network errors or CORS issues in DevTools
**Solution:**

1. Verify backend is running: `https://localhost:7152`
2. Check that backend is accessible in browser
3. Verify Vite proxy in `vite.config.ts` points to `https://localhost:7152`

### Backend Issues

#### Database Not Found

**Problem:** `Login failed for user 'DESKTOP\\Username'`
**Solution:** Ensure SQL Server LocalDB is installed

```bash
# Check if LocalDB exists
sqllocaldb info

# Create instance if needed
sqllocaldb create mssqllocaldb
sqllocaldb start mssqllocaldb
```

#### Port 7152 Already in Use

**Problem:** `Unable to start Kestrel on address`
**Solution:** Change port in `backend/HardwareCatalog.WebApi/Properties/launchSettings.json`

#### No Swagger UI

**Problem:** Swagger returns 404
**Solution:** Use HTTPS: `https://localhost:7152/swagger/index.html`

#### Database Seeding Takes Long

**Problem:** First run takes 10-30 seconds
**Solution:** This is normal - the app is:

1. Creating the database
2. Running migrations
3. Seeding 12 brands + 38 products
4. Building indexes

Wait for the app to show it's listening before testing.

---

## 📈 Development & Extension

### Project Status

✅ Production-ready foundation with:

- Clean Architecture implementation
- CQRS pattern with validation
- Type-safe API client
- Responsive UI with Tailwind CSS
- Full CRUD operations
- Natural language search

### Recommended Enhancements

1. **Authentication & Authorization**
   - Add user login with JWT tokens
   - Restrict computer management by user
   - Role-based access control

2. **Testing**
   - Backend: xUnit tests for handlers and validators
   - Frontend: Jest/React Testing Library tests
   - Integration tests for API endpoints

3. **Additional Features**
   - Inventory/stock tracking
   - Computer performance ratings
   - Bulk import (CSV/Excel)
   - Export functionality (PDF, Excel)
   - Audit logging for changes
   - Advanced filtering and sorting

4. **Performance**
   - Add pagination for large datasets
   - Implement caching strategy
   - Optimize database queries
   - Add database indexes

5. **Deployment**
   - Docker containerization
   - CI/CD pipeline (GitHub Actions, Azure DevOps)
   - Environment-specific configurations
   - Database migration strategy

### Building for Production

#### Backend

```bash
cd backend
dotnet publish -c Release -o ./publish
```

#### Frontend

```bash
cd frontend
npm run build  # Creates dist/ folder
```

---

## 📝 Documentation Files

- **README.md** (this file) - Project overview
- **QUICK_START.md** - 5-minute quick reference
- **SETUP_AND_RUN.md** - Comprehensive setup guide (300+ lines)
- Swagger/OpenAPI docs at `/swagger/index.html` when backend running

---

## 📞 Support

For issues or questions:

1. Check the **Troubleshooting** section above
2. Review the **SETUP_AND_RUN.md** for detailed instructions
3. Check browser DevTools (F12) for frontend errors
4. Review terminal output for backend errors
5. Visit Swagger UI documentation for API details

---

## 📄 License

This is a demo project. Feel free to use, modify, and distribute as needed.

---

## 🎓 Learning Resources

This project demonstrates:

- **Clean Architecture** principles
- **CQRS pattern** with MediatR
- **Validation pipeline** behavior pattern
- **Type-safe React** with TypeScript
- **Custom hooks** for state management
- **API client** design with Axios
- **Responsive UI** with Tailwind CSS
- **RESTful API** design
- **Entity Framework Core** relationships and seeding
- **Async/await** patterns

---

**Status: ✅ Ready to Use**

Start with the [Quick Start](#-quick-start) section above!
