param resourceGroupName string = resourceGroup().name
param resourceGroupLocation string = resourceGroup().location

module storageModule 'storage.bicep' = {
  name: replace(resourceGroupName, '-resourcegroup', 'StorageModule')
  params: {
    name: replace(resourceGroupName, '-resourcegroup', 'storage')
    resourceGroupLocation: resourceGroupLocation
  }
}

module logsModule 'logs-and-insights.bicep' = {
  name: replace(resourceGroupName, '-resourcegroup', 'LogsAndInsightsModule')
  params: {
    operationalInsightsName: replace(resourceGroupName, 'resourcegroup', 'logs')
    appServiceName: replace(resourceGroupName, '-resourcegroup', '-app-silo')
    appInsightsName: replace(resourceGroupName, 'resourcegroup', 'insights')
    resourceGroupLocation: resourceGroupLocation
  }
  dependsOn: [
    siloModule
  ]
}

var siloConfig = [
  {
    name: 'ORLEANS_SILO_NAME'
    value: 'Orleans Shopping Cart'
  }
  {
    name: 'ORLEANS_AZURE_STORAGE_CONNECTION_STRING'
    value: storageModule.outputs.connectionString
  }
]

resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = {
  name: replace(resourceGroupName, 'resourcegroup', 'vnet')
  location: resourceGroupLocation
  properties: {
    addressSpace: {
      addressPrefixes: [
        '172.17.0.0/16'
      ]
    }
    subnets: [
      {
        name: 'default'
        properties: {
          addressPrefix: '172.17.0.0/24'
          delegations: [
            {
              name: 'delegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
        }
      }
    ]
  }
}

module siloModule 'app-service.bicep' = {
  name: replace(resourceGroupName, '-resourcegroup', 'SiloModule')
  params: {
    appName: replace(resourceGroupName, '-resourcegroup', '-app-silo')
    resourceGroupName: resourceGroupName
    resourceGroupLocation: resourceGroupLocation
    vnetSubnetId: vnet.properties.subnets[0].id
    envVars: siloConfig
  }
}
