param location string
param environmentName string
param resourceToken string
param abbrs object
param tags object = {}
param storageAccountName string

// Note: Microsoft Playwright Testing is in preview
// This module creates a placeholder for Playwright Testing workspace
// The actual resource type may vary based on preview availability

// For now, we'll create a simple resource placeholder
// Update this when Microsoft.AzurePlaywrightService becomes generally available

output playwrightWorkspaceName string = 'pw-${environmentName}-${resourceToken}'
output playwrightWorkspaceEndpoint string = 'https://pw-${environmentName}-${resourceToken}.${location}.playwright.microsoft.com'
output playwrightStorageAccount string = storageAccountName
