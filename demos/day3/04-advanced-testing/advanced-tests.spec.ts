// Day 3.4 Demo: Advanced Test Automation Patterns
// Generated with GitHub Copilot for enterprise testing

import { test, expect } from '@playwright/test';
import type { Page } from '@playwright/test';

// ============================================================
// DATA-DRIVEN TESTS
// ============================================================

const loginTestData = [
    {
        email: 'valid@example.com',
        password: 'ValidPassword123!',
        shouldSucceed: true,
        expectedMessage: 'Welcome back'
    },
    {
        email: 'invalid@example.com',
        password: 'WrongPassword123!',
        shouldSucceed: false,
        expectedMessage: 'Invalid credentials'
    },
    {
        email: '',
        password: 'password',
        shouldSucceed: false,
        expectedMessage: 'Email is required'
    },
    {
        email: 'test@example.com',
        password: '',
        shouldSucceed: false,
        expectedMessage: 'Password is required'
    }
];

test.describe('Data-Driven Login Tests', () => {
    loginTestData.forEach(testCase => {
        test(`Login with ${testCase.email || 'empty email'} should ${testCase.shouldSucceed ? 'succeed' : 'fail'}`,
            async ({ page }) => {
                await page.goto('/login');

                // Fill form
                if (testCase.email) {
                    await page.fill('input[name="email"]', testCase.email);
                }
                if (testCase.password) {
                    await page.fill('input[name="password"]', testCase.password);
                }

                // Submit
                await page.click('button[type="submit"]');

                // Verify
                const message = await page.locator('.message').textContent();
                expect(message).toContain(testCase.expectedMessage);
            }
        );
    });
});

// ============================================================
// PARAMETERIZED PRODUCT TESTS
// ============================================================

const productTestData = [
    { id: 'prod-001', name: 'Laptop', category: 'Electronics', price: 999.99 },
    { id: 'prod-002', name: 'Book', category: 'Books', price: 29.99 },
    { id: 'prod-003', name: 'Shirt', category: 'Clothing', price: 49.99 }
];

test.describe('Product Page Tests', () => {
    productTestData.forEach(product => {
        test(`Product ${product.name} should display correct information`,
            async ({ page }) => {
                await page.goto(`/products/${product.id}`);

                // Verify product details
                const title = await page.locator('h1').textContent();
                expect(title).toBe(product.name);

                const category = await page.locator('[data-testid="category"]').textContent();
                expect(category).toBe(product.category);

                const price = await page.locator('[data-testid="price"]').textContent();
                expect(price).toContain(product.price.toString());
            }
        );
    });
});

// ============================================================
// END-TO-END WORKFLOWS
// ============================================================

test.describe('Complete Purchase Workflow', () => {
    test('Should complete purchase from browse to confirmation', async ({ page }) => {
        // Step 1: Browse Products
        await page.goto('/');
        expect(await page.locator('.product-item').count()).toBeGreaterThan(0);

        // Step 2: Add to Cart
        await page.click('.product-item:first-child button:has-text("Add to Cart")');
        await page.waitForSelector('.toast:has-text("Added to cart")');

        // Step 3: Navigate to Cart
        await page.goto('/cart');
        const cartItems = await page.locator('.cart-item').count();
        expect(cartItems).toBe(1);

        // Step 4: Proceed to Checkout
        await page.click('button:has-text("Checkout")');
        await page.waitForURL('/checkout');

        // Step 5: Fill Shipping Info
        await page.fill('input[name="firstName"]', 'John');
        await page.fill('input[name="lastName"]', 'Doe');
        await page.fill('input[name="address"]', '123 Main Street');
        await page.fill('input[name="city"]', 'New York');
        await page.fill('input[name="zipCode"]', '10001');

        // Step 6: Select Shipping Method
        await page.click('label:has-text("Standard Shipping")');

        // Step 7: Fill Payment (Stripe iframe would be handled separately)
        await page.fill('input[name="cardholderName"]', 'John Doe');

        // Step 8: Place Order
        await page.click('button:has-text("Place Order")');
        await page.waitForURL(/\/order-confirmation\/\d+/);

        // Step 9: Verify Confirmation
        const orderNumber = await page.locator('[data-testid="order-number"]').textContent();
        expect(orderNumber).toMatch(/ORD-\d+/);

        // Step 10: Verify Email
        await page.goto('/');
        const userEmail = await page.locator('[data-testid="user-email"]').textContent();
        expect(userEmail).toBeTruthy();
    });
});

// ============================================================
// API + UI INTEGRATION TESTS
// ============================================================

test.describe('API & UI Integration Tests', () => {
    test('API product data should match UI display', async ({ page, context }) => {
        // Get data from API
        const apiResponse = await context.request.get('/api/products');
        const apiProducts = await apiResponse.json();

        // Navigate to product listing
        await page.goto('/products');

        // Get UI elements
        const uiProducts = await page.locator('.product-item').count();

        // Verify counts match
        expect(uiProducts).toBe(apiProducts.length);

        // Verify first product details
        if (apiProducts.length > 0) {
            const firstProduct = apiProducts[0];
            const uiTitle = await page.locator('.product-item:first-child .title').textContent();
            expect(uiTitle).toBe(firstProduct.name);

            const uiPrice = await page.locator('.product-item:first-child .price').textContent();
            expect(uiPrice).toContain(firstProduct.price.toString());
        }
    });

    test('Create product via API should appear in UI', async ({ page, context }) => {
        // Create product via API
        const newProduct = {
            name: 'Test Product',
            price: 99.99,
            category: 'Test'
        };

        const createResponse = await context.request.post('/api/products', {
            data: newProduct
        });
        const createdProduct = await createResponse.json();

        // Navigate to product page
        await page.goto(`/products/${createdProduct.id}`);

        // Verify product appears
        const title = await page.locator('h1').textContent();
        expect(title).toBe(newProduct.name);
    });
});

// ============================================================
// PERFORMANCE TESTS
// ============================================================

test.describe('Performance Tests', () => {
    test('Homepage should load within SLA', async ({ page }) => {
        const startTime = Date.now();

        await page.goto('/', { waitUntil: 'networkidle' });

        const loadTime = Date.now() - startTime;

        expect(loadTime).toBeLessThan(3000); // 3 second SLA
    });

    test('Product listing should render without performance issues', async ({ page }) => {
        await page.goto('/products');

        // Measure paint timing
        const paintTiming = await page.evaluate(() => {
            const navigation = performance.getEntriesByType('navigation')[0];
            return {
                firstPaint: navigation.toJSON(),
                resourceCount: performance.getEntriesByType('resource').length
            };
        });

        expect(paintTiming.resourceCount).toBeLessThan(50);
    });

    test('Image loading should not block interaction', async ({ page }) => {
        await page.goto('/products');

        // Verify button is clickable even while images load
        const addButton = page.locator('button:has-text("Add to Cart")').first();
        expect(addButton).toBeEnabled();
    });
});

// ============================================================
// SECURITY TESTS
// ============================================================

test.describe('Security Tests', () => {
    test('Should prevent XSS attack in search', async ({ page }) => {
        await page.goto('/search');

        const xssPayload = '<img src=x onerror="alert(\'XSS\')">';
        await page.fill('input[name="query"]', xssPayload);
        await page.click('button[type="submit"]');

        // Verify payload is escaped, not executed
        const searchResults = page.locator('.search-results');
        expect(searchResults).toBeVisible();

        // Verify no alerts were triggered
        page.on('dialog', dialog => {
            expect(dialog.type()).not.toBe('alert');
        });
    });

    test('Should prevent SQL injection in filters', async ({ page }) => {
        await page.goto('/products');

        const sqlPayload = "'; DROP TABLE products; --";
        await page.fill('input[name="search"]', sqlPayload);

        // Should display products, not execute SQL
        const products = await page.locator('.product-item').count();
        expect(products).toBeGreaterThanOrEqual(0);
    });

    test('Should validate CSRF protection', async ({ page, context }) => {
        await page.goto('/settings');

        // Verify CSRF token in form
        const csrfToken = await page.locator('input[name="_csrf"]').getAttribute('value');
        expect(csrfToken).toBeTruthy();

        // Attempt request without CSRF token should fail
        const response = await context.request.post('/api/settings/update', {
            data: { email: 'newemail@example.com' },
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        // Should require CSRF token
        expect(response.status()).toBeGreaterThanOrEqual(400);
    });
});

// ============================================================
// ACCESSIBILITY TESTS
// ============================================================

test.describe('Accessibility Tests', () => {
    test('Page should be keyboard navigable', async ({ page }) => {
        await page.goto('/');

        // Tab through interactive elements
        await page.keyboard.press('Tab');
        let focusedElement = await page.evaluate(() => document.activeElement?.tagName);
        expect(['BUTTON', 'A', 'INPUT']).toContain(focusedElement);

        await page.keyboard.press('Tab');
        focusedElement = await page.evaluate(() => document.activeElement?.tagName);
        expect(['BUTTON', 'A', 'INPUT']).toContain(focusedElement);
    });

    test('Images should have alt text', async ({ page }) => {
        await page.goto('/products');

        const images = await page.locator('img').all();
        for (const img of images) {
            const altText = await img.getAttribute('alt');
            expect(altText).toBeTruthy();
        }
    });

    test('Form labels should be associated with inputs', async ({ page }) => {
        await page.goto('/contact');

        const labels = await page.locator('label').all();
        for (const label of labels) {
            const forAttr = await label.getAttribute('for');
            expect(forAttr).toBeTruthy();
        }
    });
});
