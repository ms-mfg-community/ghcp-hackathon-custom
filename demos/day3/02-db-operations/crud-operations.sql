-- Day 3.2 Demo: Database Operations & CRUD Scripts
-- Generated with GitHub Copilot for common database operations

-- ============================================================
-- CRUD OPERATIONS
-- ============================================================

-- ---- CREATE OPERATIONS ----

-- Add new product
INSERT INTO Products (Name, Description, Price, Stock, CategoryId, Sku)
VALUES ('New Laptop', 'Latest model laptop', 1499.99, 10, 1, 'LAP-2024-001');

-- Add new order
INSERT INTO Orders (UserId, TotalAmount, Status, ShippingAddress)
VALUES (1, 99.98, 'Pending', '123 Main St, City, State');

-- Add order item
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
VALUES (1, 1, 1, 99.99);

-- ---- READ OPERATIONS ----

-- Get all active products
SELECT ProductId, Name, Price, Stock, Rating
FROM Products
WHERE IsActive = 1
ORDER BY Name;

-- Get product details with category
SELECT 
    p.ProductId,
    p.Name,
    p.Description,
    p.Price,
    p.Stock,
    c.Name as Category,
    p.Rating,
    p.CreatedAt
FROM Products p
INNER JOIN Categories c ON p.CategoryId = c.CategoryId
WHERE p.ProductId = 1;

-- Get user's recent orders
SELECT 
    o.OrderId,
    o.OrderDate,
    o.Status,
    o.TotalAmount,
    COUNT(oi.OrderItemId) as ItemCount
FROM Orders o
LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
WHERE o.UserId = 1 AND o.OrderDate >= DATEADD(MONTH, -6, GETDATE())
GROUP BY o.OrderId, o.OrderDate, o.Status, o.TotalAmount
ORDER BY o.OrderDate DESC;

-- ---- UPDATE OPERATIONS ----

-- Update product price
UPDATE Products
SET Price = 1199.99, UpdatedAt = GETDATE()
WHERE ProductId = 1;

-- Update order status
UPDATE Orders
SET Status = 'Shipped', ShippingDate = GETDATE(), UpdatedAt = GETDATE()
WHERE OrderId = 1;

-- Bulk update: Mark products as inactive
UPDATE Products
SET IsActive = 0, UpdatedAt = GETDATE()
WHERE CreatedAt < DATEADD(YEAR, -1, GETDATE())
  AND Stock = 0;

-- ---- DELETE OPERATIONS (SOFT DELETE) ----

-- Archive old orders
UPDATE Orders
SET Status = 'Archived'
WHERE OrderDate < DATEADD(YEAR, -2, GETDATE());

-- Remove inactive products
UPDATE Products
SET IsActive = 0
WHERE Stock = 0 AND Rating < 2;

-- ============================================================
-- AGGREGATE QUERIES
-- ============================================================

-- Monthly sales summary
SELECT 
    YEAR(o.OrderDate) as Year,
    MONTH(o.OrderDate) as Month,
    COUNT(DISTINCT o.OrderId) as OrderCount,
    COUNT(DISTINCT o.UserId) as UniqueCustomers,
    SUM(o.TotalAmount) as TotalRevenue,
    AVG(o.TotalAmount) as AvgOrderValue
FROM Orders o
WHERE o.Status != 'Cancelled'
GROUP BY YEAR(o.OrderDate), MONTH(o.OrderDate)
ORDER BY Year DESC, Month DESC;

-- Top products by revenue
SELECT TOP 10
    p.ProductId,
    p.Name,
    SUM(oi.Quantity) as TotalUnitsSold,
    SUM(oi.Quantity * oi.UnitPrice) as TotalRevenue,
    AVG(r.Rating) as AvgRating
FROM Products p
INNER JOIN OrderItems oi ON p.ProductId = oi.ProductId
LEFT JOIN Reviews r ON p.ProductId = r.ProductId
GROUP BY p.ProductId, p.Name
ORDER BY TotalRevenue DESC;

-- Customer lifetime value
SELECT TOP 10
    u.UserId,
    u.Username,
    COUNT(DISTINCT o.OrderId) as OrderCount,
    SUM(o.TotalAmount) as LifetimeValue,
    MAX(o.OrderDate) as LastOrderDate,
    AVG(o.TotalAmount) as AvgOrderValue
FROM Users u
LEFT JOIN Orders o ON u.UserId = o.UserId AND o.Status != 'Cancelled'
GROUP BY u.UserId, u.Username
ORDER BY LifetimeValue DESC;

-- ============================================================
-- STORED PROCEDURES
-- ============================================================

CREATE PROCEDURE sp_GetProductStats
    @ProductId INT
AS
BEGIN
    SELECT 
        p.ProductId,
        p.Name,
        p.Price,
        p.Stock,
        COUNT(DISTINCT oi.OrderId) as TimesPurchased,
        SUM(oi.Quantity) as TotalUnitsSold,
        SUM(oi.Quantity * oi.UnitPrice) as Revenue,
        AVG(CAST(r.Rating AS FLOAT)) as AvgRating,
        COUNT(DISTINCT r.ReviewId) as ReviewCount
    FROM Products p
    LEFT JOIN OrderItems oi ON p.ProductId = oi.ProductId
    LEFT JOIN Reviews r ON p.ProductId = r.ProductId
    WHERE p.ProductId = @ProductId
    GROUP BY p.ProductId, p.Name, p.Price, p.Stock;
END;

CREATE PROCEDURE sp_CompleteOrder
    @OrderId INT,
    @DeliveryDate DATETIME
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        UPDATE Orders
        SET Status = 'Delivered', 
            DeliveryDate = @DeliveryDate,
            UpdatedAt = GETDATE()
        WHERE OrderId = @OrderId;
        
        IF @@ROWCOUNT = 0
            THROW 50001, 'Order not found', 1;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;

-- ============================================================
-- UTILITY FUNCTIONS
-- ============================================================

CREATE FUNCTION fn_GetOrderTotal(@OrderId INT)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @Total DECIMAL(18, 2);
    
    SELECT @Total = SUM(Quantity * UnitPrice * (1 - Discount/100))
    FROM OrderItems
    WHERE OrderId = @OrderId;
    
    RETURN ISNULL(@Total, 0);
END;

-- ============================================================
-- TRANSACTION EXAMPLES
-- ============================================================

-- Example: Multi-step order creation with rollback protection
BEGIN TRANSACTION;

BEGIN TRY
    -- Create order
    INSERT INTO Orders (UserId, TotalAmount, Status)
    VALUES (1, 0, 'Processing');
    
    DECLARE @OrderId INT = SCOPE_IDENTITY();
    
    -- Add items
    INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
    VALUES (@OrderId, 1, 1, 99.99);
    
    -- Update inventory
    UPDATE Products
    SET Stock = Stock - 1
    WHERE ProductId = 1;
    
    -- Calculate and update total
    UPDATE Orders
    SET TotalAmount = (SELECT SUM(Quantity * UnitPrice) FROM OrderItems WHERE OrderId = @OrderId)
    WHERE OrderId = @OrderId;
    
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH;

-- ============================================================
-- DATA EXPORT / BACKUP QUERIES
-- ============================================================

-- Export products to CSV-friendly format
SELECT 
    ProductId,
    Name,
    Description,
    Price,
    Stock,
    CategoryId,
    Sku,
    Rating,
    CreatedAt
FROM Products
WHERE IsActive = 1
ORDER BY ProductId;

-- Get data for analytics
SELECT 
    o.OrderId,
    o.OrderDate,
    u.Country,
    c.Name as Category,
    p.Name as Product,
    oi.Quantity,
    oi.UnitPrice
FROM Orders o
INNER JOIN Users u ON o.UserId = u.UserId
INNER JOIN OrderItems oi ON o.OrderId = oi.OrderId
INNER JOIN Products p ON oi.ProductId = p.ProductId
INNER JOIN Categories c ON p.CategoryId = c.CategoryId
WHERE o.OrderDate >= DATEADD(MONTH, -12, GETDATE());
