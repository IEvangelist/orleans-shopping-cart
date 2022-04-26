param appName string
param resourceGroupLocation string
param envVars array = []
param appServicePlanId string
param vnetSubnetId string

resource appService 'Microsoft.Web/sites@2021-03-01' = {
  name: appName
  location: resourceGroupLocation
  kind: 'app,linux'
  properties: {
    serverFarmId: appServicePlanId
    virtualNetworkSubnetId: vnetSubnetId
    siteConfig: {
      vnetPrivatePortsCount: 2
      webSocketsEnabled: true
      appSettings: envVars
      linuxFxVersion: 'DOTNET|6.0'
      alwaysOn: true
    }
  }
}

resource appServiceConfig 'Microsoft.Web/sites/config@2021-03-01' = {
  name: '${appService.name}/metadata'
  properties: {
    CURRENT_STACK: 'dotnet'
  }
}
