param resourceGroupName string
param resourceGroupLocation string

resource logs 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: replace(resourceGroupName, 'resourcegroup', 'logs')
  location: resourceGroupLocation
  properties: any({
    retentionInDays: 30
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  })
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: replace(resourceGroupName, 'resourcegroup', 'insights')
  location: resourceGroupLocation
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logs.id
  }
}
