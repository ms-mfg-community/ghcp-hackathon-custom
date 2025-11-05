# Product Requirements Document: Azure Migration with Azure Developer CLI

## Document Information

- **Version:** 1.0
- **Author:** GitHub Copilot
- **Date:** November 5, 2025
- **Status:** Draft

## Executive Summary

This document defines the requirements for migrating the Blazor Server calculator application to Microsoft Azure using Azure Developer CLI (azd), Bicep infrastructure-as-code, and GitHub Actions CI/CD. The solution implements a production-ready cloud architecture with App Service hosting, SQL Database, Key Vault, comprehensive monitoring, secure storage, and Playwright Testing capabilities.

## Problem Statement

The Blazor calculator application currently runs only in local development environments. To enable production deployment, scalability, monitoring, and automated testing, the application requires cloud infrastructure with proper security controls, observability, and CI/CD automation. Manual deployment processes are error-prone and don't provide the reliability, monitoring, or testing capabilities required for production workloads.

## Goals and Objectives

- Deploy Blazor Server calculator application to Azure App Service with production-ready configuration
- Implement infrastructure-as-code using Bicep for repeatable, version-controlled deployments
- Establish CI/CD pipeline using GitHub Actions for automated build, test, and deployment
- Implement comprehensive monitoring with Application Insights and Log Analytics
- Secure credentials and secrets using Azure Key Vault with managed identity authentication
- Provision Azure SQL Database infrastructure for future data persistence migration
- Enable secure storage with dual authentication (Entra ID + key-based) for Playwright artifacts
- Set up Playwright Testing workspace for automated UI testing
- Maintain in-memory calculator history for current iteration (SQL migration deferred)

## Scope

### In Scope

- Azure Developer CLI (azd) project initialization and configuration
- Bicep infrastructure modules for all Azure resources
- Azure App Service deployment (B1 tier, East US 2 region)
- Azure SQL Server and Database (Basic tier) - infrastructure only
- Azure Key Vault with RBAC and managed identity integration
- Application Insights and Log Analytics workspace with 30-day retention
- Azure Storage Account with Entra ID + shared key authentication
- Microsoft Playwright Testing workspace in East US 2
- GitHub Actions workflows for CI/CD and Playwright testing
- Application configuration updates for Azure environment
- Health check endpoint implementation
- Deployment documentation and runbook

### Out of Scope

- Migration of calculator history from in-memory to SQL Database (deferred to future iteration)
- CSV test data migration (no CSV files currently exist)
- Authentication/authorization implementation
- Custom domain and SSL certificate configuration
- Multi-region deployment or geo-redundancy
- Azure Front Door or CDN configuration
- Container-based deployment (using native App Service .NET runtime)
- Database schema design and Entity Framework implementation

## User Stories / Use Cases

### DevOps Engineer

- As a DevOps engineer, I want to deploy the application using `azd up` so that infrastructure provisioning and application deployment happen in a single command
- As a DevOps engineer, I want all infrastructure defined in Bicep so that deployments are repeatable and version-controlled
- As a DevOps engineer, I want GitHub Actions to automatically deploy on merge to main so that releases are automated
- As a DevOps engineer, I want comprehensive logging in Log Analytics so that I can troubleshoot issues efficiently

### Developer

- As a developer, I want Application Insights integrated so that I can monitor application performance and errors
- As a developer, I want secrets stored in Key Vault so that credentials are never in source control
- As a developer, I want Playwright Testing configured so that I can run automated UI tests against the deployed application
- As a developer, I want health check endpoints so that Azure can monitor application availability

### Operations Team

- As an operations team member, I want automated alerts for application failures so that I can respond to incidents quickly
- As an operations team member, I want SQL Database infrastructure ready so that future data persistence migration is straightforward
- As an operations team member, I want storage account configured for test artifacts so that Playwright results are retained
- As an operations team member, I want managed identities so that no credentials need rotation

## Functional Requirements

| Requirement ID | Description |
|---|---|
| FR-1 | The solution shall initialize an azd project with azure.yaml defining the calculator-web service |
| FR-2 | The solution shall create a main.bicep file orchestrating all Azure resource deployments |
| FR-3 | The solution shall deploy Azure App Service Plan with B1 tier and AlwaysOn enabled |
| FR-4 | The solution shall deploy Azure App Service with .NET 9.0 Linux runtime stack |
| FR-5 | The solution shall enable managed identity on App Service for Key Vault and Storage access |
| FR-6 | The solution shall deploy Azure SQL Server with Entra ID admin authentication |
| FR-7 | The solution shall deploy Azure SQL Database with Basic tier (2GB max size) |
| FR-8 | The solution shall configure SQL Server firewall to allow Azure services |
| FR-9 | The solution shall deploy Azure Key Vault with RBAC authorization model |
| FR-10 | The solution shall store SQL connection string in Key Vault as secret |
| FR-11 | The solution shall grant App Service managed identity "Key Vault Secrets User" role |
| FR-12 | The solution shall deploy Log Analytics workspace with 30-day log retention |
| FR-13 | The solution shall deploy Application Insights linked to Log Analytics workspace |
| FR-14 | The solution shall configure App Service diagnostic settings to stream logs to Log Analytics |
| FR-15 | The solution shall deploy Azure Storage Account (StorageV2) with Entra ID enabled |
| FR-16 | The solution shall enable shared key access on Storage Account with tag "SecurityControl: Ignore" |
| FR-17 | The solution shall create blob containers for Playwright test results and recordings |
| FR-18 | The solution shall grant App Service managed identity "Storage Blob Data Contributor" role |
| FR-19 | The solution shall deploy Microsoft Playwright Testing workspace in East US 2 |
| FR-20 | The solution shall create GitHub Actions workflow for azd deployment on main branch merge |
| FR-21 | The solution shall create GitHub Actions workflow for Playwright UI tests on pull requests |
| FR-22 | The solution shall add Application Insights configuration to appsettings.json |
| FR-23 | The solution shall add health check endpoint at /health in Program.cs |
| FR-24 | The solution shall configure App Service app settings to reference Key Vault secrets |
| FR-25 | The solution shall maintain in-memory CalculationHistoryService (no database migration) |

## Non-Functional Requirements

### Performance

- Application must start within 60 seconds of App Service cold start
- Health check endpoint must respond within 2 seconds
- Application Insights telemetry must not add more than 100ms latency

### Scalability

- App Service B1 tier supports vertical scaling to higher tiers without redeployment
- Architecture supports future migration to scale-out with multiple instances

### Security

- All credentials must be stored in Azure Key Vault
- Managed identity must be used for all Azure service authentication
- HTTPS must be enforced on all endpoints
- Storage Account must support both Entra ID and key-based authentication
- SQL Server must use Entra ID authentication for admin access
- Key Vault must use RBAC (not access policies)

### Reliability

- Application must implement proper error handling and logging
- Health check endpoint must accurately reflect application state
- App Service diagnostic logs must be retained for 30 days
- Basic SQL Database tier provides 99.99% SLA

### Maintainability

- All infrastructure must be defined as code in Bicep
- Bicep modules must be organized by resource type
- GitHub Actions workflows must be self-documenting
- Deployment documentation must include troubleshooting guide

### Compliance

- Storage Account tag "SecurityControl: Ignore" required for key-based auth policy bypass
- All resources must be deployed to East US 2 region per organizational requirements
- Tenant ID: 54d665dd-30f1-45c5-a8d5-d6ffcdb518f9
- Subscription ID: dee9fea3-65b6-45ad-8b44-f38b05c31fa9

## Technical Architecture

### Azure Resources

```
Resource Group: rg-calculator-eastus2
├── App Service Plan (plan-calculator-eastus2) - B1 tier
├── App Service (app-calculator-eastus2) - .NET 9.0 Linux
│   └── Managed Identity (system-assigned)
├── SQL Server (sql-calculator-eastus2)
│   └── SQL Database (sqldb-calculator) - Basic tier
├── Key Vault (kv-calculator-eastus2)
│   ├── Secret: SqlConnectionString
│   └── Secret: ApplicationInsights-InstrumentationKey
├── Log Analytics Workspace (log-calculator-eastus2)
├── Application Insights (appi-calculator-eastus2)
├── Storage Account (stcalculatoreastus2)
│   ├── Container: playwright-results
│   ├── Container: playwright-videos
│   └── Tag: SecurityControl=Ignore
└── Playwright Testing Workspace (pw-calculator-eastus2)
```

### Infrastructure Organization

```
infra/
├── main.bicep                          # Main orchestration
├── main.parameters.json                # Environment parameters
├── abbreviations.bicep                 # Resource naming conventions
└── modules/
    ├── app-service.bicep              # App Service Plan + App Service
    ├── sql.bicep                      # SQL Server + Database
    ├── keyvault.bicep                 # Key Vault + Secrets + RBAC
    ├── monitoring.bicep               # Log Analytics + App Insights
    ├── storage.bicep                  # Storage Account + Containers + RBAC
    └── playwright.bicep               # Playwright Testing Workspace
```

### GitHub Actions Workflows

```
.github/workflows/
├── azure-deploy.yml                   # Main CI/CD pipeline
│   ├── Trigger: push to main
│   ├── Build: dotnet build + test
│   ├── Deploy: azd deploy
│   └── Post-deployment: smoke tests
└── playwright-tests.yml               # UI testing pipeline
    ├── Trigger: pull_request
    ├── Deploy to staging slot
    ├── Run Playwright tests
    └── Publish test artifacts
```

## Application Configuration Updates

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  },
  "ApplicationInsights": {
    "ConnectionString": ""  // Populated from environment variable
  },
  "AllowedHosts": "*"
}
```

### Program.cs Updates

- Add `builder.Services.AddApplicationInsightsTelemetry()`
- Add `builder.Services.AddHealthChecks()`
- Add `app.MapHealthChecks("/health")`
- Configure Key Vault integration (future iteration)

## Deployment Workflow

### Initial Deployment

1. **Prerequisites Installation**
   - Install Azure Developer CLI (azd)
   - Install Azure CLI (az)
   - Install .NET 9.0 SDK
   - Install GitHub CLI (gh)

2. **Azure Authentication**
   ```bash
   azd auth login
   az login --tenant 54d665dd-30f1-45c5-a8d5-d6ffcdb518f9
   ```

3. **Environment Initialization**
   ```bash
   cd calculator-xunit-testing
   azd init
   azd env new calculator-prod
   azd env set AZURE_SUBSCRIPTION_ID dee9fea3-65b6-45ad-8b44-f38b05c31fa9
   azd env set AZURE_LOCATION eastus2
   ```

4. **Infrastructure and Application Deployment**
   ```bash
   azd up
   ```

5. **GitHub Actions Setup**
   ```bash
   azd pipeline config
   ```

### CI/CD Deployment

1. Developer pushes code to feature branch
2. Create pull request to main branch
3. Playwright tests run automatically on PR
4. On PR approval and merge to main:
   - GitHub Actions triggers azure-deploy.yml
   - Builds .NET 9 application
   - Runs xUnit and bUnit tests
   - Deploys infrastructure via azd (if changes detected)
   - Deploys application to App Service
   - Runs smoke tests against health endpoint

## Success Criteria / KPIs

- Infrastructure deploys successfully via `azd up` in under 10 minutes
- Application accessible at App Service URL with 200 OK response
- Health check endpoint returns 200 OK status
- Application Insights receiving telemetry within 5 minutes of deployment
- Log Analytics receiving diagnostic logs from App Service
- Calculator functionality works identically to local development
- Playwright Testing workspace accessible and configured
- GitHub Actions workflows execute successfully on merge to main
- All secrets stored in Key Vault (zero secrets in source control)
- Managed identity authentication working for Key Vault and Storage
- SQL Database accessible from App Service (connection test via health check)

## Milestones & Timeline

### Phase 1: Infrastructure Setup (Days 1-2)
- Initialize azd project structure
- Create Bicep modules for all resources
- Deploy infrastructure to Azure
- Verify resource connectivity

### Phase 2: Application Configuration (Day 3)
- Update appsettings.json for Azure
- Add Application Insights telemetry
- Implement health check endpoint
- Configure Key Vault references

### Phase 3: CI/CD Pipeline (Day 4)
- Create GitHub Actions workflows
- Configure repository secrets
- Test automated deployment
- Set up Playwright Testing integration

### Phase 4: Validation & Documentation (Day 5)
- End-to-end testing in Azure
- Performance validation
- Create deployment runbook
- Knowledge transfer documentation

## Assumptions and Dependencies

### Assumptions

- Azure subscription has quota for B1 App Service in East US 2
- User has Owner or Contributor role on subscription dee9fea3-65b6-45ad-8b44-f38b05c31fa9
- GitHub repository ms-mfg-community/ghcp-hackathon-custom has Actions enabled
- Microsoft Playwright Testing preview available in East US 2 region
- No existing Azure resources with conflicting names
- Application runs successfully in local development environment
- Current in-memory calculator history is acceptable for production (future SQL migration planned)

### Dependencies

- Azure Developer CLI version 1.5.0 or higher
- Azure CLI version 2.50.0 or higher
- .NET 9.0 SDK installed
- GitHub CLI for pipeline configuration
- Bicep CLI (installed with Azure CLI)
- Git version 2.30 or higher
- PowerShell 7.x (for Windows deployment)

## Risks and Mitigations

| Risk | Impact | Likelihood | Mitigation |
|---|---|---|---|
| Playwright Testing preview not available in East US 2 | High | Low | Deploy Playwright workspace to nearest region (East US) or use alternative testing approach |
| App Service B1 tier insufficient for production load | Medium | Medium | Monitor performance metrics; upgrade to S1 tier if needed |
| SQL Database Basic tier too small for future migration | Medium | Low | Basic tier sufficient for initial migration; plan upgrade to Standard tier when migrating history |
| GitHub Actions minutes quota exceeded | Low | Low | Use self-hosted runners or optimize workflow frequency |
| Managed identity permissions not propagating | Medium | Low | Add retry logic and verify RBAC assignments in Bicep |
| Key Vault name conflicts (globally unique) | Low | Medium | Use unique suffix in Bicep (e.g., uniqueString(subscription().id)) |
| Application Insights telemetry delay | Low | Medium | Set realistic expectations; telemetry appears within 5 minutes |

## Open Questions

1. **Staging Environment**: Should we deploy a separate staging environment or use deployment slots on the B1 tier? (B1 does not support slots)
2. **SQL Database Backup**: What backup retention policy should be configured for the SQL Database?
3. **Application Insights Sampling**: Should we implement telemetry sampling to reduce costs?
4. **Custom Domain**: Will a custom domain be configured post-deployment?
5. **Playwright Test Frequency**: How often should Playwright tests run in CI/CD (on every PR, daily, on-demand)?
6. **Alert Recipients**: Who should receive alerts from Application Insights and Log Analytics?

## Appendix A: Resource Naming Conventions

| Resource Type | Naming Pattern | Example |
|---|---|---|
| Resource Group | rg-{app}-{region} | rg-calculator-eastus2 |
| App Service Plan | plan-{app}-{region} | plan-calculator-eastus2 |
| App Service | app-{app}-{region} | app-calculator-eastus2 |
| SQL Server | sql-{app}-{region} | sql-calculator-eastus2 |
| SQL Database | sqldb-{app} | sqldb-calculator |
| Key Vault | kv-{app}-{region} | kv-calculator-eastus2 |
| Log Analytics | log-{app}-{region} | log-calculator-eastus2 |
| App Insights | appi-{app}-{region} | appi-calculator-eastus2 |
| Storage Account | st{app}{region} | stcalculatoreastus2 |
| Playwright Workspace | pw-{app}-{region} | pw-calculator-eastus2 |

## Appendix B: Environment Variables

| Variable | Source | Description |
|---|---|---|
| APPLICATIONINSIGHTS_CONNECTION_STRING | Key Vault | App Insights connection string |
| SqlConnectionString | Key Vault | SQL Database connection string |
| AZURE_STORAGE_ACCOUNT | App Settings | Storage account name |
| PLAYWRIGHT_SERVICE_ENDPOINT | App Settings | Playwright workspace endpoint |
| ASPNETCORE_ENVIRONMENT | App Settings | Environment name (Production) |

## Appendix C: GitHub Actions Secrets

| Secret Name | Description |
|---|---|
| AZURE_CREDENTIALS | Service principal credentials for azd |
| AZURE_SUBSCRIPTION_ID | Subscription ID (dee9fea3-65b6-45ad-8b44-f38b05c31fa9) |
| AZURE_TENANT_ID | Tenant ID (54d665dd-30f1-45c5-a8d5-d6ffcdb518f9) |
| PLAYWRIGHT_SERVICE_ACCESS_TOKEN | Playwright workspace access token |

## Appendix D: Health Check Endpoint

```csharp
// Program.cs health check implementation
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(
        connectionString: builder.Configuration["SqlConnectionString"],
        name: "sql-database",
        failureStatus: HealthStatus.Degraded
    );

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
```

## Revision History

| Version | Date | Author | Changes |
|---|---|---|---|
| 1.0 | 2025-11-05 | GitHub Copilot | Initial PRD creation |
