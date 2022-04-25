param name string
param location string
param envVars array = []
param appServicePlanId string
param vnetSubnetId string

resource appService 'Microsoft.Web/sites@2021-03-01' = {
  name: name
  location: location
  properties: {
    serverFarmId: appServicePlanId
    virtualNetworkSubnetId: vnetSubnetId
    siteConfig: {
      vnetPrivatePortsCount: 2
      webSocketsEnabled: true
      appSettings: envVars
      netFrameworkVersion: 'v6.0'
      alwaysOn: true
    }
  }
  dependsOn: [
    appServiceStack
  ]
}

resource appServiceStack 'Microsoft.Web/sites/config@2021-03-01' = {
  name: '${name}/metadata'
  kind: 'web'
  properties: {
    CURRENT_STACK: 'dotnet'
  }
}
