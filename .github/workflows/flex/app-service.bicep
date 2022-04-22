param name string = resourceGroup().name
param location string = resourceGroup().location
param envVars array = []
param appServicePlanId string
param vnetSubnetId string

resource app_service 'Microsoft.Web/sites@2021-03-01' = {
  name: name
  kind: 'app'
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
}
