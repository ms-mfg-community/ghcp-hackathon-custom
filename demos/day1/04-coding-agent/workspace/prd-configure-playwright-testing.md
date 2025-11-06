# Product Requirements Document (PRD): Playwright Testing Configuration

## Document Information

- **Version:** 1.0
- **Author(s):** GitHub Copilot
- **Date:** November 5, 2025
- **Status:** Draft
- **Related Documents:** prd-migrate-to-azure-with-azd.md

## Executive Summary

This document defines the requirements for implementing Playwright end-to-end testing for the Calculator Blazor Server application. The solution will provide comprehensive UI testing capabilities for local development and Azure-hosted environments, supporting cross-browser testing and integration with CI/CD pipelines.

## Problem Statement

The Calculator application currently lacks automated UI testing to verify the user interface, user interactions, and end-to-end workflows. Manual testing is time-consuming, inconsistent, and does not scale effectively. A robust automated UI testing framework is needed to ensure application quality and reliability across different browsers and environments.

## Goals and Objectives

- Implement Playwright testing framework for the Calculator Blazor Server application
- Enable local UI testing against the development server (https://localhost:5001)
- Support cross-browser testing (Chromium, Firefox, WebKit)
- Test actual Blazor Server SignalR circuit functionality
- Provide clear test organization and reporting
- Enable integration with CI/CD pipelines
- Support both local development and Azure-hosted testing scenarios

## Scope

### In Scope

- Playwright framework installation and configuration
- Test project structure and organization
- PowerShell automation scripts for test execution
- Cross-browser test configuration (Chromium, Firefox, WebKit)
- Sequential test execution to prevent race conditions
- Basic calculator operation tests (addition, subtraction, multiplication, division)
- Blazor Server circuit testing (actual server-side rendering validation)
- Test reporting and output configuration
- npm scripts for common testing tasks
- Integration with existing project structure

### Out of Scope

- Performance testing and load testing
- Visual regression testing (screenshot comparisons)
- Mobile browser testing
- Integration with third-party test management tools
- Advanced accessibility testing beyond basic ARIA checks
- Database integration testing (covered by separate xUnit tests)
- API testing (covered by separate integration tests)

## User Stories / Use Cases

- **As a developer**, I want to run UI tests locally before committing code to ensure my changes don't break existing functionality.
- **As a QA engineer**, I want to execute tests across multiple browsers to verify cross-browser compatibility.
- **As a DevOps engineer**, I want automated UI tests integrated into CI/CD pipelines to catch regressions early.
- **As a team lead**, I want comprehensive test reports to understand test coverage and identify failing scenarios.
- **As a developer**, I want to test Blazor Server-specific features like SignalR circuits to ensure real-time updates work correctly.

## Functional Requirements

| Requirement ID | Description |
|---|---|
| FR-1 | Initialize Playwright using `npm init playwright@latest` with TypeScript support |
| FR-2 | Configure base URL to `https://localhost:5001` for local testing |
| FR-3 | Support Chromium, Firefox, and WebKit browsers |
| FR-4 | Execute tests sequentially with `fullyParallel: false` to prevent race conditions |
| FR-5 | Create `tests/` directory with organized test files |
| FR-6 | Implement calculator operation tests (add, subtract, multiply, divide) |
| FR-7 | Validate Blazor Server SignalR circuit functionality |
| FR-8 | Create `Start-BlazorAndTest.ps1` script to automate server startup and test execution |
| FR-9 | Configure test timeout of 30 seconds per test |
| FR-10 | Enable test retries (2 retries on failure) |
| FR-11 | Generate HTML test reports in `playwright-report/` directory |
| FR-12 | Configure trace collection on first retry |
| FR-13 | Create npm scripts: `test`, `test:headed`, `test:debug`, `test:report` |
| FR-14 | Validate SSL certificate acceptance for localhost HTTPS |

## Non-Functional Requirements

- **Performance:** Tests should execute in under 5 minutes for the full suite
- **Reliability:** Tests must be deterministic and produce consistent results
- **Maintainability:** Test code should follow TypeScript best practices and be easy to understand
- **Portability:** Tests must run on Windows, macOS, and Linux (via WSL)
- **Usability:** Clear error messages and test reports for debugging failures
- **Scalability:** Framework should support adding new test cases without major refactoring

## Technical Architecture

### Directory Structure

```
calculator-xunit-testing/
├── tests/
│   ├── calculator-operations.spec.ts
│   ├── blazor-circuit.spec.ts
│   └── smoke-tests.spec.ts
├── playwright.config.ts
├── package.json
├── Start-BlazorAndTest.ps1
└── playwright-report/ (generated)
```

### Configuration Files

**playwright.config.ts:**
- Base URL: `https://localhost:5001`
- Browsers: Chromium, Firefox, WebKit
- Sequential execution: `fullyParallel: false`
- Timeout: 30000ms
- Retries: 2
- Trace: `'on-first-retry'`
- Reporter: `'html'`
- Ignore HTTPS errors: `true` (localhost only)

**package.json:**
- Scripts: test, test:headed, test:debug, test:report
- Dependencies: @playwright/test
- Dev dependencies: TypeScript, Node types

### Test Implementation

**Test Categories:**
1. **Calculator Operations** (calculator-operations.spec.ts)
   - Basic arithmetic (addition, subtraction, multiplication, division)
   - Edge cases (divide by zero, large numbers)
   - Input validation

2. **Blazor Circuit Tests** (blazor-circuit.spec.ts)
   - SignalR connection establishment
   - Server-side state management
   - Real-time UI updates

3. **Smoke Tests** (smoke-tests.spec.ts)
   - Application loads successfully
   - Health endpoint responds
   - Critical UI elements visible

## Implementation Plan

### Phase 1: Setup and Configuration (Day 1)

1. Install Playwright framework
   ```powershell
   cd calculator-xunit-testing
   npm init playwright@latest
   ```

2. Configure playwright.config.ts
   - Set base URL
   - Configure browsers
   - Set sequential execution
   - Configure retries and timeouts

3. Create package.json scripts

### Phase 2: Test Development (Day 2-3)

1. Create `tests/smoke-tests.spec.ts`
   - Application load test
   - Health check test
   - UI element visibility tests

2. Create `tests/calculator-operations.spec.ts`
   - Addition tests
   - Subtraction tests
   - Multiplication tests
   - Division tests
   - Edge case tests

3. Create `tests/blazor-circuit.spec.ts`
   - SignalR connection test
   - State persistence test
   - Real-time update test

### Phase 3: Automation (Day 4)

1. Create `Start-BlazorAndTest.ps1`
   - Build application
   - Start Blazor Server in background
   - Wait for health check
   - Execute Playwright tests
   - Capture results
   - Stop server
   - Exit with appropriate code

2. Test automation script

### Phase 4: CI/CD Integration (Day 5)

1. Document GitHub Actions integration
2. Document Azure DevOps integration
3. Test in CI/CD environment

## Automation Script Specification

### Start-BlazorAndTest.ps1

**Purpose:** Automate the complete test execution workflow

**Steps:**
1. Build the Calculator.Web project
2. Start the application in background
3. Wait for health endpoint to respond (max 30 seconds)
4. Execute Playwright tests
5. Capture test results
6. Stop the application process
7. Exit with test result code

**Parameters:**
- `-Configuration` (default: "Debug")
- `-Headed` (switch for headed browser mode)
- `-Browser` (default: "all", options: "chromium", "firefox", "webkit")

**Example Usage:**
```powershell
# Run all tests headless
.\Start-BlazorAndTest.ps1

# Run tests in headed mode
.\Start-BlazorAndTest.ps1 -Headed

# Run only Chromium tests
.\Start-BlazorAndTest.ps1 -Browser chromium
```

## Test Execution Strategy

### Local Development
- Developer runs tests before committing
- Sequential execution to prevent race conditions
- Headed mode for debugging
- Fast feedback loop

### CI/CD Pipeline
- Automated execution on pull requests
- Headless mode for efficiency
- Parallel jobs for different browsers (separate pipelines)
- Test reports uploaded as artifacts

### Azure Playwright Testing Workspace (Future)
- Cloud-based test execution
- Scalable parallel execution
- Cross-region testing
- Integration with Azure DevOps

## Success Criteria / KPIs

- ✅ Playwright framework successfully installed and configured
- ✅ All three browsers (Chromium, Firefox, WebKit) pass tests
- ✅ Test execution time < 5 minutes for full suite
- ✅ Test success rate > 95% (allowing for environmental flakiness)
- ✅ Blazor Server circuit functionality validated
- ✅ Automation script successfully starts/stops server and runs tests
- ✅ Clear, actionable test reports generated
- ✅ Zero false positives (flaky tests eliminated or properly handled)

## Assumptions and Dependencies

### Assumptions
- .NET 9.0 SDK installed on test machine
- Node.js and npm installed (version 18 or higher)
- Calculator.Web project builds successfully
- HTTPS certificate trusted for localhost
- Port 5001 available for Blazor Server
- PowerShell available for automation scripts

### Dependencies
- @playwright/test (npm package)
- Calculator.Web project (Blazor Server application)
- .NET 9.0 runtime
- Operating system browser dependencies (Chromium, Firefox, WebKit)

## Risks and Mitigation

| Risk | Impact | Probability | Mitigation |
|---|---|---|---|
| Flaky tests due to timing issues | High | Medium | Use explicit waits, sequential execution, increase timeouts |
| HTTPS certificate issues on localhost | Medium | Low | Configure `ignoreHTTPSErrors: true` for localhost |
| SignalR connection failures | High | Low | Add retry logic, verify network configuration |
| Browser installation failures | Medium | Low | Use Playwright's automatic browser installation |
| Port conflicts (5001 already in use) | Low | Medium | Check port availability in automation script |
| Test failures masking real issues | High | Low | Implement proper error reporting and logging |

## Milestones & Timeline

- **Day 1:** Playwright installation and configuration ✅
- **Day 2:** Smoke tests and basic operation tests implemented
- **Day 3:** Blazor circuit tests and edge cases implemented
- **Day 4:** Automation script created and tested
- **Day 5:** CI/CD integration documentation and validation
- **Day 6:** Final review and deployment to CI/CD pipeline

## Usage Instructions (Demonstration Sequence)

### Initial Setup

1. Navigate to project directory
   ```powershell
   cd demos\day1\04-coding-agent\workspace\calculator-xunit-testing
   ```

2. Install Playwright
   ```powershell
   npm init playwright@latest
   ```
   - Select TypeScript
   - Use default test directory: `tests`
   - Add GitHub Actions workflow: No (manual for now)

3. Install browser binaries
   ```powershell
   npx playwright install
   ```

### Manual Test Execution

1. Start Blazor Server manually
   ```powershell
   cd calculator.web
   dotnet run
   ```

2. In separate terminal, run tests
   ```powershell
   cd ..
   npm test
   ```

3. View results
   ```powershell
   npm run test:report
   ```

### Automated Test Execution

1. Run automation script
   ```powershell
   .\Start-BlazorAndTest.ps1
   ```

2. Review test results in console

3. Open HTML report if needed
   ```powershell
   npm run test:report
   ```

## Configuration Details

### playwright.config.ts

```typescript
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  fullyParallel: false, // Sequential execution
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 2,
  workers: 1, // Single worker for sequential execution
  reporter: 'html',
  timeout: 30000,
  
  use: {
    baseURL: 'https://localhost:5001',
    trace: 'on-first-retry',
    ignoreHTTPSErrors: true, // For localhost development
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
    {
      name: 'firefox',
      use: { ...devices['Desktop Firefox'] },
    },
    {
      name: 'webkit',
      use: { ...devices['Desktop Safari'] },
    },
  ],

  webServer: undefined, // Manual server management via script
});
```

### package.json Scripts

```json
{
  "scripts": {
    "test": "playwright test",
    "test:headed": "playwright test --headed",
    "test:debug": "playwright test --debug",
    "test:report": "playwright show-report",
    "test:chromium": "playwright test --project=chromium",
    "test:firefox": "playwright test --project=firefox",
    "test:webkit": "playwright test --project=webkit"
  }
}
```

## Example Test Structure

### tests/calculator-operations.spec.ts

```typescript
import { test, expect } from '@playwright/test';

test.describe('Calculator Operations', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/');
    await expect(page.locator('h1')).toContainText('Calculator');
  });

  test('should perform addition', async ({ page }) => {
    // Test implementation
  });

  test('should perform subtraction', async ({ page }) => {
    // Test implementation
  });

  test('should handle divide by zero', async ({ page }) => {
    // Test implementation
  });
});
```

## Integration with Existing Testing

### Relationship to xUnit Tests
- **xUnit Tests:** Unit and integration tests for business logic and database operations
- **Playwright Tests:** End-to-end UI tests for user workflows
- **Complementary Coverage:** Both frameworks provide different layers of testing

### Test Execution Order
1. Unit tests (xUnit) - Fast, frequent
2. Integration tests (xUnit) - Medium speed, pre-commit
3. UI tests (Playwright) - Slower, pre-merge/CI/CD

## Maintenance and Support

### Test Maintenance
- Review and update tests when UI changes
- Add new tests for new features
- Refactor common test utilities into helper functions
- Keep Playwright and browser versions updated

### Troubleshooting Guide
- **Test timeouts:** Increase timeout in playwright.config.ts
- **Certificate errors:** Verify `ignoreHTTPSErrors: true` for localhost
- **Connection refused:** Ensure Blazor Server is running and healthy
- **Flaky tests:** Add explicit waits, check for race conditions

## Key Takeaways

- Playwright provides robust cross-browser UI testing for Blazor Server applications
- Sequential test execution prevents race conditions in Blazor SignalR circuits
- Automation scripts simplify the development workflow
- Clear separation between unit tests (xUnit) and UI tests (Playwright)
- Framework is extensible for future Azure Playwright Testing workspace integration

## Questions or Feedback

- Should we include visual regression testing in future iterations?
- Do we need mobile browser testing (iOS Safari, Android Chrome)?
- Should we integrate with Azure Playwright Testing workspace immediately or defer?
- What is the desired test coverage percentage for UI tests?

## Call to Action

- Review and approve this PRD
- Begin Phase 1 implementation (Playwright installation and configuration)
- Schedule knowledge transfer session for team members
- Identify initial test scenarios for Phase 2 development

## Appendix A: Related Documentation

- [Playwright Documentation](https://playwright.dev/)
- [Blazor Server Testing Guide](https://learn.microsoft.com/en-us/aspnet/core/blazor/test)
- [Azure Playwright Testing](https://learn.microsoft.com/en-us/azure/playwright-testing/)
- prd-migrate-to-azure-with-azd.md (Parent PRD)

## Appendix B: Environment Variables

No additional environment variables required for local testing. Azure-hosted testing may require:
- `PLAYWRIGHT_SERVICE_URL` (Azure Playwright Testing workspace URL)
- `PLAYWRIGHT_SERVICE_ACCESS_TOKEN` (Authentication token)

## Appendix C: Browser Compatibility

| Browser | Version | Support Status |
|---|---|---|
| Chromium | Latest stable | ✅ Full support |
| Firefox | Latest stable | ✅ Full support |
| WebKit | Latest stable | ✅ Full support |
| Edge | Latest stable | ⚠️ Uses Chromium project |
| Safari | Latest stable | ⚠️ Uses WebKit project |

## Version History

| Version | Date | Author | Changes |
|---|---|---|---|
| 1.0 | November 5, 2025 | GitHub Copilot | Initial PRD creation |
