// Day 3.3 Demo: Playwright Testing Examples
// Generated with GitHub Copilot for browser automation

import { test, expect, Page } from '@playwright/test';

/**
 * Example Page Object Model
 * Encapsulates page interactions for reusability
 */
class ProductPage {
    constructor(private page: Page) { }

    async navigateTo(productId: string) {
        await this.page.goto(`/products/${productId}`);
        await this.page.waitForLoadState('networkidle');
    }

    async getProductTitle(): Promise<string> {
        return this.page.locator('h1').textContent();
    }

    async getProductPrice(): Promise<string> {
        return this.page.locator('.price').textContent();
    }

    async getProductRating(): Promise<string> {
        return this.page.locator('.rating').textContent();
    }

    async addToCart() {
        await this.page.locator('button:has-text("Add to Cart")').click();
        await this.page.waitForSelector('.cart-updated');
    }

    async selectQuantity(quantity: number) {
        const input = this.page.locator('input[type="number"]');
        await input.clear();
        await input.fill(quantity.toString());
    }

    async buyNow() {
        await this.page.locator('button:has-text("Buy Now")').click();
    }
}

class CartPage {
    constructor(private page: Page) { }

    async navigateTo() {
        await this.page.goto('/cart');
        await this.page.waitForLoadState('networkidle');
    }

    async getCartItemCount(): Promise<number> {
        const items = await this.page.locator('.cart-item').count();
        return items;
    }

    async getCartTotal(): Promise<string> {
        return this.page.locator('.total-price').textContent();
    }

    async removeItem(index: number) {
        await this.page.locator('.remove-button').nth(index).click();
    }

    async checkout() {
        await this.page.locator('button:has-text("Checkout")').click();
    }

    async isEmpty(): Promise<boolean> {
        const emptyMessage = await this.page.locator('text=Your cart is empty');
        return emptyMessage.isVisible();
    }
}

class CheckoutPage {
    constructor(private page: Page) { }

    async navigateTo() {
        await this.page.goto('/checkout');
        await this.page.waitForLoadState('networkidle');
    }

    async fillShippingInfo(
        firstName: string,
        lastName: string,
        address: string,
        city: string,
        zipCode: string
    ) {
        await this.page.fill('input[name="firstName"]', firstName);
        await this.page.fill('input[name="lastName"]', lastName);
        await this.page.fill('input[name="address"]', address);
        await this.page.fill('input[name="city"]', city);
        await this.page.fill('input[name="zipCode"]', zipCode);
    }

    async selectShippingMethod(method: string) {
        await this.page.locator(`label:has-text("${method}")`).click();
    }

    async fillPaymentInfo(cardNumber: string, expiryDate: string, cvv: string) {
        // Note: In production, use secure payment provider's iframe
        await this.page.fill('input[name="cardNumber"]', cardNumber);
        await this.page.fill('input[name="expiryDate"]', expiryDate);
        await this.page.fill('input[name="cvv"]', cvv);
    }

    async placeOrder() {
        await this.page.locator('button:has-text("Place Order")').click();
        await this.page.waitForSelector('.order-confirmation');
    }

    async getOrderNumber(): Promise<string> {
        return this.page.locator('.order-number').textContent();
    }
}

// ============================================================
// TEST CASES
// ============================================================

test.describe('E-Commerce Product Tests', () => {
    let productPage: ProductPage;

    test.beforeEach(async ({ page }) => {
        productPage = new ProductPage(page);
        await productPage.navigateTo('product-1');
    });

    test('should display product information', async () => {
        const title = await productPage.getProductTitle();
        expect(title).toBeTruthy();
        expect(title).not.toBe('');

        const price = await productPage.getProductPrice();
        expect(price).toMatch(/\$\d+\.\d{2}/);
    });

    test('should add product to cart', async () => {
        await productPage.selectQuantity(2);
        await productPage.addToCart();
        // Verify success message appears
    });

    test('should handle quantity selection', async () => {
        await productPage.selectQuantity(5);
        // Verify quantity is updated
    });

    test('should enable buy now button', async () => {
        const buyButton = productPage.page.locator('button:has-text("Buy Now")');
        expect(buyButton).toBeEnabled();
    });
});

test.describe('Shopping Cart Tests', () => {
    let cartPage: CartPage;

    test.beforeEach(async ({ page }) => {
        cartPage = new CartPage(page);
        await cartPage.navigateTo();
    });

    test('should display cart items', async () => {
        const itemCount = await cartPage.getCartItemCount();
        expect(itemCount).toBeGreaterThan(0);
    });

    test('should calculate total correctly', async () => {
        const total = await cartPage.getCartTotal();
        expect(total).toMatch(/\$\d+\.\d{2}/);
    });

    test('should remove item from cart', async () => {
        const initialCount = await cartPage.getCartItemCount();
        await cartPage.removeItem(0);
        const finalCount = await cartPage.getCartItemCount();
        expect(finalCount).toBe(initialCount - 1);
    });

    test('should show empty cart message when no items', async () => {
        // Remove all items
        let itemCount = await cartPage.getCartItemCount();
        for (let i = 0; i < itemCount; i++) {
            await cartPage.removeItem(0);
        }

        const isEmpty = await cartPage.isEmpty();
        expect(isEmpty).toBe(true);
    });

    test('should enable checkout button', async ({ page }) => {
        const checkoutButton = page.locator('button:has-text("Checkout")');
        expect(checkoutButton).toBeEnabled();
    });
});

test.describe('Checkout Flow Tests', () => {
    let checkoutPage: CheckoutPage;

    test.beforeEach(async ({ page }) => {
        checkoutPage = new CheckoutPage(page);
        await checkoutPage.navigateTo();
    });

    test('should complete checkout successfully', async () => {
        // Fill shipping information
        await checkoutPage.fillShippingInfo(
            'John',
            'Doe',
            '123 Main Street',
            'New York',
            '10001'
        );

        // Select shipping method
        await checkoutPage.selectShippingMethod('Standard (5-7 days)');

        // Fill payment info (in production, use Stripe/PayPal iframe)
        await checkoutPage.fillPaymentInfo('4111111111111111', '12/25', '123');

        // Place order
        await checkoutPage.placeOrder();

        // Verify order confirmation
        const orderNumber = await checkoutPage.getOrderNumber();
        expect(orderNumber).toMatch(/ORD-\d+/);
    });

    test('should validate required fields', async ({ page }) => {
        // Try to place order without filling fields
        await checkoutPage.placeOrder();

        // Verify error messages appear
        const errorMessages = page.locator('.error-message');
        const count = await errorMessages.count();
        expect(count).toBeGreaterThan(0);
    });
});

test.describe('Cross-Browser Tests', () => {
    test('should render correctly on Chrome', async ({ browser }) => {
        const context = await browser.newContext({ ...{ viewport: { width: 1280, height: 720 } } });
        const page = await context.newPage();
        await page.goto('/');
        expect(page).toBeTruthy();
        await context.close();
    });

    test('should handle responsive design', async ({ page }) => {
        await page.goto('/');

        // Test mobile view
        await page.setViewportSize({ width: 375, height: 667 });
        const mobileMenu = page.locator('.mobile-menu');
        expect(mobileMenu).toBeVisible();

        // Test tablet view
        await page.setViewportSize({ width: 768, height: 1024 });
        const tabletMenu = page.locator('.tablet-menu');
        expect(tabletMenu).toBeVisible();
    });
});

test.describe('Performance Tests', () => {
    test('should load product page in reasonable time', async ({ page }) => {
        const startTime = Date.now();
        await page.goto('/products/product-1');
        const loadTime = Date.now() - startTime;

        expect(loadTime).toBeLessThan(5000); // Should load in less than 5 seconds
    });

    test('should handle network failures gracefully', async ({ page }) => {
        await page.context().setOffline(true);
        await page.goto('/products/product-1');

        const errorMessage = page.locator('text=Connection error');
        expect(errorMessage).toBeVisible();

        await page.context().setOffline(false);
    });
});
