# Day 3.1: Database Fundamentals

## Objective

Master database schema design and optimization:
- Entity relationship modeling
- Normalization principles
- Indexing strategies
- Query performance analysis
- Best practices

## What You'll Learn

1. Designing schemas with GitHub Copilot assistance
2. Applying normalization rules
3. Index creation and maintenance
4. Performance analysis with EXPLAIN PLAN
5. Scalability considerations

## Normalization Forms

| Form | Rules |
|------|-------|
| **1NF** | Atomic values, no repeating groups |
| **2NF** | 1NF + remove partial dependencies |
| **3NF** | 2NF + remove transitive dependencies |
| **BCNF** | 3NF + all determinants are candidate keys |

## Database Design Patterns

### E-Commerce Schema Example

```sql
-- Users table (normalized)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Products table
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

-- Orders table (normalized)
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- OrderItems (many-to-many between Orders and Products)
CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);
```

## Indexing Strategy

### Primary Indexes
- ✅ Primary Key (automatic)
- ✅ Foreign Keys for JOINs
- ✅ Frequently searched columns

### Secondary Indexes
- ✅ Email, Username (unique)
- ✅ Created dates (time-series queries)
- ✅ Status columns (filtering)

### Composite Indexes
- ✅ (UserId, OrderDate) for user-specific queries
- ✅ (ProductId, CategoryId) for product filtering

## Performance Analysis

Use `EXPLAIN PLAN` or execution plans:

```sql
-- Analyze query performance
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

-- Your query here
SELECT * FROM Products WHERE CategoryId = 1;

SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
```

## GitHub Copilot Database Prompts

1. "Design a normalized schema for a blog system"
2. "Create optimal indexes for this query: [SQL]"
3. "Explain this execution plan and suggest optimizations"
4. "Normalize this denormalized table"
5. "Generate a schema diagram from this SQL"
