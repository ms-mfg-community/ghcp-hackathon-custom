# Day 3.3: Playwright Testing Framework

## Objective

Master browser automation with Playwright:
- Selector strategies
- Test case generation with Copilot
- Page object model pattern
- Cross-browser testing
- Handling dynamic content

## What You'll Learn

1. Playwright setup and configuration
2. Element selection and interaction
3. Assertions and verifications
4. Waiting for elements (waits and retries)
5. Screenshot and recording capabilities
6. Test organization

## Installation

```powershell
npm install playwright
npm install -D @playwright/test

# Install browser binaries
npx playwright install
```

## Basic Test Structure

```javascript
import { test, expect } from '@playwright/test';

test('should add item to cart', async ({ page }) => {
    // Arrange
    await page.goto('https://example.com');
    
    // Act
    await page.click('button:has-text("Add to Cart")');
    
    // Assert
    const cartCount = await page.textContent('.cart-count');
    expect(cartCount).toBe('1');
});
```

## Selector Strategies

### 1. CSS Selectors
```javascript
await page.click('.product-button');
await page.fill('input[name="email"]', 'user@example.com');
```

### 2. XPath
```javascript
await page.click('//button[@class="submit"]');
```

### 3. Playwright Locators
```javascript
await page.locator('text=Add to Cart').click();
await page.getByRole('button', { name: 'Submit' }).click();
```

## Common Patterns

### Wait for Element
```javascript
await page.waitForSelector('.loading-complete');
```

### Verify Text Content
```javascript
expect(page.locator('h1')).toContainText('Welcome');
```

### Fill Form
```javascript
await page.fill('input[type="email"]', 'test@example.com');
await page.fill('input[type="password"]', 'password');
```

## GitHub Copilot Playwright Prompts

1. "Generate Playwright tests for login flow"
2. "Create page object model for product listing"
3. "Generate cross-browser tests with Playwright"
4. "Create test for adding to cart and checkout"
5. "Generate test for form validation scenarios"
