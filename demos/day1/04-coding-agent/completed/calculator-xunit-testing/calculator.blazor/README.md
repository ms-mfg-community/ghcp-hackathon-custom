# Blazor Calculator Web Application

A modern, responsive Blazor Server web application that performs basic calculator operations with keyboard shortcuts for enhanced user experience.

## Features

- **6 Mathematical Operations**: Addition, Subtraction, Multiplication, Division, Modulo, and Exponent
- **Keyboard Shortcuts**: Fast operation with Enter (calculate), Esc (clear), and +/-/*//%/^ (operator selection)
- **Responsive Design**: Works seamlessly on desktop, tablet, and mobile devices
- **Real-time Validation**: Immediate feedback on input errors
- **Beautiful UI**: Modern gradient-based design with smooth animations
- **Playwright-Ready**: All interactive elements have data-testid attributes for E2E testing

## Project Structure

```
calculator.blazor/
├── Models/
│   └── CalculationResult.cs      # Result wrapper with Success, Value, ErrorMessage
├── Services/
│   └── CalculatorService.cs      # Wrapper for static Calculator methods
├── Pages/
│   ├── _Host.cshtml              # HTML host page
│   ├── Index.razor               # Home page with documentation
│   └── Calculator.razor          # Main calculator page
├── Shared/
│   ├── MainLayout.razor          # Application layout with sidebar
│   └── NavMenu.razor             # Navigation menu
├── wwwroot/
│   └── css/
│       ├── calculator.css        # Calculator-specific styling
│       └── site.css              # Global site styling
├── _Imports.razor                # Global using directives
├── App.razor                     # Router configuration
├── Program.cs                    # Application startup and DI configuration
└── calculator.blazor.csproj      # Project file
```

## Running the Application

### Development Mode

```powershell
# Navigate to the calculator.blazor directory
cd calculator.blazor

# Run the application
dotnet run

# Or run with hot reload
dotnet watch run
```

The application will start and display the URL (typically `https://localhost:5001` or `http://localhost:5000`).

### Build for Production

```powershell
# Build in Release mode
dotnet build -c Release

# Run the Release build
dotnet run -c Release
```

## Keyboard Shortcuts

| Key | Action |
|-----|--------|
| **Enter** | Calculate the result |
| **Esc** | Clear all fields and results |
| **+** | Select Addition operator |
| **-** | Select Subtraction operator |
| **\*** | Select Multiplication operator |
| **/** | Select Division operator |
| **%** | Select Modulo operator |
| **^** | Select Exponent operator |

## Architecture

### CalculatorService Pattern

The application uses a wrapper service pattern for better testability:

```csharp
public class CalculatorService
{
    public CalculationResult Calculate(double firstNumber, double secondNumber, string operatorSymbol)
    {
        // Calls static Calculator.Add(), Subtract(), etc.
        // Returns CalculationResult with Success, Value, ErrorMessage
    }
}
```

### Dependency Injection

The service is registered as scoped in `Program.cs`:

```csharp
builder.Services.AddScoped<CalculatorService>();
```

### Data-TestId Attributes

All interactive elements have data-testid attributes for Playwright E2E testing:

- `first-number`: First number input
- `second-number`: Second number input
- `operator`: Operator dropdown
- `calculate-button`: Calculate button
- `clear-button`: Clear button
- `result-display`: Result container
- `result-value`: Result value text
- `error-message`: Error message container

## Testing

### Unit Tests

The static Calculator class is tested via the `calculator.tests` project:

```powershell
# Run all tests from solution root
dotnet test

# Expected output: 42 tests passed
```

### E2E Testing with Playwright

Example Playwright test using data-testid attributes:

```typescript
import { test, expect } from '@playwright/test';

test('calculate 5 + 3', async ({ page }) => {
  await page.goto('https://localhost:5001/calculator');
  
  await page.getByTestId('first-number').fill('5');
  await page.getByTestId('second-number').fill('3');
  await page.getByTestId('operator').selectOption('+');
  await page.getByTestId('calculate-button').click();
  
  await expect(page.getByTestId('result-value')).toContainText('5 + 3 = 8');
});
```

## Error Handling

- **Division by Zero**: Displays "Cannot divide by zero" error message
- **Invalid Input**: Browser validation prevents non-numeric input
- **Exception Handling**: All errors are caught and displayed to the user

## Browser Support

- **Chrome**: ✅ Fully supported
- **Edge**: ✅ Fully supported
- **Firefox**: ✅ Fully supported
- **Safari**: ✅ Fully supported

## Dependencies

- .NET 9.0
- Bootstrap 5.3.0 (via CDN)
- Bootstrap Icons 1.10.0 (via CDN)
- Blazor Server

## Development Notes

### CSS Organization

- **site.css**: Global styles, validation, accessibility
- **calculator.css**: Component-specific styles, animations

### Accessibility

- All form fields have labels
- Keyboard navigation fully supported
- Focus outlines visible for accessibility
- ARIA-compliant error messages

## Performance

- **Server-Side Rendering**: Fast initial page load with pre-rendering
- **SignalR Connection**: Real-time updates via WebSockets
- **Minimal JavaScript**: Most logic runs on the server

## Security

- Input validation on both client and server
- HTTPS enforced in production
- CSRF protection via Blazor framework

## License

MIT License - See parent project LICENSE file

## Related Projects

- **calculator**: Console calculator with static Calculator class
- **calculator.tests**: xUnit test suite with CSV-driven tests

## Support

For issues or questions, refer to the parent project documentation.
