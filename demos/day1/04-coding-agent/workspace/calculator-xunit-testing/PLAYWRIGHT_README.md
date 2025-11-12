# Playwright Testing Setup

This directory contains Playwright end-to-end tests for the Calculator Blazor Server application.

## Prerequisites

- .NET 9.0 SDK
- Node.js 18 or higher
- PowerShell (for automation scripts)

## Installation

The Playwright framework and browsers are already installed. If you need to reinstall:

```powershell
# Install npm dependencies
npm install

# Install Playwright browsers
npx playwright install
```

## Running Tests

### Using the Automation Script (Recommended)

The `Start-BlazorAndTest.ps1` script handles everything automatically:

```powershell
# Run all tests (headless mode)
.\Start-BlazorAndTest.ps1

# Run tests with visible browsers
.\Start-BlazorAndTest.ps1 -Headed

# Run only Chromium tests
.\Start-BlazorAndTest.ps1 -Browser chromium

# Run Firefox tests with Release build
.\Start-BlazorAndTest.ps1 -Configuration Release -Project firefox
```

### Manual Test Execution

If you prefer to manage the server manually:

1. Start the Blazor Server:
```powershell
cd calculator.web
dotnet run
```

2. In a separate terminal, run tests:
```powershell
# Run all tests
npm test

# Run with visible browsers
npm run test:headed

# Run in debug mode
npm run test:debug

# Run specific browser
npm run test:chromium
npm run test:firefox
npm run test:webkit
```

3. View test report:
```powershell
npm run test:report
```

## Test Structure

```
tests/
├── smoke-tests.spec.ts           # Basic application health checks
├── calculator-operations.spec.ts # Calculator operation tests
└── blazor-circuit.spec.ts        # Blazor Server SignalR circuit tests
```

### Test Categories

- **Smoke Tests**: Verify application loads, health endpoint responds, critical UI elements visible
- **Calculator Operations**: Test arithmetic operations (add, subtract, multiply, divide) and edge cases
- **Blazor Circuit Tests**: Validate Blazor Server-specific functionality including SignalR connections

## Configuration

The `playwright.config.ts` file contains:

- **Base URL**: `https://localhost:5001`
- **Browsers**: Chromium, Firefox, WebKit
- **Execution**: Sequential (to prevent race conditions)
- **Timeout**: 30 seconds per test
- **Retries**: 2 retries on failure
- **Reports**: HTML report in `playwright-report/`

## Test Development

When adding new tests:

1. Create test files in the `tests/` directory with `.spec.ts` extension
2. Import Playwright test utilities:
   ```typescript
   import { test, expect } from '@playwright/test';
   ```
3. Follow the existing test structure and naming conventions
4. Use explicit waits for Blazor initialization:
   ```typescript
   await page.waitForFunction(() => {
     return typeof (window as any).Blazor !== 'undefined';
   }, { timeout: 10000 });
   ```

## Troubleshooting

### Tests timeout
- Increase timeout in `playwright.config.ts`
- Check if Blazor Server started successfully
- Verify port 5001 is available

### Certificate errors
- The config already has `ignoreHTTPSErrors: true` for localhost
- Trust the development certificate: `dotnet dev-certs https --trust`

### Connection refused
- Ensure Blazor Server is running
- Check health endpoint: `https://localhost:5001/health`
- Verify no firewall blocking port 5001

### Flaky tests
- Tests run sequentially to prevent race conditions
- Add explicit waits for UI elements
- Check for proper Blazor initialization

## CI/CD Integration

To integrate with GitHub Actions or Azure DevOps:

1. Install dependencies: `npm ci`
2. Install browsers: `npx playwright install --with-deps`
3. Run tests: `npm test`
4. Upload reports as artifacts

See the PRD document (`prd-configure-playwright-testing.md`) for detailed CI/CD configuration examples.

## Reports

After test execution:

- **HTML Report**: `playwright-report/index.html`
- **Screenshots**: Captured on failure
- **Videos**: Recorded on failure
- **Traces**: Captured on first retry

View the HTML report:
```powershell
npm run test:report
```

## Related Documentation

- [Playwright Documentation](https://playwright.dev/)
- [Blazor Server Testing](https://learn.microsoft.com/en-us/aspnet/core/blazor/test)
- PRD: `prd-configure-playwright-testing.md`
- Parent PRD: `prd-migrate-to-azure-with-azd.md`

## Support

For issues or questions:
1. Check the troubleshooting section above
2. Review test output and HTML reports
3. Refer to the PRD for detailed requirements and specifications
