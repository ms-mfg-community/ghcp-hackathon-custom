# GitHub Copilot Hackathon - Complete Training Package

## 📋 Overview

A comprehensive 3-day training program demonstrating GitHub Copilot capabilities across fundamentals, .NET development, and advanced testing. Includes 14 demonstration sessions with executable code, real-world patterns, and security best practices.

**Total Content:** 105+ KB PRD, 3000+ lines of Python, 2000+ lines of C#/.NET, 500+ lines of SQL, 1000+ lines of TypeScript/JavaScript test automation.

## 🎯 Quick Start

```bash
# Clone repository
git clone <repo-url>
cd ghcp-hackathon-custom

# View PRD
cat PRD-GitHub-Copilot-Hackathon.md

# Start Day 1 demonstrations
cd demos/day1
# Follow README files in each session directory

# Start Day 2 demonstrations
cd ../day2
npm install  # For future test frameworks

# Start Day 3 demonstrations
cd ../day3
npm install  # For Playwright tests
cd 03-playwright && npm install && npx playwright install
```

## 📚 Course Structure

### Day 1: GitHub Copilot Fundamentals (4 Sessions, 4 hours)

Learn core Copilot capabilities and coding patterns:

| Session | Title | Focus | Duration | Files |
|---------|-------|-------|----------|-------|
| 1.1 | Overview & Flow | Core capabilities, prompting, multi-turn patterns | 45 min | `example-functions.py`, `example-classes.cs`, `workflow-example-1.py` |
| 1.2 | Security | Best practices, validation, auth | 30 min | `secure-api-design.py` |
| 1.3 | Coding Agent | Complex system decomposition | 60 min | `agent-demo-1.py` |
| 1.4 | Custom Agents | Domain-specific agent modes | 45 min | `custom-agents-demo.py` |

**Break/Q&A:** 60 minutes distributed throughout day  
**Total:** 4 hours (3 hours instruction + 1 hour breaks/Q&A)

**Key Concepts:**
- ✅ Code completion across languages
- ✅ Multi-turn conversation patterns
- ✅ Security-first code generation
- ✅ Agentic workflows
- ✅ Domain-specific agent modes

**Run Day 1:**
```bash
cd demos/day1
# Each subdirectory contains executable code and README
# Review patterns and run locally with your IDE
```

### Day 2: .NET Development & Patterns (5 Sessions, 4 hours)

Master .NET development with Copilot:

| Session | Title | Focus | Duration | Files |
|---------|-------|-------|----------|-------|
| 2.1 | .NET Agentic | CRUD API, service layer | 45 min | `Product.cs`, `ProductService.cs` |
| 2.2 | Code Gen & Refactoring | LINQ, extensions, modernization | 45 min | READMEs with patterns |
| 2.3 | Unit Testing | xUnit, Moq, TDD | 60 min | `ProductServiceTests.cs` |
| 2.4 | Debugging & Docs | IDE debugging, XML docs | 20 min | README with workflow |
| 2.5 | MCP Overview | Advanced integration concepts | 30 min | `mcp-server-example.py` |

**Break/Q&A:** 60 minutes distributed throughout day  
**Total:** 4 hours (3 hours instruction + 1 hour breaks/Q&A)

**Key Concepts:**
- ✅ Entity/DTO patterns with validation
- ✅ Service layer architecture
- ✅ LINQ query generation
- ✅ Test-driven development with 11 test cases
- ✅ MCP (Model Context Protocol) server implementation
- ✅ Async/await patterns

**Run Day 2:**
```bash
cd demos/day2
# 01-dotnet-agentic: Product and service layer examples
# 04-unit-testing: Comprehensive test suite with Moq
# 06-mcp-overview: MCP server architecture
```

### Day 3: Databases & Testing (3 Sessions, 4 hours)

Advanced database and testing automation:

| Session | Title | Focus | Duration | Files |
|---------|-------|-------|----------|-------|
| 3.1 | Database Fundamentals & Operations | Schema design, normalization, CRUD, stored procedures | 60 min | `ecommerce-schema.sql`, `crud-operations.sql` |
| 3.2 | Playwright Testing | Browser automation, POM pattern, cross-browser | 90 min | `example-tests.spec.ts`, `playwright.config.ts` |
| 3.3 | Advanced Testing (Highlights) | Data-driven, E2E concepts, CI/CD patterns | 30 min | `advanced-tests.spec.ts` |

**Break/Q&A:** 60 minutes distributed throughout day  
**Total:** 4 hours (3 hours instruction + 1 hour breaks/Q&A)

**Key Concepts:**
- ✅ Normalized schema design (1NF, 2NF, 3NF, BCNF)
- ✅ Indexing strategies for performance
- ✅ Complex SQL queries with aggregation
- ✅ Stored procedures and transactions
- ✅ Page Object Model (POM) pattern
- ✅ Data-driven testing
- ✅ End-to-end workflows
- ✅ Security and accessibility testing

**Run Day 3:**
```bash
cd demos/day3

# Database demonstrations (SQL Server / Azure SQL)
cd 01-databases
# Review schema and run SQL scripts

# Database operations
cd ../02-db-operations
# Review CRUD patterns and complex queries

# Playwright setup
cd ../03-playwright
npm install
npm run test

# Advanced testing
cd ../04-advanced-testing
npm install
npm run test:all
```

## 📁 Directory Structure

```
ghcp-hackathon-custom/
├── PRD-GitHub-Copilot-Hackathon.md          # 20-section comprehensive PRD
├── README.md                                  # This file
│
├── demos/
│   ├── day1/                                 # Day 1: Fundamentals (4 delivery sessions)
│   │   ├── 01-copilot-overview/
│   │   ├── 02-copilot-flow/
│   │   ├── 03-security/
│   │   ├── 04-coding-agent/
│   │   └── 05-custom-agents/
│   │
│   ├── day2/                                 # Day 2: .NET Development (5 delivery sessions)
│   │   ├── 01-dotnet-agentic/
│   │   ├── 02-code-generation/
│   │   ├── 03-refactoring/
│   │   ├── 04-unit-testing/
│   │   ├── 05-debugging-docs/
│   │   └── 06-mcp-overview/
│   │
│   └── day3/                                 # Day 3: Databases & Testing (3 delivery sessions)
│       ├── 01-databases/
│       ├── 02-db-operations/
│       ├── 03-playwright/
│       └── 04-advanced-testing/
```

## 🔧 Prerequisites

### Required

- **Git** - Version control
- **GitHub Copilot** - VS Code extension or GitHub CLI
- **VS Code** - Code editor

### Day 1 & 2

- **Python 3.9+** - Python demonstrations
- **C# / .NET 6.0+** - .NET examples
- **Visual Studio 2022** or **VS Code** with C# extension

### Day 3

- **SQL Server** or **Azure SQL** - Database demonstrations
- **Node.js 18+** - Playwright and test frameworks
- **npm** - Package manager

### Installation Commands

```powershell
# Python
python --version
python -m venv venv
.\venv\Scripts\Activate.ps1

# .NET
dotnet --version
dotnet tool install -g dotnet-ef

# Node.js & npm
node --version
npm --version

# Playwright
npm install --save-dev playwright @playwright/test
npx playwright install
```

## 📖 PRD Document

The complete Product Requirements Document includes:

- **20 Comprehensive Sections**
  - Executive Summary & Problem Statement
  - Goals, Objectives, and Scope
  - User Stories and Functional Requirements
  - Non-Functional Requirements
  - Success Criteria and KPIs
  - Detailed Demonstration Sequence (12 effective sessions)
  - Setup & Execution Instructions
  - Verification & Validation Procedures
  - Resources and FAQ
  - Appendix with Best Practices

**Access PRD:**
```bash
cat PRD-GitHub-Copilot-Hackathon.md
```

## 🎓 Learning Path

### Recommended Sequence

**Day 1 (4 hours total)**
1. Start with **1.1 Overview & Flow** - Basic capabilities and prompting (45 min)
2. Review **1.2 Security** - Learn best practices (30 min)
3. Study **1.3 Agent** - Complex decomposition (60 min)
4. Explore **1.4 Custom Agents** - Specialized modes (45 min)
5. **Breaks/Q&A** - Throughout day (60 min)

**Day 2 (4 hours total)**
1. Begin with **2.1 .NET Agentic** - Architecture patterns (45 min)
2. Review **2.2 Code Gen & Refactoring** - Pattern generation and modernization (45 min)
3. Deep dive **2.3 Unit Testing** - Test-driven development (60 min)
4. Quick **2.4 Debugging & Docs** - IDE features (20 min)
5. Explore **2.5 MCP** - Advanced integration (30 min)
6. **Breaks/Q&A** - Throughout day (60 min)

**Day 3 (4 hours total)**
1. Start with **3.1 Database Fundamentals & Operations** - Schema and CRUD (60 min)
2. Comprehensive **3.2 Playwright** - Browser automation (90 min)
3. Quick **3.3 Advanced Testing** - Highlights and patterns (30 min)
4. **Breaks/Q&A** - Throughout day (60 min)

**Optional: Independent Practice** - Additional 1-2 hours after each day

## 🚀 Running Examples

### Python Examples (Day 1)

```bash
cd demos/day1/01-copilot-overview
python example-functions.py

cd ../02-copilot-flow
python workflow-example-1.py

cd ../03-security
python secure-api-design.py
```

### .NET Examples (Day 2)

```bash
cd demos/day2/01-dotnet-agentic
# Review Product.cs and ProductService.cs
# Integrate into your .NET project

cd ../04-unit-testing
# Run with your C# test runner
dotnet test ProductServiceTests.cs
```

### SQL Examples (Day 3.1-3.2)

```bash
cd demos/day3/01-databases
# Connect to SQL Server/Azure SQL
# Run ecommerce-schema.sql

cd ../02-db-operations
# Execute CRUD and aggregation queries
```

### Playwright Tests (Day 3.3-3.4)

```bash
cd demos/day3/03-playwright
npm install
npx playwright install
npm test

# Or with UI
npm run test:ui

# Headed mode (see browser)
npm run test:headed
```

## 📊 Success Criteria

✅ Complete all 14 demonstrations  
✅ Understand Copilot's capabilities and limitations  
✅ Apply patterns to your own projects  
✅ Generate secure, tested code consistently  
✅ Leverage Copilot for productivity improvement  

## 🔐 Security Highlights

All code examples follow security best practices:

- ✅ No hardcoded secrets
- ✅ Input validation and sanitization
- ✅ Password hashing (PBKDF2)
- ✅ Rate limiting and DDoS prevention
- ✅ SQL injection prevention
- ✅ XSS attack prevention
- ✅ CSRF token validation
- ✅ Role-based authorization
- ✅ Accessibility compliance

## 📚 Technologies & Frameworks

### Languages
- Python 3.9+
- C# / .NET 6.0+
- SQL (T-SQL / Azure SQL)
- TypeScript / JavaScript
- HTML / CSS

### Frameworks & Tools
- xUnit (testing)
- Moq (mocking)
- Entity Framework Core (ORM)
- Playwright (browser automation)
- MCP (Model Context Protocol)

### Databases
- Microsoft SQL Server
- Azure SQL Database

## 🤝 Contributing

Guidelines for enhancing the training material:

1. Follow existing code style
2. Add comments explaining Copilot suggestions
3. Include security considerations
4. Write comprehensive tests
5. Update documentation

## 📝 License

MIT License - See LICENSE file

## 🎉 Acknowledgments

Created with GitHub Copilot to demonstrate:
- Code generation capabilities
- Real-world architectural patterns
- Security-first development
- Test-driven development
- Enterprise best practices

## ❓ FAQ

**Q: Do I need GitHub Copilot Pro?**  
A: GitHub Copilot (any tier) works. Pro offers more features.

**Q: Can I run these offline?**  
A: Most examples can. Copilot features require internet.

**Q: What if I use VS Code instead of Visual Studio?**  
A: All examples work with VS Code + appropriate extensions.

**Q: How long does each session take?**  
A: Sessions range from 20-90 minutes. Each day totals 4 hours including breaks and Q&A (3 hours instruction + 1 hour breaks). Independent practice is optional and adds 1-2 hours.

**Q: Can I customize the content?**  
A: Absolutely! The code is provided as examples to adapt.

## 📞 Support

- 📖 Review PRD for detailed specifications
- 💬 Check README files in each session directory
- 🔍 Search code comments for explanations
- 📚 Refer to external documentation links

---

**Ready to master GitHub Copilot?**  
Start with Day 1 Session 1.1: Copilot Overview
