param location string
param environmentName string
param resourceToken string
param abbrs object
param tags object = {}
param keyVaultName string
param applicationInsightsConnectionString string
param logAnalyticsWorkspaceId string
param storageAccountName string

// App Service Plan - B1 tier per PRD
resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: '${abbrs.webServerFarms}${environmentName}-${resourceToken}'
  location: location
  tags: tags
  sku: {
    name: 'B1'
    tier: 'Basic'
    size: 'B1'
    family: 'B'
    capacity: 1
  }
  kind: 'linux'
  properties: {
    reserved: true // Required for Linux
  }
}

// App Service
resource appService 'Microsoft.Web/sites@2023-01-01' = {
  name: '${abbrs.webSitesAppService}${environmentName}-${resourceToken}'
  location: location
  tags: union(tags, {
    'azd-service-name': 'calculator-web'
  })
  kind: 'app,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|9.0'
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      alwaysOn: true
      http20Enabled: true
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: applicationInsightsConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~3'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_Mode'
          value: 'Recommended'
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'AZURE_KEY_VAULT_ENDPOINT'
          value: 'https://${keyVaultName}${environment().suffixes.keyvaultDns}/'
        }
        {
          name: 'AZURE_STORAGE_ACCOUNT_NAME'
          value: storageAccountName
        }
      ]
      healthCheckPath: '/health'
    }
  }
}

// Configure diagnostic settings for App Service
resource appServiceDiagnostics 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: 'app-service-diagnostics'
  scope: appService
  properties: {
    workspaceId: logAnalyticsWorkspaceId
    logs: [
      {
        category: 'AppServiceHTTPLogs'
        enabled: true
        retentionPolicy: {
          enabled: true
          days: 30
        }
      }
      {
        category: 'AppServiceConsoleLogs'
        enabled: true
        retentionPolicy: {
          enabled: true
          days: 30
        }
      }
      {
        category: 'AppServiceAppLogs'
        enabled: true
        retentionPolicy: {
          enabled: true
          days: 30
        }
      }
      {
        category: 'AppServicePlatformLogs'
        enabled: true
        retentionPolicy: {
          enabled: true
          days: 30
        }
      }
    ]
    metrics: [
      {
        category: 'AllMetrics'
        enabled: true
        retentionPolicy: {
          enabled: true
          days: 30
        }
      }
    ]
  }
}

output appServiceName string = appService.name
output appServiceId string = appService.id
output appServiceUrl string = 'https://${appService.properties.defaultHostName}'
output appServicePrincipalId string = appService.identity.principalId
output appServicePlanName string = appServicePlan.name
output appServicePlanId string = appServicePlan.id
