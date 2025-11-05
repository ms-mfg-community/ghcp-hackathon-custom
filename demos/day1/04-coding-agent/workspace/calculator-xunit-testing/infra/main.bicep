targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

@description('Id of the user or app to assign application roles')
param principalId string = ''

@description('Tags that should be applied to all resources')
param tags object = {}

@description('Tenant ID for Azure resources')
param tenantId string = '54d665dd-30f1-45c5-a8d5-d6ffcdb518f9'

@description('Subscription ID for Azure resources')
param subscriptionId string = 'dee9fea3-65b6-45ad-8b44-f38b05c31fa9'

@description('SQL Server administrator login')
@secure()
param sqlAdminLogin string = 'sqladmin'

@description('SQL Server administrator password')
@secure()
param sqlAdminPassword string

// Generate unique suffix for globally unique resource names
var abbrs = loadJsonContent('abbreviations.json')
var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var prefix = '${abbrs.resourcesResourceGroups}${environmentName}'

// Organize resources in a resource group
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: '${abbrs.resourcesResourceGroups}${environmentName}-${location}'
  location: location
  tags: union(tags, {
    'azd-env-name': environmentName
    'environment': environmentName
    'project': 'calculator'
  })
}

// Deploy Log Analytics and Application Insights first (dependency for other resources)
module monitoring './modules/monitoring.bicep' = {
  name: 'monitoring'
  scope: rg
  params: {
    location: location
    environmentName: environmentName
    resourceToken: resourceToken
    abbrs: abbrs
    tags: tags
  }
}

// Deploy Key Vault
module keyVault './modules/keyvault.bicep' = {
  name: 'keyvault'
  scope: rg
  params: {
    location: location
    environmentName: environmentName
    resourceToken: resourceToken
    abbrs: abbrs
    tags: tags
    tenantId: tenantId
    principalId: principalId
  }
}

// Deploy SQL Server and Database
module sql './modules/sql.bicep' = {
  name: 'sql'
  scope: rg
  params: {
    location: location
    environmentName: environmentName
    resourceToken: resourceToken
    abbrs: abbrs
    tags: tags
    sqlAdminLogin: sqlAdminLogin
    sqlAdminPassword: sqlAdminPassword
    keyVaultName: keyVault.outputs.keyVaultName
  }
}

// Deploy Storage Account
module storage './modules/storage.bicep' = {
  name: 'storage'
  scope: rg
  params: {
    location: location
    environmentName: environmentName
    resourceToken: resourceToken
    abbrs: abbrs
    tags: tags
  }
}

// Deploy Playwright Testing Workspace
module playwright './modules/playwright.bicep' = {
  name: 'playwright'
  scope: rg
  params: {
    location: location
    environmentName: environmentName
    resourceToken: resourceToken
    abbrs: abbrs
    tags: tags
    storageAccountName: storage.outputs.storageAccountName
  }
}

// Deploy App Service Plan and App Service
module appService './modules/app-service.bicep' = {
  name: 'app-service'
  scope: rg
  params: {
    location: location
    environmentName: environmentName
    resourceToken: resourceToken
    abbrs: abbrs
    tags: tags
    keyVaultName: keyVault.outputs.keyVaultName
    applicationInsightsConnectionString: monitoring.outputs.applicationInsightsConnectionString
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    storageAccountName: storage.outputs.storageAccountName
  }
}

// Grant App Service managed identity access to Key Vault
module keyVaultRoleAssignment './modules/keyvault-role-assignment.bicep' = {
  name: 'keyvault-role-assignment'
  scope: rg
  params: {
    keyVaultName: keyVault.outputs.keyVaultName
    principalId: appService.outputs.appServicePrincipalId
  }
  dependsOn: [
    appService
    keyVault
  ]
}

// Grant App Service managed identity access to Storage Account
module storageRoleAssignment './modules/storage-role-assignment.bicep' = {
  name: 'storage-role-assignment'
  scope: rg
  params: {
    storageAccountName: storage.outputs.storageAccountName
    principalId: appService.outputs.appServicePrincipalId
  }
  dependsOn: [
    appService
    storage
  ]
}

// Outputs
output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenantId
output AZURE_SUBSCRIPTION_ID string = subscriptionId
output AZURE_RESOURCE_GROUP string = rg.name

// App Service outputs
output APP_SERVICE_NAME string = appService.outputs.appServiceName
output APP_SERVICE_URL string = appService.outputs.appServiceUrl
output APP_SERVICE_PLAN_NAME string = appService.outputs.appServicePlanName

// Monitoring outputs
output APPLICATION_INSIGHTS_NAME string = monitoring.outputs.applicationInsightsName
output APPLICATION_INSIGHTS_CONNECTION_STRING string = monitoring.outputs.applicationInsightsConnectionString
output LOG_ANALYTICS_WORKSPACE_NAME string = monitoring.outputs.logAnalyticsWorkspaceName

// Key Vault outputs
output KEY_VAULT_NAME string = keyVault.outputs.keyVaultName
output KEY_VAULT_URI string = keyVault.outputs.keyVaultUri

// SQL outputs
output SQL_SERVER_NAME string = sql.outputs.sqlServerName
output SQL_DATABASE_NAME string = sql.outputs.sqlDatabaseName

// Storage outputs
output STORAGE_ACCOUNT_NAME string = storage.outputs.storageAccountName

// Playwright outputs
output PLAYWRIGHT_WORKSPACE_NAME string = playwright.outputs.playwrightWorkspaceName
