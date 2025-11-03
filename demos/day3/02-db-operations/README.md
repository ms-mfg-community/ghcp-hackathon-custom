# Day 3.2: Database Operations & Scripting

## Objective

Generate SQL operations and scripts with GitHub Copilot:
- CRUD operations for all entities
- Complex queries with aggregation
- Stored procedures
- Data migration and ETL scripts
- Backup and maintenance scripts

## What You'll Learn

1. CRUD pattern generation in SQL
2. Aggregation and grouping operations
3. Stored procedure generation
4. Transaction management
5. Bulk operations and performance

## CRUD Operation Patterns

### CREATE
```sql
INSERT INTO Products (Name, Price, Stock, CategoryId)
VALUES ('New Product', 99.99, 50, 1);
```

### READ
```sql
SELECT ProductId, Name, Price, Stock
FROM Products
WHERE CategoryId = 1 AND IsActive = 1;
```

### UPDATE
```sql
UPDATE Products
SET Price = 109.99, UpdatedAt = GETDATE()
WHERE ProductId = 1;
```

### DELETE
```sql
UPDATE Products
SET IsActive = 0
WHERE ProductId = 1;  -- Soft delete
```

## Stored Procedure Examples

```sql
-- Generate order
CREATE PROCEDURE sp_CreateOrder
    @UserId INT,
    @OrderId INT OUTPUT
AS
BEGIN
    BEGIN TRANSACTION;
    
    INSERT INTO Orders (UserId, TotalAmount, Status)
    VALUES (@UserId, 0, 'Pending');
    
    SET @OrderId = SCOPE_IDENTITY();
    
    COMMIT TRANSACTION;
END;
```

## GitHub Copilot SQL Prompts

1. "Generate a stored procedure to calculate monthly sales revenue"
2. "Create a query that finds customers who haven't ordered in 6 months"
3. "Write an ETL script to migrate data from old to new schema"
4. "Generate CRUD operations for Product entity"
5. "Create a maintenance script to clean up old orders"
