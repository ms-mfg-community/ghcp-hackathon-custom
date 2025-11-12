# Quick Start: Deploy Calculator App to Azure

**Duration:** 30-45 minutes  
**Last Updated:** November 7, 2025  
**Status:** ✅ Successfully deployed to Azure

## Recent Deployment Summary

**Latest Deployment:** November 7, 2025

### What Was Deployed

✅ **Infrastructure Provisioned:**
- Resource Group: `rg-calculator-prod-eastus2`
- App Service Plan: B1 (Basic tier)
- App Service: Calculator Blazor Server application
- Key Vault: Secret management
- Application Insights: Performance monitoring
- Log Analytics Workspace: Log aggregation
- Storage Account: File storage (with SecurityControl: Ignore tag)
- Playwright Workspace: Web UI testing configuration
- RBAC: Role assignments for Key Vault and Storage access

❌ **SQL Server:** Commented out due to Azure Policy restrictions (MCAPS)

### Key Fixes Applied

1. **Diagnostic Settings Retention:** Removed retention policies from diagnostic settings to comply with Azure Policy
2. **Log Analytics Retention:** Removed retentionInDays property from Log Analytics workspace
3. **Startup Command Configuration:** Set explicit startup command to `dotnet calculator.web.dll` to prevent placeholder page
4. **SQL Module:** Temporarily disabled due to MCAPS policy restrictions

### Known Issues

- **SQL Database:** Not deployed due to policy restrictions. Request exemption or manual deployment required.
- **Initial Placeholder Page:** Resolved by configuring explicit startup command (see Step 7)

### Application URL

**Live Application:** https://app-calculator-prod-c4aj6654c3sbe.azurewebsites.net/

---

## Prerequisites

✅ Azure CLI installed  
✅ Azure Developer CLI (azd) installed  
✅ .NET 9.0 SDK installed  
✅ Node.js 18+ installed  
✅ PowerShell 7+ installed  
✅ Git installed

## Step-by-Step Deployment

**Note:** Your SQLite database already exists with 41 test records, so deployment will be faster!

### Step 1: Verify Prerequisites (5 minutes)

```powershell
# Check Azure CLI
az --version

# Check azd
azd version

# Check .NET
dotnet --version

# Check Node.js
node --version

# Check PowerShell
$PSVersionTable.PSVersion
```

### Step 2: Login to Azure (2 minutes)

```powershell
# Login to Azure
az login

# Set subscription
az account set --subscription "dee9fea3-65b6-45ad-8b44-f38b05c31fa9"

# Verify
az account show

# Login to azd
azd auth login
```

### Step 3: Navigate to Project (1 minute)

```powershell
cd demos\day1\04-coding-agent\workspace\calculator-xunit-testing
```

### Step 4: Verify SQLite Database (1 minute)

**Status:** ✅ Database already exists with 41 test records!

```powershell
# Verify database exists
Test-Path calculator_tests.db  # Should return: True

# Check record count
sqlite3 calculator_tests.db "SELECT COUNT(*) FROM TestCases;"  # Returns: 41

# View sample data
python list_test_cases.py
# OR
.\List-TestCases.ps1
```

**Note:** If you need to reset the database, run `.\Initialize-And-Query-Database.ps1`

### Step 5: Configure Environment Variables (5 minutes)

```powershell
# Get your principal ID
$principalId = az ad signed-in-user show --query id -o tsv

# Update .azure/calculator-prod/.env
# Verify these values:
@"
AZURE_ENV_NAME="calculator-prod"
AZURE_LOCATION="eastus2"
AZURE_SUBSCRIPTION_ID="<your-subscription-id>"
AZURE_TENANT_ID="<your-tenant-id>"
principalId="$principalId"
sqlAdminPassword="<YourSecurePassword123!>"
"@ | Out-File -FilePath .azure\calculator-prod\.env -Encoding utf8

Write-Host "Environment configured!" -ForegroundColor Green
```

### Step 6: Deploy to Azure (15-20 minutes)

```powershell
# One command to provision and deploy
azd up

# This will:
# - Create resource group
# - Deploy all infrastructure (App Service, SQL, Key Vault, etc.)
# - Build the application
# - Deploy to Azure App Service
```

**Note:** The first deployment takes 15-20 minutes. Subsequent deployments are faster (3-5 minutes).

### Step 7: Configure Startup Command (2 minutes)

**Important:** Azure may detect multiple `.runtimeconfig.json` files. Configure the startup command explicitly:

```powershell
# Get App Service name
$appName = azd env get-values | Where-Object { $_ -match 'APP_SERVICE_NAME' } | ForEach-Object { $_.Split('=')[1].Trim('"') }

# Set explicit startup command
az webapp config set --name $appName --resource-group rg-calculator-prod-eastus2 --startup-file "dotnet calculator.web.dll"

# Restart to apply changes
az webapp restart --name $appName --resource-group rg-calculator-prod-eastus2

Write-Host "Startup command configured. Waiting for app to restart..." -ForegroundColor Yellow
Start-Sleep -Seconds 30
```

**Why This is Needed:**
If you see a placeholder page saying "Your web app is running and waiting for your content", it means Azure found multiple runtime config files and defaulted to the hosting placeholder. The explicit startup command fixes this.

### Step 8: Verify Deployment (3 minutes)

```powershell
# Get App Service URL
$appUrl = azd env get-values | Where-Object { $_ -match 'APP_SERVICE_URL' } | ForEach-Object { $_.Split('=')[1].Trim('"') }

Write-Host "Application URL: $appUrl" -ForegroundColor Green

# Open in browser
Start-Process $appUrl

# Test health endpoint
Invoke-WebRequest -Uri "$appUrl/health" -Method GET

# Check logs to verify correct startup
az webapp log tail --name $appName --resource-group rg-calculator-prod-eastus2
```

### Step 9: Migrate Data to Azure SQL (10 minutes)

**Note:** SQL Server module may be commented out due to Azure Policy restrictions. If SQL is not deployed, skip this step or request policy exemption from your Azure administrator.

#### Option A: Using Entity Framework (Recommended)

```powershell
# Get SQL connection string from Key Vault
$kvName = azd env get-values | Where-Object { $_ -match 'KEY_VAULT_NAME' } | ForEach-Object { $_.Split('=')[1].Trim('"') }
$sqlConnString = az keyvault secret show --vault-name $kvName --name SqlConnectionString --query value -o tsv

# Run migration
cd calculator.tests
$env:ConnectionStrings__DefaultConnection = $sqlConnString
dotnet ef database update --context TestDataDbContext

# Verify
dotnet test --filter "FullyQualifiedName~CalculatorDatabaseTests"
```

#### Option B: Using SQL Scripts

```powershell
# Get SQL Server details
$sqlServer = azd env get-values | Where-Object { $_ -match 'SQL_SERVER_NAME' } | ForEach-Object { $_.Split('=')[1].Trim('"') }
$sqlDb = azd env get-values | Where-Object { $_ -match 'SQL_DATABASE_NAME' } | ForEach-Object { $_.Split('=')[1].Trim('"') }

# Create schema and seed data
sqlcmd -S "$sqlServer.database.windows.net" -d $sqlDb -U sqladmin -P "<password>" -Q "
CREATE TABLE TestCases (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TestName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    FirstOperand FLOAT NOT NULL,
    SecondOperand FLOAT NOT NULL,
    Operation NVARCHAR(20) NOT NULL,
    ExpectedResult FLOAT NOT NULL,
    Description NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
"

# Verify (should show 41 records after migration)
sqlcmd -S "$sqlServer.database.windows.net" -d $sqlDb -U sqladmin -P "<password>" -Q "SELECT COUNT(*) FROM TestCases;"
```

### Step 10: Run Playwright Tests (5 minutes)

```powershell
# Install Playwright browsers (first time only)
npx playwright install

# Run tests against Azure-hosted app
$env:BASE_URL = $appUrl
npm test

# View report
npm run test:report
```

### Step 11: View Monitoring (3 minutes)

```powershell
# Get Application Insights details
$appInsightsName = azd env get-values | Where-Object { $_ -match 'APPLICATION_INSIGHTS_NAME' } | ForEach-Object { $_.Split('=')[1].Trim('"') }

Write-Host "Application Insights: $appInsightsName" -ForegroundColor Cyan

# Open in Azure Portal
az monitor app-insights component show --app $appInsightsName --resource-group rg-calculator-prod-eastus2 --query appId -o tsv
```

## Quick Commands Reference

### Deployment

```powershell
# Full deployment
azd up

# Provision only (infrastructure)
azd provision

# Deploy code only
azd deploy

# View environment variables
azd env get-values

# Monitor logs
azd monitor --logs

# Clean up (delete all resources)
azd down
```

### Application

```powershell
# Build
dotnet build

# Run locally
cd calculator.web
dotnet run

# Run tests
dotnet test

# Run Playwright tests
npm test
```

### Azure CLI

```powershell
# List resources
az resource list --resource-group rg-calculator-prod-eastus2 --output table

# View App Service logs
az webapp log tail --name <app-name> --resource-group rg-calculator-prod-eastus2

# Get connection string
az keyvault secret show --vault-name <kv-name> --name SqlConnectionString --query value -o tsv

# Query SQL Database
sqlcmd -S <server>.database.windows.net -d <database> -U sqladmin -P <password> -Q "SELECT * FROM TestCases"
```

## Common Issues

### Issue: `azd up` fails with authentication error

**Solution:**
```powershell
az login
azd auth login
azd up
```

### Issue: SQL connection timeout

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

### Issue: Playwright tests fail

**Solution:**
```powershell
# Verify base URL
echo $env:BASE_URL

# Install browsers
npx playwright install --with-deps

# Run in headed mode for debugging
npx playwright test --headed
```

### Issue: Application shows placeholder page

**Symptoms:** "Your web app is running and waiting for your content" page displayed after deployment

**Solution:**
```powershell
# Configure startup command explicitly
az webapp config set --name <app-name> --resource-group rg-calculator-prod-eastus2 --startup-file "dotnet calculator.web.dll"

# Restart app
az webapp restart --name <app-name> --resource-group rg-calculator-prod-eastus2

# Wait 30-60 seconds, then refresh browser
```

### Issue: Application not loading

**Solution:**
```powershell
# Check App Service status
az webapp show --name <app-name> --resource-group rg-calculator-prod-eastus2 --query state

# Restart App Service
az webapp restart --name <app-name> --resource-group rg-calculator-prod-eastus2

# View logs
az webapp log tail --name <app-name> --resource-group rg-calculator-prod-eastus2
```

## Next Steps

1. **Set up CI/CD:** Configure GitHub Actions workflow (see AZURE_MIGRATION_PLAN.md)
2. **Configure Alerts:** Set up Application Insights alerts
3. **Review Security:** Enable Azure Defender, configure backup policies
4. **Optimize Costs:** Review cost analysis, configure auto-scaling
5. **Documentation:** Update team documentation with deployment procedures

## Resources

- **Full Migration Plan:** [AZURE_MIGRATION_PLAN.md](./AZURE_MIGRATION_PLAN.md)
- **Playwright Testing:** [PLAYWRIGHT_README.md](./PLAYWRIGHT_README.md)
- **Azure Portal:** https://portal.azure.com
- **azd Documentation:** https://learn.microsoft.com/azure/developer/azure-developer-cli/

## Support

For issues or questions:
1. Check [AZURE_MIGRATION_PLAN.md](./AZURE_MIGRATION_PLAN.md) troubleshooting section
2. Review Azure Portal diagnostics
3. Check Application Insights logs
4. Contact DevOps team

---

**Time to Deploy:** 25-35 minutes (first time)  
**Time to Re-deploy:** 3-5 minutes (subsequent)  
**Database Status:** ✅ 41 test records in SQLite (SQL Server not deployed due to policy)  
**Estimated Monthly Cost:** $62-72 USD (without SQL Server)  
**Deployment Status:** ✅ Successfully deployed on November 7, 2025
