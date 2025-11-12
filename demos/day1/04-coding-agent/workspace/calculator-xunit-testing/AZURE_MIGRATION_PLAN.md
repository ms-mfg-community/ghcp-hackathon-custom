# Azure Migration Plan: Calculator Application

## Document Information

- **Version:** 1.0
- **Date:** November 7, 2025
- **Status:** Ready for Implementation
- **Project:** Calculator Blazor Server Application
- **Deployment Method:** Azure Developer CLI (azd) with Bicep IaC

## Executive Summary

This document provides a comprehensive migration plan to deploy the Calculator Blazor Server application to Azure using Azure Developer CLI (`azd`) with Bicep as the Infrastructure as Code (IaC) solution. The deployment leverages GitHub for source control and workflow orchestration, hosting the application on Azure App Service (B1 tier) with supporting services including Azure SQL Database, Key Vault, Application Insights, Log Analytics, Storage Account, and Azure Playwright Testing workspace.

## Current State Assessment

### Existing Infrastructure (✅ Already Implemented)

The project already has significant Azure infrastructure configured:

**Bicep Modules:**
- ✅ `infra/main.bicep` - Main infrastructure orchestration
- ✅ `infra/modules/app-service.bicep` - App Service and Plan (B1 tier)
- ✅ `infra/modules/keyvault.bicep` - Key Vault for secrets
- ✅ `infra/modules/monitoring.bicep` - Application Insights + Log Analytics
- ✅ `infra/modules/sql.bicep` - Azure SQL Server and Database (Basic tier)
- ✅ `infra/modules/storage.bicep` - Storage Account
- ✅ `infra/modules/playwright.bicep` - Playwright Testing workspace
- ✅ `infra/modules/keyvault-role-assignment.bicep` - RBAC for Key Vault
- ✅ `infra/modules/storage-role-assignment.bicep` - RBAC for Storage

**Configuration:**
- ✅ `azure.yaml` - azd configuration
- ✅ `.azure/config.json` - Environment configuration
- ✅ `.azure/calculator-prod/.env` - Environment variables

**SQLite Database:**
- ✅ Schema: `CalculatorTestCase` table with **41 test records** (already exists)
- ✅ Location: `calculator_tests.db` (ready for migration)
- ✅ Entity Framework Context: `TestDataDbContext.cs`
- ✅ Categories: Addition (6), Division (6), Multiplication (7), Subtraction (5), Modulo (8), Exponent (7), EdgeCases (2)

**Playwright Testing:**
- ✅ Configuration: `playwright.config.ts`
- ✅ Test Suites: 26 tests across 3 spec files
- ✅ Automation: `Start-BlazorAndTest.ps1`

## Target Architecture

### Azure Resources

| Resource | Type | SKU/Tier | Purpose |
|---|---|---|---|
| App Service Plan | Microsoft.Web/serverfarms | B1 (Basic) | Host Blazor Server application |
| App Service | Microsoft.Web/sites | - | Calculator web application |
| Key Vault | Microsoft.KeyVault/vaults | Standard | Store secrets and connection strings |
| Application Insights | Microsoft.Insights/components | - | Performance monitoring and telemetry |
| Log Analytics Workspace | Microsoft.OperationalInsights/workspaces | PerGB2018 | Log aggregation, metrics, alerting |
| Storage Account | Microsoft.Storage/storageAccounts | Standard_LRS | File storage, test artifacts |
| SQL Server | Microsoft.Sql/servers | - | Database server |
| SQL Database | Microsoft.Sql/servers/databases | Basic (5 DTU) | Calculator test data |
| Playwright Workspace | Microsoft.AzurePlaywrightService/accounts | - | Web UI testing |

### Tenant and Subscription Details

- **Tenant ID:** `54d665dd-30f1-45c5-a8d5-d6ffcdb518f9`
- **Subscription ID:** `dee9fea3-65b6-45ad-8b44-f38b05c31fa9`
- **Subscription Name:** Online Landing Zone
- **Default Environment:** `calculator-prod`
- **Default Region:** `eastus2`

## Migration Plan Phases

### Phase 1: Pre-Migration Preparation ⏳

**Duration:** 1-2 hours

#### 1.1 SQLite Database Verification

**Status:** ✅ Database already exists with 41 test records

```powershell
# Navigate to project directory
cd demos\day1\04-coding-agent\workspace\calculator-xunit-testing

# Verify database exists
Test-Path calculator_tests.db  # Should return: True

# Check record count
sqlite3 calculator_tests.db "SELECT COUNT(*) FROM TestCases;"  # Returns: 41

# View breakdown by category
sqlite3 calculator_tests.db "SELECT Category, COUNT(*) as Count FROM TestCases GROUP BY Category;"
```

**Current Data:**
- **Total Records:** 41 test cases
- **Categories:** Addition (6), Division (6), Multiplication (7), Subtraction (5), Modulo (8), Exponent (7), EdgeCases (2)
- **Operations:** Add, Subtract, Multiply, Divide, Modulo, Exponent

**Optional - Re-initialize if needed:**
```powershell
# Only run if you need to reset the database
.\Initialize-And-Query-Database.ps1
```

#### 1.2 Update Storage Account Configuration

**Action Required:** Add security tag to storage account Bicep module

**File:** `infra/modules/storage.bicep`

**Update Required:**
```bicep
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccountName
  location: location
  tags: union(tags, {
    'SecurityControl': 'Ignore'  // Add this tag
  })
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    allowBlobPublicAccess: false
    allowSharedKeyAccess: true  // Enable key-based auth as requested
    minimumTlsVersion: 'TLS1_2'
    supportsHttpsTrafficOnly: true
    defaultToOAuthAuthentication: false  // Allow key-based alongside EntraID
  }
}
```

#### 1.3 Verify Azure CLI Authentication

```powershell
# Check authentication status
azd auth login --check-status

# If not logged in
azd auth login

# Verify subscription
az account show
az account set --subscription "dee9fea3-65b6-45ad-8b44-f38b05c31fa9"
```

#### 1.4 Environment Variable Configuration

**File:** `.azure/calculator-prod/.env`

**Verify/Update:**
```env
AZURE_ENV_NAME="calculator-prod"
AZURE_LOCATION="eastus2"
AZURE_SUBSCRIPTION_ID="dee9fea3-65b6-45ad-8b44-f38b05c31fa9"
AZURE_TENANT_ID="54d665dd-30f1-45c5-a8d5-d6ffcdb518f9"
principalId="<your-user-principal-id>"
sqlAdminLogin="sqladmin"
sqlAdminPassword="<secure-password>"
```

**Get Principal ID:**
```powershell
# Get your principal ID
az ad signed-in-user show --query id -o tsv
```

### Phase 2: Infrastructure Provisioning 🚀

**Duration:** 10-15 minutes

#### 2.1 Provision Azure Infrastructure

```powershell
# Navigate to project root
cd demos\day1\04-coding-agent\workspace\calculator-xunit-testing

# Provision infrastructure (no deployment yet)
azd provision
```

**What This Does:**
- Creates resource group: `rg-calculator-prod-eastus2`
- Deploys all Bicep modules
- Provisions App Service Plan (B1)
- Creates App Service
- Sets up Key Vault
- Configures Application Insights + Log Analytics
- Creates Storage Account with tags
- Provisions Azure SQL Server + Database
- Sets up Playwright Testing workspace
- Configures RBAC permissions
- Stores connection strings in Key Vault

**Expected Duration:** 8-12 minutes

#### 2.2 Verify Infrastructure Deployment

```powershell
# List all resources in the resource group
az resource list --resource-group rg-calculator-prod-eastus2 --output table

# Verify App Service
az webapp show --name <app-service-name> --resource-group rg-calculator-prod-eastus2

# Verify SQL Database
az sql db show --server <sql-server-name> --name <db-name> --resource-group rg-calculator-prod-eastus2

# Verify Key Vault secrets
az keyvault secret list --vault-name <keyvault-name> --output table
```

### Phase 3: Database Migration (SQLite → Azure SQL) 🗄️

**Duration:** 30-60 minutes

#### 3.1 Export SQLite Data

**Create Migration Script:** `scripts/Export-SQLiteData.ps1`

```powershell
# Export SQLite data to JSON for migration
$dbPath = "calculator_tests.db"
$outputPath = "data-export.json"

# Use sqlite3 to export data
sqlite3 $dbPath "SELECT json_object(
    'Id', Id,
    'TestName', TestName,
    'Category', Category,
    'FirstOperand', FirstOperand,
    'SecondOperand', SecondOperand,
    'Operation', Operation,
    'ExpectedResult', ExpectedResult,
    'Description', Description,
    'IsActive', IsActive,
    'CreatedAt', CreatedAt
) FROM TestCases;" | Out-File -FilePath $outputPath -Encoding UTF8

Write-Host "Data exported to $outputPath" -ForegroundColor Green
```

#### 3.2 Prepare Azure SQL Database

**Create Migration Script:** `scripts/Initialize-AzureSqlDatabase.ps1`

```powershell
param(
    [Parameter(Mandatory=$true)]
    [string]$ServerName,
    
    [Parameter(Mandatory=$true)]
    [string]$DatabaseName,
    
    [Parameter(Mandatory=$true)]
    [string]$AdminUser,
    
    [Parameter(Mandatory=$true)]
    [SecureString]$AdminPassword
)

# Get connection string from Key Vault
$kvName = (azd env get-values | Where-Object { $_ -match 'KEY_VAULT_NAME' }).Split('=')[1].Trim('"')
$connString = az keyvault secret show --vault-name $kvName --name SqlConnectionString --query value -o tsv

# Create schema using Entity Framework migrations
Write-Host "Creating database schema..." -ForegroundColor Cyan

cd calculator.tests

# Add migration if not exists
dotnet ef migrations add InitialCreate --context TestDataDbContext

# Update database (creates schema and seeds data)
dotnet ef database update --context TestDataDbContext --connection "$connString"

Write-Host "Database schema created and seeded successfully!" -ForegroundColor Green
```

#### 3.3 Alternative: SQL Script Migration

**Create:** `scripts/sql/01-create-schema.sql`

```sql
-- Create CalculatorTestCase table in Azure SQL
CREATE TABLE [dbo].[TestCases] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [TestName] NVARCHAR(100) NOT NULL,
    [Category] NVARCHAR(50) NOT NULL,
    [FirstOperand] FLOAT NOT NULL,
    [SecondOperand] FLOAT NOT NULL,
    [Operation] NVARCHAR(20) NOT NULL,
    [ExpectedResult] FLOAT NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE INDEX IX_TestCases_Category ON TestCases(Category);
CREATE INDEX IX_TestCases_Operation ON TestCases(Operation);
```

**Create:** `scripts/sql/02-seed-data.sql`

```sql
-- Seed test data (same as SQLite)
INSERT INTO TestCases (TestName, Category, FirstOperand, SecondOperand, Operation, ExpectedResult, Description, IsActive)
VALUES
    ('Add_TwoPositiveNumbers', 'Addition', 5, 3, 'Add', 8, 'Adding two positive integers', 1),
    ('Add_PositiveAndNegative', 'Addition', 10, -5, 'Add', 5, 'Adding positive and negative numbers', 1),
    -- ... (all 26 records)
;
```

**Execute Migration:**
```powershell
# Get SQL connection details
$sqlServer = azd env get-values | Where-Object { $_ -match 'SQL_SERVER_NAME' } | ForEach-Object { $_.Split('=')[1].Trim('"') }
$sqlDb = azd env get-values | Where-Object { $_ -match 'SQL_DATABASE_NAME' } | ForEach-Object { $_.Split('=')[1].Trim('"') }

# Run scripts using sqlcmd
sqlcmd -S "$sqlServer.database.windows.net" -d $sqlDb -U sqladmin -P "<password>" -i scripts/sql/01-create-schema.sql
sqlcmd -S "$sqlServer.database.windows.net" -d $sqlDb -U sqladmin -P "<password>" -i scripts/sql/02-seed-data.sql
```

#### 3.4 Verify Data Migration

```powershell
# Query Azure SQL to verify data
sqlcmd -S "$sqlServer.database.windows.net" -d $sqlDb -U sqladmin -P "<password>" -Q "SELECT COUNT(*) AS RecordCount FROM TestCases"

# Should return 26 records
```

### Phase 4: Application Configuration 🔧

**Duration:** 15-30 minutes

#### 4.1 Update Connection Strings

**File:** `calculator.tests/calculator.tests.csproj`

**Add Configuration:**
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
  <PackageReference Include="Azure.Identity" Version="1.13.0" />
  <PackageReference Include="Azure.Security.KeyVault.Secrets" Version="4.8.0" />
</ItemGroup>
```

**File:** `calculator.tests/Data/TestDataDbContext.cs`

**Update OnConfiguring:**
```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        // Check for Azure SQL connection string
        var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
        
        if (!string.IsNullOrEmpty(connectionString))
        {
            // Use Azure SQL
            optionsBuilder.UseSqlServer(connectionString);
        }
        else
        {
            // Fallback to SQLite for local development
            optionsBuilder.UseSqlite("Data Source=calculator_tests.db");
        }
    }
}
```

#### 4.2 Configure App Service Settings

**Update:** `infra/modules/app-service.bicep`

**Add App Settings:**
```bicep
resource webApp 'Microsoft.Web/sites@2023-01-01' = {
  name: webAppName
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      netFrameworkVersion: 'v9.0'
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: applicationInsightsConnectionString
        }
        {
          name: 'SqlConnectionString'
          value: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/SqlConnectionString/)'
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
      ]
      connectionStrings: [
        {
          name: 'DefaultConnection'
          connectionString: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/SqlConnectionString/)'
          type: 'SQLAzure'
        }
      ]
    }
  }
}
```

### Phase 5: Application Deployment 🚢

**Duration:** 5-10 minutes

#### 5.1 Deploy Application

```powershell
# Deploy application code to App Service
azd deploy

# OR deploy and provision in one command
azd up
```

**What This Does:**
- Builds the Blazor Server application
- Packages the application
- Deploys to Azure App Service
- Configures application settings
- Restarts the app service

#### 5.2 Configure Startup Command

**Important:** Azure App Service may detect multiple `.runtimeconfig.json` files and default to the hosting placeholder. Configure the startup command explicitly:

```powershell
# Set the startup command to run the correct DLL
az webapp config set --name <app-name> --resource-group rg-calculator-prod-eastus2 --startup-file "dotnet calculator.web.dll"

# Restart the app to apply changes
az webapp restart --name <app-name> --resource-group rg-calculator-prod-eastus2
```

**Common Issue:**
```
WARNING: Found files: 'calculator.web.runtimeconfig.json, calculator.runtimeconfig.json'
WARNING: To fix this issue you can set the startup command to point to a particular startup file
Running the default app using command: dotnet "/defaulthome/hostingstart/hostingstart.dll"
```

**Resolution:** The explicit startup command tells Azure which DLL to execute, preventing it from running the default placeholder application.

#### 5.3 Verify Deployment

```powershell
# Get App Service URL
$appUrl = azd env get-values | Where-Object { $_ -match 'APP_SERVICE_URL' } | ForEach-Object { $_.Split('=')[1].Trim('"') }

Write-Host "Application URL: $appUrl" -ForegroundColor Green

# Check application logs
az webapp log tail --name <app-name> --resource-group rg-calculator-prod-eastus2

# Open in browser
Start-Process $appUrl
```

**Manual Verification:**
1. Wait 30-60 seconds for app restart to complete
2. Navigate to the App Service URL
3. Verify Calculator page loads (not placeholder)
4. Test basic operations (add, subtract, multiply, divide)
5. Check Application Insights for telemetry

### Phase 6: GitHub Workflow Integration 🔄

**Duration:** 30-45 minutes

#### 6.1 Create GitHub Actions Workflow

**File:** `.github/workflows/azure-deploy.yml`

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]
    paths:
      - 'demos/day1/04-coding-agent/workspace/calculator-xunit-testing/**'
  pull_request:
    branches: [ main ]
  workflow_dispatch:

env:
  AZURE_ENV_NAME: calculator-prod
  WORKING_DIRECTORY: demos/day1/04-coding-agent/workspace/calculator-xunit-testing

permissions:
  id-token: write
  contents: read

jobs:
  build:
    runs-on: ubuntu-latest
    defaults:
      run:
        working-directory: ${{ env.WORKING_DIRECTORY }}
    
    steps:
      - name: Checkout code
        uses: actions/checkout@v4
      
      - name: Setup .NET 9.0
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      
      - name: Restore dependencies
        run: dotnet restore
      
      - name: Build solution
        run: dotnet build --configuration Release --no-restore
      
      - name: Run unit tests
        run: dotnet test --no-restore --verbosity normal
      
      - name: Publish application
        run: dotnet publish calculator.web/calculator.web.csproj -c Release -o ./publish
      
      - name: Upload artifact
        uses: actions/upload-artifact@v4
        with:
          name: calculator-app
          path: ${{ env.WORKING_DIRECTORY }}/publish

  deploy:
    runs-on: ubuntu-latest
    needs: build
    if: github.ref == 'refs/heads/main'
    environment: production
    defaults:
      run:
        working-directory: ${{ env.WORKING_DIRECTORY }}
    
    steps:
      - name: Checkout code
        uses: actions/checkout@v4
      
      - name: Download artifact
        uses: actions/download-artifact@v4
        with:
          name: calculator-app
          path: ${{ env.WORKING_DIRECTORY }}/publish
      
      - name: Azure Login
        uses: azure/login@v2
        with:
          client-id: ${{ secrets.AZURE_CLIENT_ID }}
          tenant-id: ${{ secrets.AZURE_TENANT_ID }}
          subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}
      
      - name: Install azd
        uses: Azure/setup-azd@v1.0.0
      
      - name: Deploy to Azure
        run: |
          azd auth login --client-id ${{ secrets.AZURE_CLIENT_ID }} \
                         --client-secret ${{ secrets.AZURE_CLIENT_SECRET }} \
                         --tenant-id ${{ secrets.AZURE_TENANT_ID }}
          azd env refresh -e ${{ env.AZURE_ENV_NAME }}
          azd deploy --no-prompt
        env:
          AZURE_ENV_NAME: ${{ env.AZURE_ENV_NAME }}
          AZURE_LOCATION: eastus2
          AZURE_SUBSCRIPTION_ID: ${{ secrets.AZURE_SUBSCRIPTION_ID }}

  playwright-tests:
    runs-on: ubuntu-latest
    needs: deploy
    if: github.ref == 'refs/heads/main'
    defaults:
      run:
        working-directory: ${{ env.WORKING_DIRECTORY }}
    
    steps:
      - name: Checkout code
        uses: actions/checkout@v4
      
      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Install Playwright Browsers
        run: npx playwright install --with-deps
      
      - name: Get App Service URL
        id: get-url
        run: |
          APP_URL=$(azd env get-values | grep APP_SERVICE_URL | cut -d'=' -f2 | tr -d '"')
          echo "app_url=$APP_URL" >> $GITHUB_OUTPUT
      
      - name: Run Playwright tests
        run: npx playwright test
        env:
          BASE_URL: ${{ steps.get-url.outputs.app_url }}
      
      - name: Upload Playwright Report
        uses: actions/upload-artifact@v4
        if: always()
        with:
          name: playwright-report
          path: ${{ env.WORKING_DIRECTORY }}/playwright-report/
          retention-days: 30
```

#### 6.2 Configure GitHub Secrets

**Navigate to:** GitHub Repository → Settings → Secrets and variables → Actions

**Add Repository Secrets:**

| Secret Name | Value | Description |
|---|---|---|
| `AZURE_CLIENT_ID` | `<service-principal-client-id>` | Azure Service Principal App ID |
| `AZURE_CLIENT_SECRET` | `<service-principal-secret>` | Azure Service Principal Secret |
| `AZURE_TENANT_ID` | `54d665dd-30f1-45c5-a8d5-d6ffcdb518f9` | Azure Tenant ID |
| `AZURE_SUBSCRIPTION_ID` | `dee9fea3-65b6-45ad-8b44-f38b05c31fa9` | Azure Subscription ID |

**Create Service Principal:**
```powershell
# Create service principal for GitHub Actions
az ad sp create-for-rbac --name "github-actions-calculator" `
                          --role Contributor `
                          --scopes /subscriptions/dee9fea3-65b6-45ad-8b44-f38b05c31fa9 `
                          --sdk-auth

# Output will include:
# - clientId (AZURE_CLIENT_ID)
# - clientSecret (AZURE_CLIENT_SECRET)
# - tenantId (AZURE_TENANT_ID)
# - subscriptionId (AZURE_SUBSCRIPTION_ID)
```

### Phase 7: Playwright Testing Integration 🎭

**Duration:** 15-20 minutes

#### 7.1 Update Playwright Configuration for Azure

**File:** `playwright.config.ts`

**Update:**
```typescript
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  fullyParallel: false,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : 1,
  reporter: [
    ['html'],
    ['junit', { outputFile: 'test-results/junit.xml' }],
    ['json', { outputFile: 'test-results/results.json' }]
  ],
  timeout: 30000,
  
  use: {
    baseURL: process.env.BASE_URL || 'https://localhost:5001',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    ignoreHTTPSErrors: process.env.BASE_URL?.includes('localhost') ?? false,
  },

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

  webServer: process.env.CI ? undefined : {
    command: 'dotnet run --project calculator.web',
    url: 'https://localhost:5001',
    reuseExistingServer: !process.env.CI,
    timeout: 120000,
    ignoreHTTPSErrors: true,
  },
});
```

#### 7.2 Configure Azure Playwright Service

**Update:** `infra/modules/playwright.bicep`

```bicep
param location string
param environmentName string
param resourceToken string
param abbrs object
param tags object = {}
param storageAccountName string

// Azure Playwright Testing Workspace
resource playwrightWorkspace 'Microsoft.AzurePlaywrightService/accounts@2024-02-01-preview' = {
  name: '${abbrs.playwrightService}${environmentName}-${resourceToken}'
  location: location
  tags: tags
  properties: {
    regionalAffinity: 'Enabled'
    reporting: {
      type: 'StorageAccount'
      storageAccountName: storageAccountName
    }
    scalableExecution: {
      enabled: true
    }
  }
}

// Grant Playwright workspace access to storage
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

// Store Playwright workspace URL in Key Vault
output playwrightWorkspaceName string = playwrightWorkspace.name
output playwrightServiceUrl string = playwrightWorkspace.properties.dashboardUri
output playwrightWorkspaceId string = playwrightWorkspace.id
```

#### 7.3 Run Playwright Tests Against Azure

```powershell
# Get App Service URL
$appUrl = azd env get-values | Where-Object { $_ -match 'APP_SERVICE_URL' } | ForEach-Object { $_.Split('=')[1].Trim('"') }

# Run tests against Azure-hosted app
$env:BASE_URL = $appUrl
npm test

# View report
npm run test:report
```

### Phase 8: Monitoring and Observability 📊

**Duration:** 15-20 minutes

#### 8.1 Configure Application Insights

**File:** `calculator.web/Program.cs`

**Add:**
```csharp
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
});

// Add custom telemetry
builder.Services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();
```

#### 8.2 Create Custom Dashboards

**Azure Portal:**
1. Navigate to Application Insights resource
2. Create custom dashboard for calculator operations
3. Add charts for:
   - Request rates
   - Response times
   - Failed requests
   - Custom events (calculator operations)

#### 8.3 Configure Alerts

**Create Alert Rules:**

```powershell
# High response time alert
az monitor metrics alert create \
  --name "HighResponseTime" \
  --resource-group rg-calculator-prod-eastus2 \
  --scopes /subscriptions/dee9fea3-65b6-45ad-8b44-f38b05c31fa9/resourceGroups/rg-calculator-prod-eastus2/providers/Microsoft.Web/sites/<app-name> \
  --condition "avg requests/duration > 1000" \
  --description "Alert when average response time exceeds 1 second"

# Failed requests alert
az monitor metrics alert create \
  --name "FailedRequests" \
  --resource-group rg-calculator-prod-eastus2 \
  --scopes /subscriptions/dee9fea3-65b6-45ad-8b44-f38b05c31fa9/resourceGroups/rg-calculator-prod-eastus2/providers/Microsoft.Web/sites/<app-name> \
  --condition "total requests/failed > 10" \
  --description "Alert when failed requests exceed 10"
```

#### 8.4 Configure Log Analytics Queries

**Common Queries:**

```kusto
// Calculator operations
traces
| where message contains "Calculator"
| summarize count() by operation_Name, bin(timestamp, 5m)
| render timechart

// Error rate
requests
| where success == false
| summarize errorCount = count() by bin(timestamp, 1h)
| render timechart

// SQL query performance
dependencies
| where type == "SQL"
| summarize avg(duration), count() by name
| order by avg_duration desc
```

## Deployment Checklist

### Pre-Deployment ✓

- [ ] SQLite database created and seeded (26 records)
- [ ] Azure CLI installed and authenticated
- [ ] Azure Developer CLI (azd) installed
- [ ] Correct subscription selected
- [ ] Environment variables configured
- [ ] Storage account tags updated
- [ ] SQL admin credentials generated

### Infrastructure ✓

- [ ] Resource group created
- [ ] App Service Plan (B1) provisioned
- [ ] App Service created
- [ ] Key Vault deployed
- [ ] Application Insights configured
- [ ] Log Analytics workspace created
- [ ] Storage Account with tags deployed
- [ ] Azure SQL Server created
- [ ] Azure SQL Database (Basic) created
- [ ] Playwright workspace provisioned
- [ ] RBAC roles assigned

### Database Migration ✓

- [ ] SQLite data exported
- [ ] Azure SQL schema created
- [ ] Test data migrated (26 records)
- [ ] Connection strings configured
- [ ] Entity Framework contexts updated
- [ ] Migration verified

### Application ✓

- [ ] Application built successfully
- [ ] Unit tests passing
- [ ] Application deployed to App Service
- [ ] App settings configured
- [ ] Connection strings in Key Vault
- [ ] Application accessible via URL
- [ ] Basic functionality verified

### GitHub Integration ✓

- [ ] GitHub Actions workflow created
- [ ] Service Principal created
- [ ] GitHub secrets configured
- [ ] Workflow tested
- [ ] Playwright tests integrated
- [ ] CI/CD pipeline functional

### Monitoring ✓

- [ ] Application Insights telemetry flowing
- [ ] Log Analytics queries configured
- [ ] Custom dashboard created
- [ ] Alert rules configured
- [ ] Diagnostic logs enabled

## Rollback Plan

### Quick Rollback (< 5 minutes)

```powershell
# Revert to previous deployment slot
az webapp deployment slot swap \
  --resource-group rg-calculator-prod-eastus2 \
  --name <app-name> \
  --slot staging \
  --target-slot production
```

### Full Rollback (10-15 minutes)

```powershell
# Delete resource group (removes all resources)
az group delete --name rg-calculator-prod-eastus2 --yes --no-wait

# Re-provision from previous state
azd provision --environment calculator-prod-backup
azd deploy --environment calculator-prod-backup
```

### Database Rollback

```powershell
# Restore from point-in-time backup
az sql db restore \
  --resource-group rg-calculator-prod-eastus2 \
  --server <sql-server-name> \
  --name <db-name> \
  --dest-name <db-name>-restored \
  --time "2025-11-07T00:00:00Z"
```

## Cost Estimation

### Monthly Cost Breakdown (USD)

| Resource | SKU/Tier | Estimated Cost |
|---|---|---|
| App Service Plan | B1 (Basic) | $54.75/month |
| Azure SQL Database | Basic (5 DTU) | $4.90/month |
| Application Insights | Pay-as-you-go | $5-10/month |
| Log Analytics Workspace | PerGB2018 | $2-5/month |
| Storage Account | Standard_LRS | $0.50-2/month |
| Key Vault | Standard | $0.03/10k operations |
| Playwright Testing | Pay-per-minute | $5-15/month |

**Total Estimated Cost:** $72-92/month

**Cost Optimization:**
- Use Azure Dev/Test pricing (if applicable)
- Enable auto-pause for SQL Database (serverless)
- Configure retention policies for logs
- Use reserved instances for long-term (not applicable for B1)

## Security Considerations

### Implemented Security Measures

1. **Key Vault Integration**
   - All secrets stored in Key Vault
   - Managed Identity for App Service
   - No secrets in code or configuration files

2. **Network Security**
   - HTTPS enforced
   - TLS 1.2 minimum
   - Azure SQL firewall rules
   - Private endpoint ready (future enhancement)

3. **Storage Account**
   - EntraID authentication enabled
   - Key-based auth allowed (tagged for exception)
   - Public blob access disabled
   - HTTPS-only traffic

4. **SQL Database**
   - Azure AD authentication
   - Encrypted at rest
   - TLS 1.2 enforced
   - Auditing enabled

5. **Application Security**
   - Managed Identity for resource access
   - RBAC for fine-grained permissions
   - Application Insights data encryption

### Security Best Practices

- Rotate SQL admin password regularly
- Review Key Vault access policies monthly
- Enable Azure Defender for SQL
- Configure backup and retention policies
- Implement geo-redundancy for production
- Use Azure Front Door for DDoS protection (future)

## Testing Strategy

### Pre-Deployment Testing

1. **Local Testing**
   ```powershell
   dotnet test
   npm test
   ```

2. **Integration Testing**
   ```powershell
   # Test against local SQL Server
   $env:SqlConnectionString = "Server=localhost;Database=CalculatorTests;Trusted_Connection=True;"
   dotnet test
   ```

### Post-Deployment Testing

1. **Smoke Tests**
   ```powershell
   # Health check
   Invoke-WebRequest -Uri "$appUrl/health" -Method GET

   # Calculator page
   Invoke-WebRequest -Uri "$appUrl" -Method GET
   ```

2. **Functional Tests**
   ```powershell
   # Run Playwright tests against Azure
   $env:BASE_URL = $appUrl
   npm test
   ```

3. **Performance Tests**
   ```powershell
   # Load test using Azure Load Testing (optional)
   az load test create \
     --name calculator-load-test \
     --resource-group rg-calculator-prod-eastus2 \
     --test-plan load-test.jmx
   ```

## Troubleshooting Guide

### Common Issues and Resolutions

#### Issue 1: Deployment Fails - "Resource not found"

**Solution:**
```powershell
# Refresh environment
azd env refresh -e calculator-prod

# Re-run deployment
azd deploy
```

#### Issue 2: SQL Connection Timeout

**Solution:**
```powershell
# Add your IP to SQL firewall
$myIp = (Invoke-WebRequest -Uri "https://ifconfig.me/ip").Content.Trim()
az sql server firewall-rule create \
  --resource-group rg-calculator-prod-eastus2 \
  --server <sql-server-name> \
  --name AllowMyIP \
  --start-ip-address $myIp \
  --end-ip-address $myIp
```

#### Issue 3: Key Vault Access Denied

**Solution:**
```powershell
# Grant access policy
az keyvault set-policy \
  --name <keyvault-name> \
  --object-id <app-service-principal-id> \
  --secret-permissions get list
```

#### Issue 5: App Shows Placeholder Instead of Application

**Symptoms:**
- Deployment succeeds but shows "Your web app is running and waiting for your content"
- Logs show: `WARNING: Found files: 'calculator.web.runtimeconfig.json, calculator.runtimeconfig.json'`
- Default hosting start page displayed instead of Calculator app

**Solution:**
```powershell
# Configure explicit startup command
az webapp config set \
  --name <app-name> \
  --resource-group rg-calculator-prod-eastus2 \
  --startup-file "dotnet calculator.web.dll"

# Restart the application
az webapp restart \
  --name <app-name> \
  --resource-group rg-calculator-prod-eastus2

# Wait 30-60 seconds, then verify
Start-Process "https://<app-name>.azurewebsites.net"
```

**Root Cause:** Azure App Service detected multiple runtime configuration files and couldn't determine which application to run, defaulting to the placeholder app.

## Post-Migration Tasks

### Day 1 (Immediate)

- [ ] Verify all services running
- [ ] Test critical user flows
- [ ] Monitor Application Insights for errors
- [ ] Review deployment logs
- [ ] Notify stakeholders of successful deployment

### Week 1

- [ ] Review performance metrics
- [ ] Optimize slow queries
- [ ] Fine-tune alert thresholds
- [ ] Configure backup policies
- [ ] Document any configuration changes

### Month 1

- [ ] Review cost analysis
- [ ] Optimize resource utilization
- [ ] Review security audit logs
- [ ] Plan for scaling (if needed)
- [ ] Update documentation

## Maintenance Plan

### Daily

- Monitor Application Insights dashboard
- Review error logs
- Check alert notifications

### Weekly

- Review performance metrics
- Analyze cost trends
- Check backup status
- Review security alerts

### Monthly

- Rotate secrets and keys
- Review and update alert rules
- Analyze usage patterns
- Plan capacity adjustments
- Security patch reviews

### Quarterly

- Disaster recovery drill
- Cost optimization review
- Security audit
- Update dependencies
- Architecture review

## References

### Documentation

- [Azure Developer CLI (azd)](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/)
- [Bicep Documentation](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/)
- [Azure App Service](https://learn.microsoft.com/en-us/azure/app-service/)
- [Azure SQL Database](https://learn.microsoft.com/en-us/azure/azure-sql/)
- [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
- [Azure Playwright Testing](https://learn.microsoft.com/en-us/azure/playwright-testing/)
- [GitHub Actions for Azure](https://learn.microsoft.com/en-us/azure/developer/github/github-actions)

### Related PRDs

- `prd-configure-playwright-testing.md`
- `prd-migrate-to-azure-with-azd.md`
- `prd-sql-crud-operations.md`
- `prd-csharp-basic-calculator-solution.md`
- `prd-refactor-as-blazor-app.md`

## Appendix A: Command Reference

### Essential azd Commands

```powershell
# Initialize new environment
azd init

# Set environment
azd env select calculator-prod

# Set environment variables
azd env set AZURE_LOCATION eastus2

# Get environment values
azd env get-values

# Provision infrastructure
azd provision

# Deploy application
azd deploy

# Provision and deploy
azd up

# Monitor deployment
azd monitor

# View logs
azd logs

# Clean up resources
azd down
```

### Essential Azure CLI Commands

```powershell
# Login
az login
az account set --subscription <subscription-id>

# List resources
az resource list --resource-group <rg-name> --output table

# SQL commands
az sql server list --output table
az sql db list --server <server-name> --output table

# Key Vault commands
az keyvault secret list --vault-name <vault-name> --output table
az keyvault secret show --vault-name <vault-name> --name <secret-name>

# App Service commands
az webapp list --output table
az webapp log tail --name <app-name> --resource-group <rg-name>
az webapp config appsettings list --name <app-name> --resource-group <rg-name>
```

## Appendix B: SQL Migration Scripts

### Export Script (Complete)

```powershell
# File: scripts/Export-SQLiteToJson.ps1

$ErrorActionPreference = "Stop"

$dbPath = "calculator_tests.db"
$outputPath = "data-export.json"

if (-not (Test-Path $dbPath)) {
    Write-Error "Database file not found: $dbPath"
    exit 1
}

Write-Host "Exporting data from SQLite database..." -ForegroundColor Cyan

# Export all records as JSON array
$jsonData = sqlite3 $dbPath @"
SELECT json_group_array(
    json_object(
        'Id', Id,
        'TestName', TestName,
        'Category', Category,
        'FirstOperand', FirstOperand,
        'SecondOperand', SecondOperand,
        'Operation', Operation,
        'ExpectedResult', ExpectedResult,
        'Description', Description,
        'IsActive', IsActive,
        'CreatedAt', CreatedAt
    )
) FROM TestCases;
"@

$jsonData | Out-File -FilePath $outputPath -Encoding UTF8
Write-Host "Data exported to $outputPath" -ForegroundColor Green
Write-Host "Record count: $((Get-Content $outputPath | ConvertFrom-Json).Count)" -ForegroundColor Yellow
```

### Import Script (Complete)

```powershell
# File: scripts/Import-JsonToAzureSQL.ps1

param(
    [Parameter(Mandatory=$true)]
    [string]$ConnectionString,
    
    [Parameter(Mandatory=$false)]
    [string]$JsonFile = "data-export.json"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $JsonFile)) {
    Write-Error "JSON file not found: $JsonFile"
    exit 1
}

Write-Host "Importing data to Azure SQL..." -ForegroundColor Cyan

# Load JSON data
$data = Get-Content $JsonFile | ConvertFrom-Json

# Build INSERT statements
$insertStatements = foreach ($record in $data) {
    @"
INSERT INTO TestCases (TestName, Category, FirstOperand, SecondOperand, Operation, ExpectedResult, Description, IsActive, CreatedAt)
VALUES ('$($record.TestName)', '$($record.Category)', $($record.FirstOperand), $($record.SecondOperand), '$($record.Operation)', $($record.ExpectedResult), '$($record.Description)', $($record.IsActive), '$($record.CreatedAt)');
"@
}

# Execute via sqlcmd or Invoke-Sqlcmd
$sqlFile = "temp-import.sql"
$insertStatements | Out-File -FilePath $sqlFile -Encoding UTF8

# Execute (requires sqlcmd or Invoke-Sqlcmd)
sqlcmd -S $ConnectionString -i $sqlFile

Remove-Item $sqlFile
Write-Host "Import completed successfully!" -ForegroundColor Green
```

## Appendix C: GitHub Actions Complete Workflow

See section 6.1 for the complete GitHub Actions workflow YAML.

## Appendix D: Environment Variables

### Local Development (.env)

```env
AZURE_ENV_NAME=calculator-prod
AZURE_LOCATION=eastus2
AZURE_SUBSCRIPTION_ID=dee9fea3-65b6-45ad-8b44-f38b05c31fa9
AZURE_TENANT_ID=54d665dd-30f1-45c5-a8d5-d6ffcdb518f9
SqlConnectionString=Data Source=calculator_tests.db
```

### Azure App Service (Application Settings)

```json
{
  "APPLICATIONINSIGHTS_CONNECTION_STRING": "@Microsoft.KeyVault(...)",
  "SqlConnectionString": "@Microsoft.KeyVault(...)",
  "ASPNETCORE_ENVIRONMENT": "Production",
  "WEBSITE_RUN_FROM_PACKAGE": "1"
}
```

---

## Document Approval

| Role | Name | Date | Signature |
|---|---|---|---|
| Project Lead | | | |
| DevOps Engineer | | | |
| Security Reviewer | | | |
| Stakeholder | | | |

---

**Document Version:** 1.0  
**Last Updated:** November 7, 2025  
**Next Review Date:** December 7, 2025
