param location string
param environmentName string
param resourceToken string
param abbrs object
param tags object = {}

// Storage Account with Entra ID + key-based auth
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: '${abbrs.storageStorageAccounts}${resourceToken}'
  location: location
  tags: union(tags, {
    SecurityControl: 'Ignore' // Allow key-based auth bypass per PRD
  })
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    allowSharedKeyAccess: true // Enable key-based auth per PRD
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        blob: {
          enabled: true
        }
        file: {
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
  }
}

// Blob service
resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2023-01-01' = {
  parent: storageAccount
  name: 'default'
  properties: {
    deleteRetentionPolicy: {
      enabled: true
      days: 7
    }
  }
}

// Container for Playwright test results
resource playwrightResultsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  parent: blobService
  name: 'playwright-results'
  properties: {
    publicAccess: 'None'
  }
}

// Container for Playwright test videos
resource playwrightVideosContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  parent: blobService
  name: 'playwright-videos'
  properties: {
    publicAccess: 'None'
  }
}

output storageAccountName string = storageAccount.name
output storageAccountId string = storageAccount.id
output storageAccountPrimaryEndpoints object = storageAccount.properties.primaryEndpoints
