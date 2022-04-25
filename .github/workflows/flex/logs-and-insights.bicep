param name string = resourceGroup().name
param location string = resourceGroup().location

resource logs 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: '${name}logs'
  location: location
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

resource appInsightsComponents 'Microsoft.Insights/components@2020-02-02' = {
  name: replace(name, 'resourcegroup', 'insights')
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logs.id
  }
}
