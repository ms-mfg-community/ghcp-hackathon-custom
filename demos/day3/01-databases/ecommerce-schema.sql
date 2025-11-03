-- Day 3.1 Demo: E-Commerce Database Schema
-- Generated with GitHub Copilot for database design
-- Demonstrates normalized schema with proper relationships and indexes

-- ============================================================
-- 1. BASIC TABLES (First Normal Form)
-- ============================================================

CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Country NVARCHAR(50),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18, 2) NOT NULL,
    Stock INT NOT NULL,
    CategoryId INT NOT NULL,
    Sku NVARCHAR(50) UNIQUE,
    Rating DECIMAL(3, 2) DEFAULT 0,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

-- ============================================================
-- 2. NORMALIZED TABLES (Second and Third Normal Form)
-- ============================================================

CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    ShippingDate DATETIME,
    DeliveryDate DATETIME,
    Status NVARCHAR(20) DEFAULT 'Pending', -- Pending, Processing, Shipped, Delivered
    TotalAmount DECIMAL(18, 2) NOT NULL,
    ShippingAddress NVARCHAR(MAX),
    BillingAddress NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(18, 2) NOT NULL,
    Discount DECIMAL(5, 2) DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE Reviews (
    ReviewId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    UserId INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Title NVARCHAR(200),
    Comment NVARCHAR(MAX),
    HelpfulCount INT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE Inventory (
    InventoryId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL UNIQUE,
    WarehouseLocation NVARCHAR(100),
    ReorderLevel INT,
    ReorderQuantity INT,
    LastRestockDate DATETIME,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- ============================================================
-- 3. INDEXES FOR PERFORMANCE
-- ============================================================

-- Primary Key indexes (automatic, but listed for reference)
-- CREATE INDEX PK_Users ON Users(UserId);
-- CREATE INDEX PK_Products ON Products(ProductId);
-- CREATE INDEX PK_Orders ON Orders(OrderId);
-- CREATE INDEX PK_OrderItems ON OrderItems(OrderItemId);

-- Foreign Key indexes
CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE INDEX IX_Orders_UserId ON Orders(UserId);
CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);
CREATE INDEX IX_Reviews_ProductId ON Reviews(ProductId);
CREATE INDEX IX_Reviews_UserId ON Reviews(UserId);

-- Search indexes
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Products_Sku ON Products(Sku);
CREATE INDEX IX_Products_Category_Active ON Products(CategoryId, IsActive);

-- Time-based indexes
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IX_Orders_Status ON Orders(Status);
CREATE INDEX IX_OrderItems_CreatedAt ON OrderItems(CreatedAt);

-- Composite indexes for common queries
CREATE INDEX IX_Orders_User_Date ON Orders(UserId, OrderDate DESC);
CREATE INDEX IX_Products_Category_Price ON Products(CategoryId, Price);

-- ============================================================
-- 4. COMMON QUERIES WITH COPILOT ASSISTANCE
-- ============================================================

-- Query 1: Get user order history (optimized with index)
SELECT 
    o.OrderId,
    o.OrderDate,
    o.TotalAmount,
    o.Status,
    COUNT(oi.OrderItemId) as ItemCount
FROM Orders o
LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
WHERE o.UserId = 1
GROUP BY o.OrderId, o.OrderDate, o.TotalAmount, o.Status
ORDER BY o.OrderDate DESC;

-- Query 2: Top selling products (uses composite index)
SELECT TOP 10
    p.ProductId,
    p.Name,
    c.Name as Category,
    SUM(oi.Quantity) as TotalSold,
    SUM(oi.Quantity * oi.UnitPrice) as Revenue
FROM Products p
INNER JOIN OrderItems oi ON p.ProductId = oi.ProductId
INNER JOIN Categories c ON p.CategoryId = c.CategoryId
GROUP BY p.ProductId, p.Name, c.Name
ORDER BY TotalSold DESC;

-- Query 3: Low stock products with reorder info
SELECT 
    p.ProductId,
    p.Name,
    p.Stock,
    i.ReorderLevel,
    i.ReorderQuantity,
    i.WarehouseLocation
FROM Products p
INNER JOIN Inventory i ON p.ProductId = i.ProductId
WHERE p.Stock <= i.ReorderLevel
ORDER BY p.Stock ASC;

-- ============================================================
-- 5. SAMPLE DATA INSERTION
-- ============================================================

INSERT INTO Categories (Name, Description) VALUES
('Electronics', 'Electronic devices and accessories'),
('Books', 'Physical and digital books'),
('Clothing', 'Apparel and fashion items'),
('Home & Garden', 'Home and garden products');

INSERT INTO Users (Username, Email, FirstName, LastName, Country) VALUES
('johndoe', 'john@example.com', 'John', 'Doe', 'USA'),
('janedoe', 'jane@example.com', 'Jane', 'Doe', 'Canada'),
('bobsmith', 'bob@example.com', 'Bob', 'Smith', 'USA');

INSERT INTO Products (Name, Description, Price, Stock, CategoryId, Sku, Rating) VALUES
('Laptop Pro', 'High-performance laptop', 1299.99, 15, 1, 'LAP-PRO-001', 4.8),
('Wireless Mouse', 'Ergonomic wireless mouse', 29.99, 100, 1, 'MSE-WRL-001', 4.5),
('Programming Guide', 'Learn programming basics', 39.99, 50, 2, 'BOK-PRG-001', 4.7),
('Cotton T-Shirt', 'Comfortable cotton shirt', 19.99, 200, 3, 'TSH-COT-001', 4.3);
