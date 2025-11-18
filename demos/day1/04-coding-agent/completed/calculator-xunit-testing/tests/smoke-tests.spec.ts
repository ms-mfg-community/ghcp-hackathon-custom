import { test, expect } from '@playwright/test';

/**
 * Smoke Tests
 * Basic tests to verify the application is running and accessible
 */
test.describe('Smoke Tests', () => {
  
  test('application loads successfully', async ({ page }) => {
    // Navigate to the application
    await page.goto('/');
    
    // Wait for the page to load
    await page.waitForLoadState('networkidle');
    
    // Verify the page loaded successfully
    expect(page.url()).toContain('localhost:5001');
  });

  test('health endpoint responds', async ({ page }) => {
    // Navigate to health endpoint
    const response = await page.goto('/health');
    
    // Verify health endpoint is accessible
    expect(response).not.toBeNull();
    expect(response?.status()).toBe(200);
  });

  test('calculator page title is present', async ({ page }) => {
    await page.goto('/');
    
    // Wait for the page to be fully loaded
    await page.waitForLoadState('domcontentloaded');
    
    // Check for Calculator heading
    const heading = page.locator('h1, h2, h3').filter({ hasText: /calculator/i }).first();
    await expect(heading).toBeVisible({ timeout: 10000 });
  });

  test('critical UI elements are visible', async ({ page }) => {
    await page.goto('/');
    await page.waitForLoadState('domcontentloaded');
    
    // Verify page is interactive (Blazor has loaded)
    // Look for any input field or button to confirm UI is rendered
    const interactiveElement = page.locator('input, button, select').first();
    await expect(interactiveElement).toBeVisible({ timeout: 10000 });
  });

  test('Blazor framework has initialized', async ({ page }) => {
    await page.goto('/');
    
    // Wait for Blazor to initialize by checking for Blazor script
    await page.waitForFunction(() => {
      return typeof (window as any).Blazor !== 'undefined';
    }, { timeout: 10000 });
    
    // If we got here, Blazor has initialized
    expect(true).toBe(true);
  });
});
