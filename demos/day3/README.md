# Day 3: Databases & Testing

## Overview

Day 3 focuses on database operations and advanced testing:
- Azure SQL and MS SQL database work
- Database schema design and optimization
- SQL query generation with GitHub Copilot
- Playwright-based browser automation
- End-to-end testing patterns

## Sessions

### 3.1: Database Fundamentals
**Focus:** Schema design, normalization, and optimization
- Entity relationship modeling
- Indexing strategies
- Query performance analysis
- Best practices

### 3.2: Database Operations & Scripting
**Focus:** SQL generation and data operations
- CRUD operation generation
- Complex query construction
- Stored procedures
- Data migration scripts

### 3.3: Playwright Testing Framework
**Focus:** Browser automation and testing
- Selector strategies
- Test case generation
- Page object model
- Cross-browser testing

### 3.4: Advanced Test Automation
**Focus:** Integration and CI/CD patterns
- End-to-end testing
- Data-driven testing
- CI/CD pipeline integration
- Test reporting

## Demonstration Files

Each session includes:
- `README.md` - Session overview
- Code examples and scripts
- Configuration files
- Setup and execution guides

## Prerequisites

- SQL Server or Azure SQL
- Node.js 18+ (for Playwright)
- Playwright browser binaries
- Database tools (SSMS or Azure Data Studio)
- GitHub Copilot extension

## Running Day 3 Demonstrations

```powershell
# Database demonstrations
cd demos/day3/01-databases
# Review and execute SQL scripts

# Database operations
cd ../02-db-operations
# Review generated CRUD operations

# Playwright setup
cd ../03-playwright
npm install
npm test

# Advanced testing
cd ../04-advanced-testing
npm install
npm run test:all
```
