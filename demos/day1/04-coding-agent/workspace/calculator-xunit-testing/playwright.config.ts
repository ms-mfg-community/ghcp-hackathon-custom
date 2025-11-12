import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright configuration for Calculator Blazor Server application
 * See https://playwright.dev/docs/test-configuration
 */
export default defineConfig({
  testDir: './tests',
  
  /* Run tests sequentially to prevent race conditions in Blazor SignalR circuits */
  fullyParallel: false,
  
  /* Fail the build on CI if you accidentally left test.only in the source code */
  forbidOnly: !!process.env.CI,
  
  /* Retry on CI and locally for flaky tests */
  retries: process.env.CI ? 2 : 2,
  
  /* Single worker for sequential execution */
  workers: 1,
  
  /* Reporter to use */
  reporter: 'html',
  
  /* Timeout for each test */
  timeout: 30000,
  
  /* Shared settings for all the projects below */
  use: {
    /* Base URL for the Blazor Server application */
    baseURL: 'https://localhost:5001',
    
    /* Collect trace on first retry */
    trace: 'on-first-retry',
    
    /* Ignore HTTPS errors for localhost development */
    ignoreHTTPSErrors: true,
    
    /* Screenshot on failure */
    screenshot: 'only-on-failure',
    
    /* Video on first retry */
    video: 'retain-on-failure',
  },

  /* Configure projects for major browsers */
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

  /* Manual server management via Start-BlazorAndTest.ps1 script */
  webServer: undefined,
});
