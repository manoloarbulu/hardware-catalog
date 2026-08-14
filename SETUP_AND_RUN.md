# Hardware Catalog - Full-Stack Demo Portal

This is a complete full-stack demo application for a hardware catalog management system. The application consists of a .NET 10 Web API backend and a React TypeScript SPA frontend.

## Project Structure

```
hardware-catalog/
├── backend/                 # .NET 10 Web API with Clean Architecture
│   ├── HardwareCatalog.Domain/        # Domain entities and enums
│   ├── HardwareCatalog.Application/   # CQRS commands/queries, validators
│   ├── HardwareCatalog.Infrastructure/# Database context, seeding
│   ├── HardwareCatalog.WebApi/        # API controllers and configuration
│   └── HardwareCatalog.Tests/         # XUnit tests with Moq and FluentAssertions
└── frontend/               # React 18 TypeScript SPA
    ├── src/
    │   ├── components/     # React components
    │   ├── hooks/          # Custom React hooks
    │   ├── services/       # API client with Axios
    │   └── App.tsx         # Main application component
    └── package.json        # Frontend dependencies
```

## Technology Stack

### Backend

- **.NET 10** Web API
- **Entity Framework Core 10** with SQL Server
- **MediatR 12.5.0** for CQRS pattern
- **FluentValidation 11.12.0** for request validation
- **Swagger/OpenAPI** for API documentation

### Frontend

- **React 18.3.1** UI library
- **TypeScript 5.5.0** for type safety
- **Vite 5.1.6** for fast builds
- **Tailwind CSS 3.4.10** for styling
- **Axios 1.7.8** for HTTP requests

## Prerequisites

### Backend Requirements

- **Windows OS**
- **.NET 10 SDK** installed
- **SQL Server LocalDB** (included with Visual Studio or downloadable separately)

### Frontend Requirements

- **Node.js v20.19.0 or v22.12.0+** (Note: v20.10.0 is insufficient for Vite)
- **npm** (comes with Node.js)

## Setup Instructions

### Backend Setup

1. Navigate to the backend directory:

```bash
cd backend
```

2. Restore NuGet packages:

```bash
dotnet restore
```

3. Build the solution:

```bash
dotnet build
```

4. Run the Web API (it will automatically create the database and seed data):

```bash
dotnet run --project HardwareCatalog.WebApi
```

The API will be available at: `https://localhost:7152`

**Note**: The database "ProductsDemo" will be automatically created on SQL Server LocalDB, migrations will be applied, and sample data (12 brands and 38 products) will be seeded on first run.

### Frontend Setup

1. Navigate to the frontend directory:

```bash
cd frontend
```

2. Install dependencies:

```bash
npm install
```

3. Start the development server:

```bash
npm run dev
```

The frontend will be available at: `http://localhost:5173`

**Note**: The Vite dev server is configured with a proxy to forward `/api` requests to the backend at `https://localhost:7152/api`.

## Running Both Services Simultaneously

It's recommended to run both backend and frontend in separate terminal windows:

**Terminal 1 - Backend:**

```bash
cd backend
dotnet run --project HardwareCatalog.WebApi
```

**Terminal 2 - Frontend:**

```bash
cd frontend
npm run dev
```

## Running Backend Tests

The backend includes a comprehensive XUnit test suite with 21 tests covering validators, handlers, and entities.

**Run all tests:**

```bash
cd backend
dotnet test
```

**Run tests with detailed output:**

```bash
dotnet test --verbosity detailed
```

**Run specific test project:**

```bash
dotnet test HardwareCatalog.Tests/HardwareCatalog.Tests.csproj
```

**Test Coverage:**

- ✅ 21 unit tests across validators, handlers, and entities
- ✅ Validator tests: Positive and negative scenarios
- ✅ Handler tests: CRUD operations and query results
- ✅ Entity tests: Domain model behavior
- ✅ Using XUnit, Moq, FluentAssertions, and EntityFrameworkCore.InMemory

**Test Files:**

- `HardwareCatalog.Tests/Validators/CreateComputerCommandValidatorTests.cs` - Validator tests
- `HardwareCatalog.Tests/Handlers/SearchProductsQueryHandlerTests.cs` - Query handler tests
- `HardwareCatalog.Tests/Entities/EntityTests.cs` - Domain entity tests

## API Endpoints

### Computers

- `GET /api/computers` - Get all computers
- `GET /api/computers/{id}` - Get computer by ID
- `POST /api/computers` - Create new computer
- `PUT /api/computers/{id}` - Update computer
- `DELETE /api/computers/{id}` - Delete computer

### Products

- `GET /api/products/search?query=<search-query>` - Search products with natural language queries

**Example product search queries:**

- "Show me all storage drives"
- "processors from Intel"
- "16GB memory"
- "graphics cards"

## Database Schema

### Tables

- **Brands** - Computer component manufacturers (Intel, AMD, NVIDIA, etc.)
- **Products** - Hardware components (processors, memory, storage, etc.)
- **Computers** - Computer configurations
- **ComputerProducts** - Junction table linking computers to their components

### Sample Data

The database is seeded with:

- **12 Brands**: Intel, AMD, NVIDIA, Kingston, Western Digital, Seagate, Corsair, MSI, Dell, HP, Lenovo, IBM
- **38 Products** across 6 categories:
  - Processors (8)
  - Graphics Cards (7)
  - Memory (4)
  - Storage (7)
  - Power Supplies (5)
  - Ports (3)

## Application Features

### Dashboard

- View all configured computers in a searchable table
- See computer type, weight, creation date, and component count
- Edit or delete existing computers

### Computer Management

- Create new computers with type, weight, manufacturer info
- Add/remove hardware components and quantities
- Edit existing configurations
- Delete computers

### Product Search

- Natural language search for hardware components
- Search by category, brand, specifications
- Add products to computer configurations
- Browse all available components

## Frontend Components

### ComputerList

Displays all computers in a responsive table with actions for editing and deletion.

### ComputerForm

Form for creating and editing computer configurations. Includes:

- Computer type selection (Laptop, Desktop, Server, BladeServer)
- Weight input and unit selection
- Optional manufacturer, serial number, and description
- Product selection with quantities

### ProductSearch

Natural language product search interface. Supports queries like "show me 1TB storage" or "high-end graphics cards".

## Validation Rules

### Computer Creation/Update

- Type: Required, must be valid ComputerType enum
- Weight: Required, must be > 0
- Weight Unit: Required, must be valid WeightUnit enum
- Products: Required, at least 1 product must be added
- Each Product Quantity: Must be > 0

### Product Search

- Query: Required, minimum 2 characters

## CORS Configuration

The backend is configured with a CORS policy that allows all origins, methods, and headers. This enables the frontend running on a different port to communicate with the backend without restrictions.

## Troubleshooting

### Backend Issues

**Database not found error:**

- Ensure SQL Server LocalDB is installed
- Check that the connection string in `appsettings.json` is correct
- Verify Windows authentication is enabled

**Port already in use (7152):**

- Change the port in `launchSettings.json` or `appsettings.json`
- Or stop any existing process using that port

**Swagger UI not loading:**

- Ensure you're using `https://localhost:7152/swagger/index.html` (HTTPS required)
- Confirm you're running in Development environment

### Frontend Issues

**Node version too old:**

- Run `node --version` to check
- If < 20.19.0, upgrade Node.js from https://nodejs.org/
- Vite requires Node 20.19.0 or 22.12.0+

**Port 5173 already in use:**

- Change the port: `npm run dev -- --port 3000`
- Or stop the process using port 5173

**Cannot connect to backend:**

- Verify backend is running at `https://localhost:7152`
- Check browser DevTools Network tab for CORS errors
- Ensure HTTPS certificate is accepted (dev certificate for localhost)

**TypeScript compilation errors:**

- Run `npm install` again to ensure all types are installed
- Check that `tsconfig.json` is correctly configured
- Verify no circular dependencies exist

## Development Commands

### Backend

```bash
dotnet build                                    # Build solution
dotnet run --project HardwareCatalog.WebApi    # Run backend
dotnet test                                    # Run tests (if added)
```

### Frontend

```bash
npm install                 # Install dependencies
npm run dev                # Start dev server (Vite)
npm run build              # Build for production
npm run lint               # Run ESLint
npm run preview            # Preview production build locally
```

## Production Build

### Backend

```bash
dotnet publish -c Release -o ./publish
```

### Frontend

```bash
npm run build              # Creates dist/ folder
npm run preview            # Test production build locally
```

## Next Steps

To extend this application:

1. Add user authentication and authorization
2. Implement backend unit tests with xUnit
3. Add more advanced search filters
4. Create inventory/stock tracking features
5. Add audit logging for computer changes
6. Implement pagination for large datasets
7. Add export features (CSV, PDF)
8. Create admin dashboard with analytics

## Notes

- The backend automatically seeds the database on first run
- CORS is configured to allow all origins (not recommended for production)
- API responses follow the CQRS pattern with type-safe DTOs
- Frontend uses React hooks and custom hooks for state management
- TypeScript is configured in strict mode for maximum type safety

## Support

For issues or questions:

1. Check the Troubleshooting section above
2. Review the backend API documentation at `/swagger/index.html`
3. Check browser console (F12) for frontend errors
4. Review application logs in terminal output
