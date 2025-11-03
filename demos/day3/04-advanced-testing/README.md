# Day 3.4: Advanced Test Automation

## Objective

Master advanced testing patterns and CI/CD integration:
- Data-driven testing
- End-to-end workflows
- Performance testing
- Security testing
- CI/CD pipeline integration

## What You'll Learn

1. Parameterized/data-driven tests
2. Complex user workflows
3. API testing alongside UI testing
4. Environment configuration
5. Test reporting and metrics
6. Parallel test execution

## Data-Driven Testing

### Test Data
```javascript
const testData = [
    { email: 'valid@example.com', password: 'Password123!', expected: 'success' },
    { email: 'invalid', password: 'weak', expected: 'error' },
    { email: '', password: 'valid', expected: 'required' }
];

testData.forEach(data => {
    test(`Login with ${data.email}`, async ({ page }) => {
        // Test using data
    });
});
```

## End-to-End Workflow

```javascript
test('complete purchase flow', async ({ page }) => {
    // 1. Browse products
    // 2. Add to cart
    // 3. Checkout
    // 4. Payment
    // 5. Order confirmation
    // 6. Email verification
});
```

## API + UI Testing

```javascript
test('verify API data in UI', async ({ page, context }) => {
    // Get data from API
    const response = await context.request.get('/api/products');
    
    // Verify in UI
    const uiItems = page.locator('.product-item');
    // Compare counts and values
});
```

## CI/CD Integration

### GitHub Actions Example
```yaml
name: E2E Tests
on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - uses: actions/setup-node@v2
      - run: npm install
      - run: npx playwright install
      - run: npm run test:e2e
      - uses: actions/upload-artifact@v2
        with:
          name: playwright-report
```

## Performance Testing

```javascript
test('homepage load time', async ({ page }) => {
    const startTime = performance.now();
    await page.goto('/');
    const loadTime = performance.now() - startTime;
    
    expect(loadTime).toBeLessThan(3000); // 3 seconds
});
```

## Security Testing

```javascript
test('SQL injection prevention', async ({ page }) => {
    await page.fill('input', "'; DROP TABLE users; --");
    await page.click('button[type="submit"]');
    
    // Verify error handling, not execution
    expect(page.locator('.error')).toBeVisible();
});
```
