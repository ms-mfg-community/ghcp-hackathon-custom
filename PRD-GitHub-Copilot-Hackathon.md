# Product Requirements Document (PRD): GitHub Copilot Hackathon

## 1. Document Information

- **Version:** 1.0
- **Author(s):** GitHub Copilot, MS-MFG-Community
- **Date:** November 2, 2025
- **Status:** Active
- **Project:** GitHub Copilot Hackathon - Custom Training Program

---

## 2. Executive Summary

This PRD defines a comprehensive 3-day hackathon designed to provide hands-on training and practical demonstrations of GitHub Copilot capabilities across multiple domains. The program covers fundamental concepts, advanced agentic workflows, multi-language development practices, database operations, and testing methodologies. Each day builds upon foundational knowledge with progressive complexity, culminating in integrated practices across modern development scenarios.

---

## 3. Problem Statement

Development teams need practical, hands-on experience with AI-assisted coding tools to maximize productivity and code quality. Traditional training approaches often fail to bridge the gap between theoretical knowledge and practical application. This hackathon addresses the need for immersive, demonstration-rich learning experiences that showcase GitHub Copilot's capabilities in realistic development contexts.

---

## 4. Goals and Objectives

### 4.1 Primary Goals

- Provide comprehensive hands-on training on GitHub Copilot capabilities and workflows
- Demonstrate GitHub Copilot effectiveness across multiple programming languages and scenarios
- Enable developers to leverage agentic modes and custom agent workflows
- Build proficiency in modern development practices (testing, refactoring, debugging)
- Showcase best practices for database operations and data integration
- Introduce advanced testing frameworks and automation patterns

### 4.2 Secondary Goals

- Foster community engagement and knowledge sharing among developers
- Create reusable demonstration code and documentation
- Establish baseline competency standards for GitHub Copilot usage
- Build custom Model Context Protocol (MCP) integrations

---

## 5. Scope

### 5.1 In Scope

**Day 1: GitHub Copilot Fundamentals & Advanced Modes**
- GitHub Copilot Overview and core capabilities
- GitHub Copilot Flow workflows and best practices
- Security considerations and best practices
- GitHub Copilot Coding Agent introduction and demonstrations
- Custom Agent Modes and use case development

**Day 2: .NET Development & MCP Integration**
- .NET development with GitHub Copilot (Agentic Mode)
- Code generation and optimization techniques
- Code refactoring patterns and anti-patterns
- Unit testing frameworks and test-driven development
- Debugging workflows with IDE integration
- API documentation and inline comments
- Model Context Protocol (MCP) Overview

**Day 3: Data & Testing Operations**
- Database fundamentals (Azure SQL and MS SQL)
- Database schema design and optimization
- Playwright-based testing automation
- Cross-browser testing scenarios
- Independent practice and reinforcement

### 5.2 Out of Scope

- Advanced machine learning model training
- Custom GitHub Copilot fine-tuning
- Enterprise deployment and governance at scale
- Performance benchmarking across AI models
- Integration with non-Microsoft cloud platforms

---

## 6. User Stories & Use Cases

### UC-1: Junior Developer
"As a junior developer, I want hands-on guidance on using GitHub Copilot effectively so I can accelerate my learning curve and write better code from day one."

### UC-2: .NET Developer
"As a .NET developer, I want to leverage GitHub Copilot for code generation, refactoring, and testing so I can reduce boilerplate and focus on business logic."

### UC-3: DevOps Engineer
"As a DevOps engineer, I want to understand GitHub Copilot's capabilities for infrastructure and deployment scenarios so I can incorporate it into my workflows."

### UC-4: QA/Test Engineer
"As a QA engineer, I want to use GitHub Copilot to generate test cases and Playwright scripts so I can automate testing more efficiently."

### UC-5: Database Administrator
"As a DBA, I want to understand GitHub Copilot's capabilities for query optimization and schema design so I can improve database management."

---

## 7. Functional Requirements

### Day 1: Fundamentals & Advanced Modes

| ID | Requirement | Priority |
|---|---|---|
| FR-1.1 | Demonstrate GitHub Copilot core features: code completion, suggestions, and chat interface | HIGH |
| FR-1.2 | Explain GitHub Copilot Flow and integration with development workflows | HIGH |
| FR-1.3 | Cover security best practices, API key management, and data privacy considerations | HIGH |
| FR-1.4 | Introduce Coding Agent with multi-turn conversation and context awareness | HIGH |
| FR-1.5 | Demonstrate custom agent modes for domain-specific use cases | MEDIUM |
| FR-1.6 | Provide code samples showcasing each capability with executable demos | HIGH |

### Day 2: .NET Development & MCP

| ID | Requirement | Priority |
|---|---|---|
| FR-2.1 | Demonstrate .NET development using Agentic Mode for complex feature implementation | HIGH |
| FR-2.2 | Show code generation techniques for common .NET patterns (CRUD, API, ORM) | HIGH |
| FR-2.3 | Provide refactoring examples: performance improvements, design pattern implementation | HIGH |
| FR-2.4 | Demonstrate unit testing with xUnit framework and test-driven development | HIGH |
| FR-2.5 | Show debugging workflows using Visual Studio integration | MEDIUM |
| FR-2.6 | Provide documentation and comment generation examples | MEDIUM |
| FR-2.7 | Introduce Model Context Protocol (MCP) architecture and use cases | MEDIUM |

### Day 3: Data & Testing

| ID | Requirement | Priority |
|---|---|---|
| FR-3.1 | Demonstrate Azure SQL and MS SQL operations (schema design, queries, optimization) | HIGH |
| FR-3.2 | Provide SQL code generation examples for common scenarios | HIGH |
| FR-3.3 | Introduce Playwright for browser automation and cross-browser testing | HIGH |
| FR-3.4 | Demonstrate test case generation using GitHub Copilot | MEDIUM |
| FR-3.5 | Show Playwright integration with CI/CD pipelines | MEDIUM |

---

## 8. Non-Functional Requirements

| Attribute | Requirement |
|---|---|
| **Accessibility** | All demonstrations must be runnable on Windows 11+ with PowerShell 7+ |
| **Reproducibility** | Code samples must be executable in any development environment with documented prerequisites |
| **Documentation** | All code includes inline comments explaining GitHub Copilot suggestions and rationale |
| **Performance** | Demonstrations complete within 30 seconds on modern hardware (8GB+ RAM) |
| **Maintainability** | Code follows PEP 8 (Python), C# conventions, and SQL best practices |
| **Scalability** | Demonstrations scale from single-file scripts to multi-project solutions |

---

## 9. Assumptions & Dependencies

### Assumptions

- Participants have GitHub Copilot subscription or access (Pro, Business, or Enterprise)
- Development environment includes VS Code or Visual Studio 2022+
- PowerShell 7+ is installed on Windows machines
- .NET 6.0+ SDK is available
- SQL Server or Azure SQL is accessible for database demonstrations
- Node.js 18+ is installed for Playwright demonstrations

### Dependencies

- GitHub Copilot API/VSCode Extension
- Visual Studio Code and/or Visual Studio 2022
- .NET 6.0+ SDK
- Python 3.9+ with virtual environments
- Playwright testing framework
- xUnit testing framework
- Azure CLI or SQL Server Management Studio
- PowerShell 7+

---

## 10. Success Criteria & KPIs

| Criterion | Target | Measurement |
|---|---|---|
| Participant Understanding | 90%+ understanding of core concepts | Post-session survey scores |
| Code Sample Execution | 100% of samples run without errors | Automated test execution |
| Documentation Quality | All code samples fully documented | Code review checklist |
| Time Efficiency | Each demo completes in <30 seconds | Stopwatch measurement |
| Engagement Level | >80% active participation | Session attendance and Q&A |
| Knowledge Retention | >75% pass comprehensive quiz | Assessment results |

---

## 11. Milestones & Timeline

| Phase | Duration | Deliverables |
|---|---|---|
| **Day 1: Fundamentals** | 4 hours (excluding independent practice) | Code samples, demonstrations, use case documentation |
| **Day 2: .NET & MCP** | 4 hours (excluding independent practice) | .NET project templates, test suites, MCP integration examples |
| **Day 3: Data & Testing** | 4 hours (excluding independent practice) | Database scripts, Playwright test suites, CI/CD configurations |
| **Follow-up** | Ongoing | Code repositories, documentation, community support |

---

## 12. Demonstration Sequence

### Day 1: GitHub Copilot Fundamentals & Advanced Modes

#### Session 1.1: GitHub Copilot Overview & Core Capabilities
**Duration:** 45 minutes

- Interactive walkthrough of GitHub Copilot interface
- Live demonstrations of code completion and suggestions
- Explanation of model capabilities and limitations
- Multi-turn conversation patterns and effective prompting
- **Demo Code Location:** `demos/day1/01-copilot-overview/` and `demos/day1/02-copilot-flow/`

#### Session 1.2: Security Best Practices
**Duration:** 30 minutes

- API key and credential management
- Data privacy and compliance considerations
- Secure code generation practices (focused on key patterns)
- **Demo Code Location:** `demos/day1/03-security/`

#### Session 1.3: GitHub Copilot Coding Agent
**Duration:** 60 minutes

- Introduction to agentic workflows
- Multi-turn agent demonstrations
- Building complex solutions with agents
- **Demo Code Location:** `demos/day1/04-coding-agent/`

#### Session 1.4: Custom Agent Modes
**Duration:** 45 minutes

- Domain-specific agent configuration
- Use case development and integration patterns
- Focused demonstration of one comprehensive agent example
- **Demo Code Location:** `demos/day1/05-custom-agents/`

**Break/Q&A:** 60 minutes distributed throughout day

**Total Day 1 Session Time:** 4 hours (3 hours instruction + 1 hour breaks/Q&A)

#### Independent Practice (Optional - After Hours)
**Duration:** 1-2 hours

- Guided exercises building custom workflows
- Peer code review sessions

---

### Day 2: .NET Development & MCP Integration

#### Session 2.1: .NET Development with Agentic Mode
**Duration:** 45 minutes

- Building CRUD APIs with agentic assistance (focused on one entity)
- Entity Framework integration
- Dependency injection patterns
- **Demo Code Location:** `demos/day2/01-dotnet-agentic/`

#### Session 2.2: Code Generation & Refactoring
**Duration:** 45 minutes

- Generating common .NET patterns (LINQ, Repository, extensions)
- Legacy code modernization and refactoring patterns
- Design pattern implementation highlights
- **Demo Code Location:** `demos/day2/02-code-generation/` and `demos/day2/03-refactoring/`

#### Session 2.3: Unit Testing & TDD
**Duration:** 60 minutes

- xUnit framework with GitHub Copilot
- Test-driven development workflows
- Mock objects and assertions
- **Demo Code Location:** `demos/day2/04-unit-testing/`

#### Session 2.4: Debugging & Documentation
**Duration:** 20 minutes

- IDE debugging integration (brief demo)
- Using GitHub Copilot for diagnostics
- Auto-generated documentation and comments
- **Demo Code Location:** `demos/day2/05-debugging-docs/`

#### Session 2.5: MCP Overview
**Duration:** 30 minutes

- Model Context Protocol architecture (conceptual overview)
- Integration use cases and patterns
- Simplified MCP server example walkthrough
- **Demo Code Location:** `demos/day2/06-mcp-overview/`

**Break/Q&A:** 60 minutes distributed throughout day

**Total Day 2 Session Time:** 4 hours (3 hours instruction + 1 hour breaks/Q&A)

#### Independent Practice (Optional - After Hours)
**Duration:** 1-2 hours

- Build a complete .NET microservice
- Full test coverage implementation

---

### Day 3: Databases & Testing

#### Session 3.1: Database Fundamentals & Operations
**Duration:** 60 minutes

- Schema design best practices and normalization
- Indexing strategies and query optimization
- CRUD operations generation with GitHub Copilot
- Stored procedures and complex queries
- **Demo Code Location:** `demos/day3/01-databases/` and `demos/day3/02-db-operations/`

#### Session 3.2: Playwright Testing Framework
**Duration:** 90 minutes

- Playwright setup and configuration
- Automation script generation with GitHub Copilot
- Page Object Model pattern implementation
- Cross-browser testing scenarios
- **Demo Code Location:** `demos/day3/03-playwright/`

#### Session 3.3: Advanced Test Automation (Highlights)
**Duration:** 30 minutes

- Data-driven testing concepts
- CI/CD pipeline integration patterns
- Security and accessibility testing overview
- **Demo Code Location:** `demos/day3/04-advanced-testing/`

**Break/Q&A:** 60 minutes distributed throughout day

**Total Day 3 Session Time:** 4 hours (3 hours instruction + 1 hour breaks/Q&A)

#### Independent Practice (Optional - After Hours)
**Duration:** 1-2 hours

- Build end-to-end test suites
- Database and UI integration testing

---

## 13. Demonstration Code Structure

All demonstration code follows this structure:

```
demos/
├── day1/
│   ├── 01-copilot-overview/
│   │   ├── README.md
│   │   ├── example-*.py
│   │   └── example-*.cs
│   ├── 02-copilot-flow/
│   ├── 03-security/
│   ├── 04-coding-agent/
│   └── 05-custom-agents/
├── day2/
│   ├── 01-dotnet-agentic/
│   │   ├── DemoProject.sln
│   │   ├── DemoProject.csproj
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── Controllers/
│   │   └── Tests/
│   ├── 02-code-generation/
│   ├── 03-refactoring/
│   ├── 04-unit-testing/
│   ├── 05-debugging-docs/
│   └── 06-mcp-overview/
├── day3/
│   ├── 01-databases/
│   │   ├── schemas.sql
│   │   ├── queries.sql
│   │   └── optimization-examples.sql
│   ├── 02-db-operations/
│   ├── 03-playwright/
│   │   ├── package.json
│   │   ├── tests/
│   │   └── page-objects/
│   └── 04-advanced-testing/
└── README.md
```

---

## 14. Setup & Execution Instructions

### Prerequisites

1. **GitHub Copilot Access**
   ```powershell
   # Verify GitHub Copilot extension is installed
   # VS Code: Extensions view → Search "GitHub Copilot"
   # Visual Studio: Extensions → Manage Extensions → Search "GitHub Copilot"
   ```

2. **Development Tools**
   ```powershell
   # .NET SDK
   dotnet --version  # Should be 6.0 or higher

   # PowerShell
   $PSVersionTable.PSVersion  # Should be 7.0 or higher

   # Python (for Day 1 demos)
   python --version  # Should be 3.9 or higher
   ```

3. **Database Access** (for Day 3)
   ```powershell
   # Azure SQL or MS SQL Server must be accessible
   # Connection strings configured in appsettings.json or environment variables
   ```

4. **Node.js** (for Playwright)
   ```powershell
   node --version  # Should be 18 or higher
   npm --version   # Should be 9 or higher
   ```

### Day 1 Setup

```powershell
# Navigate to Day 1 demonstrations
cd demos/day1

# Run Day 1 overview demonstration
python 01-copilot-overview/example-intro.py

# Run security best practices demo
dotnet run --project 03-security/SecurityDemo.csproj
```

### Day 2 Setup

```powershell
# Navigate to Day 2 demonstrations
cd demos/day2

# Restore .NET dependencies
dotnet restore 01-dotnet-agentic/DemoProject.sln

# Build and run .NET demonstrations
dotnet build 01-dotnet-agentic/DemoProject.sln
dotnet run --project 01-dotnet-agentic/DemoProject.csproj

# Run unit tests
dotnet test 04-unit-testing/DemoProject.Tests.csproj
```

### Day 3 Setup

```powershell
# Navigate to Day 3 demonstrations
cd demos/day3

# Database setup (Azure SQL or MS SQL)
# Execute schemas.sql and seed scripts via SQL Management Studio or Azure Portal

# Playwright setup
cd 03-playwright
npm install
npm test

# Advanced testing
cd ../04-advanced-testing
npm install
npm run test:all
```

---

## 15. Verification & Validation

### Automated Verification

```powershell
# Day 1: Run all Python examples
Get-ChildItem "demos/day1" -Recurse -Filter "*.py" | ForEach-Object {
    Write-Host "Running: $($_.FullName)"
    python $_.FullName
}

# Day 2: Build and test all .NET projects
Get-ChildItem "demos/day2" -Recurse -Filter "*.sln" | ForEach-Object {
    Write-Host "Building: $($_.FullName)"
    dotnet build $_.FullName
    dotnet test $_.FullName
}

# Day 3: Run Playwright tests
cd demos/day3/03-playwright
npm run test:verify
```

### Success Indicators

- [ ] All code samples compile/run without errors
- [ ] Unit tests achieve >80% code coverage
- [ ] Documentation is complete and accurate
- [ ] Performance benchmarks are met (all demos <30 seconds)
- [ ] Participant feedback survey shows >85% satisfaction

---

## 16. Key Takeaways

### For Participants

1. **GitHub Copilot fundamentals** provide foundation for all advanced techniques
2. **Agentic workflows** enable complex multi-step solution development
3. **Security practices** are essential when using AI-assisted code generation
4. **Testing and documentation** should be primary focuses of Copilot usage
5. **Database operations** benefit significantly from GitHub Copilot assistance
6. **Modern testing frameworks** integrate seamlessly with GitHub Copilot
7. **Custom agent modes** unlock domain-specific productivity gains

### For Organizations

1. **Productivity gains** of 30-50% are achievable with proper training
2. **Code quality** improves with AI-assisted refactoring and testing
3. **Team onboarding** accelerates with GitHub Copilot support
4. **Technical debt** reduction through systematic refactoring
5. **Knowledge sharing** improves through collaborative demonstrations

---

## 17. Frequently Asked Questions

### Q: Do I need a GitHub Copilot subscription?
**A:** Yes, GitHub Copilot requires an active subscription (Individual Pro, Business, or Enterprise). Educational and open-source licenses may also apply.

### Q: What if I only know one programming language?
**A:** Participants should focus on sessions relevant to their expertise. All sessions build transferable skills applicable across languages.

### Q: How do I access the demonstration code?
**A:** Code is available in the `demos/` directory within this repository. Clone the repository and follow setup instructions in each demo's README.

### Q: Can I use GitHub Copilot for security-sensitive code?
**A:** Yes, with proper precautions. Never include API keys, secrets, or sensitive data in prompts. Use the security best practices from Session 1.3.

### Q: How is GitHub Copilot's performance measured?
**A:** Success is measured through increased developer velocity, code quality improvements, and user satisfaction surveys.

---

## 18. Resources & References

### Official Documentation
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [GitHub Copilot Best Practices](https://github.blog/ai-and-ml/)
- [Visual Studio GitHub Copilot Integration](https://learn.microsoft.com/en-us/visualstudio/)

### Development Frameworks
- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [xUnit Testing Framework](https://xunit.net/)
- [Playwright Testing](https://playwright.dev/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/)

### Database Resources
- [Azure SQL Documentation](https://learn.microsoft.com/en-us/azure/azure-sql/)
- [Microsoft SQL Server Documentation](https://learn.microsoft.com/en-us/sql/)
- [SQL Query Optimization Guide](https://learn.microsoft.com/en-us/sql/relational-databases/query-processing-architecture-guide)

---

## 19. Contact & Support

- **Hackathon Lead:** GitHub Copilot (AI Assistant)
- **Community:** MS-MFG-Community
- **Repository:** [ghcp-hackathon-custom](https://github.com/ms-mfg-community/ghcp-hackathon-custom)
- **Issues:** GitHub Issues in this repository
- **Questions:** Use GitHub Discussions feature

---

## 20. Appendix: Copilot-Assisted Development Tips

### Effective Prompting Techniques

1. **Be Specific:** "Generate a async method to fetch user data from Azure SQL" → Better results than "Get data"
2. **Provide Context:** Include class names, existing code patterns, and constraints
3. **Show Examples:** "Similar to this pattern: [example code]"
4. **Iterate:** Use multi-turn conversations to refine suggestions
5. **Ask for Explanations:** "Explain this code" → Better understanding

### Common Pitfalls to Avoid

1. ❌ Don't include passwords or API keys in prompts
2. ❌ Don't assume generated code is always optimal
3. ❌ Don't skip code review and testing
4. ❌ Don't use Copilot as a replacement for learning fundamentals
5. ❌ Don't ignore security and performance implications

### Best Practices Checklist

- ✅ Review all generated code before committing
- ✅ Test generated code thoroughly
- ✅ Maintain code ownership and accountability
- ✅ Document why Copilot was used for specific code sections
- ✅ Keep security and privacy as top priorities
- ✅ Provide feedback to help improve Copilot suggestions

---

**Document Version:** 1.0  
**Last Updated:** November 2, 2025  
**Next Review Date:** November 16, 2025
