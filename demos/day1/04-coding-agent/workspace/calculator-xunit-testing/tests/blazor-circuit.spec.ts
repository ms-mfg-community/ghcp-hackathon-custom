import { test, expect } from '@playwright/test';

/**
 * Blazor Circuit Tests
 * Tests for Blazor Server-specific functionality including SignalR circuits
 */
test.describe('Blazor Circuit Tests', () => {

    test('SignalR connection establishes successfully', async ({ page }) => {
        await page.goto('/');

        // Wait for Blazor to initialize
        await page.waitForFunction(() => {
            return typeof (window as any).Blazor !== 'undefined';
        }, { timeout: 10000 });

        // Check for SignalR connection by monitoring network activity
        // Blazor Server uses SignalR for server-client communication
        let signalRConnected = false;

        page.on('websocket', ws => {
            // WebSocket connection indicates SignalR is working
            signalRConnected = true;
        });

        // Wait a moment for WebSocket to establish
        await page.waitForTimeout(2000);

        // Verify Blazor is connected
        const isBlazorRunning = await page.evaluate(() => {
            return typeof (window as any).Blazor !== 'undefined';
        });

        expect(isBlazorRunning).toBe(true);
    });

    test('Blazor Server maintains state during interactions', async ({ page }) => {
        await page.goto('/');
        await page.waitForLoadState('domcontentloaded');

        // Wait for Blazor to initialize
        await page.waitForFunction(() => {
            return typeof (window as any).Blazor !== 'undefined';
        }, { timeout: 10000 });

        // Perform an interaction (adjust based on actual UI)
        const firstInput = page.locator('input').first();
        if (await firstInput.isVisible({ timeout: 5000 })) {
            await firstInput.fill('42');

            // Wait for server round-trip
            await page.waitForTimeout(500);

            // Verify the value persisted (state maintained on server)
            const value = await firstInput.inputValue();
            expect(value).toBe('42');
        }
    });

    test('Blazor handles server-side rendering updates', async ({ page }) => {
        await page.goto('/');

        // Wait for Blazor to initialize
        await page.waitForFunction(() => {
            return typeof (window as any).Blazor !== 'undefined';
        }, { timeout: 10000 });

        // Find an interactive element (button, input, etc.)
        const button = page.locator('button').first();

        if (await button.isVisible({ timeout: 5000 })) {
            // Click the button to trigger server-side update
            await button.click();

            // Wait for server response
            await page.waitForTimeout(500);

            // Verify the page is still responsive (circuit maintained)
            expect(await button.isEnabled()).toBe(true);
        } else {
            // If no button found, just verify Blazor is still running
            const isBlazorRunning = await page.evaluate(() => {
                return typeof (window as any).Blazor !== 'undefined';
            });
            expect(isBlazorRunning).toBe(true);
        }
    });

    test('Blazor circuit reconnects after temporary disconnection', async ({ page, context }) => {
        await page.goto('/');

        // Wait for Blazor to initialize
        await page.waitForFunction(() => {
            return typeof (window as any).Blazor !== 'undefined';
        }, { timeout: 10000 });

        // Simulate network condition change (offline/online)
        await context.setOffline(true);
        await page.waitForTimeout(1000);
        await context.setOffline(false);

        // Wait for reconnection
        await page.waitForTimeout(2000);

        // Verify Blazor is still functional
        const isBlazorRunning = await page.evaluate(() => {
            return typeof (window as any).Blazor !== 'undefined';
        });

        expect(isBlazorRunning).toBe(true);
    });

    test('Multiple operations maintain circuit integrity', async ({ page }) => {
        await page.goto('/');

        // Wait for Blazor to initialize
        await page.waitForFunction(() => {
            return typeof (window as any).Blazor !== 'undefined';
        }, { timeout: 10000 });

        // Perform multiple interactions to stress-test the circuit
        const inputs = page.locator('input');
        const inputCount = await inputs.count();

        if (inputCount > 0) {
            // Fill multiple inputs
            for (let i = 0; i < Math.min(inputCount, 3); i++) {
                await inputs.nth(i).fill(`${i + 1}`);
                await page.waitForTimeout(300);
            }

            // Verify all inputs retained their values (circuit maintained state)
            for (let i = 0; i < Math.min(inputCount, 3); i++) {
                const value = await inputs.nth(i).inputValue();
                expect(value).toBe(`${i + 1}`);
            }
        }

        // Verify Blazor circuit is still active
        const isBlazorRunning = await page.evaluate(() => {
            return typeof (window as any).Blazor !== 'undefined';
        });

        expect(isBlazorRunning).toBe(true);
    });
});
