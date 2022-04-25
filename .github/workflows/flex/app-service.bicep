param resourceGroupName string
param resourceGroupLocation string
param envVars array = []
param appServicePlanId string
param vnetSubnetId string

resource appService 'Microsoft.Web/sites@2021-03-01' = {
  name: replace(resourceGroupName, 'resourcegroup', 'silo')
  location: resourceGroupLocation
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
  name: '${replace(resourceGroupName, 'resourcegroup', 'silo')}/metadata'
  kind: 'web'
  properties: {
    CURRENT_STACK: 'dotnet'
  }
}
