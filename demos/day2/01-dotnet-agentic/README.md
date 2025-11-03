# Day 2.1: .NET Development with Agentic Mode

## Objective

Demonstrate building complete .NET features using GitHub Copilot in Agentic Mode:
- CRUD API development
- Entity Framework integration
- Dependency injection patterns
- Multi-layer architecture

## What You'll Learn

1. How agents handle complex .NET architectural decisions
2. Generating complete service layers with agents
3. Database integration patterns
4. API controller generation
5. Request/response DTO generation

## Example Project Structure

```
DemoProject/
├── Models/
│   ├── Product.cs
│   ├── ProductDto.cs
│   └── AppDbContext.cs
├── Services/
│   ├── IProductService.cs
│   └── ProductService.cs
├── Controllers/
│   └── ProductsController.cs
├── Data/
│   └── DataSeeder.cs
└── Program.cs
```

## Key Patterns Demonstrated

1. **Entity Classes** - Database models with relationships
2. **DTOs** - Data transfer objects for API contracts
3. **DbContext** - Entity Framework context configuration
4. **Service Layer** - Business logic separation
5. **Dependency Injection** - Service registration
6. **Controllers** - API endpoints

## Running the Demonstration

```powershell
# Create new .NET project
dotnet new webapi -n DotNetAgentic
cd DotNetAgentic

# Copy demonstration files
copy Product.cs Models/
copy ProductDto.cs Models/
copy AppDbContext.cs Models/
copy IProductService.cs Services/
copy ProductService.cs Services/
copy ProductsController.cs Controllers/
copy Program.cs

# Run the application
dotnet run

# API will be available at: https://localhost:5001/api/products
```

## Agent Prompts to Try

1. "Build a complete CRUD API for Product management with Entity Framework"
2. "Generate a service layer with dependency injection for product operations"
3. "Create DTOs and mapping logic for Product entities"
4. "Implement repository pattern for data access"
5. "Add validation and error handling to API endpoints"
