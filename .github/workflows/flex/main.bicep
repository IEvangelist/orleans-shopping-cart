param resourceGroupName string = resourceGroup().name
param resourceGroupLocation string = resourceGroup().location

module storage 'storage.bicep' = {
  name: replace(resourceGroupName, '-resourcegroup', 'StorageModule')
  params: {
    name: replace(resourceGroupName, '-resourcegroup', 'storage')
    resourceGroupLocation: resourceGroupLocation
  }
}

var sharedConfig = [
  {
    name: 'ORLEANS_AZURE_STORAGE_CONNECTION_STRING'
    value: format('DefaultEndpointsProtocol=https;AccountName=${storage.outputs.storageName};AccountKey=${storage.outputs.accountKey};EndpointSuffix=core.windows.net')
  }
]

var siloConfig = [
  {
    name: 'ORLEANS_SILO_NAME'
    value: 'Orleans Shopping Cart'
  }
]

module logs 'logs-and-insights.bicep' = {
  name: replace(resourceGroupName, '-resourcegroup', 'LogsAndInsightsModule')
  params: {
    operationalInsightsName: replace(resourceGroupName, 'resourcegroup', 'logs')
    appInsightsName: replace(resourceGroupName, 'resourcegroup', 'insights')
    resourceGroupLocation: resourceGroupLocation
  }
}

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

resource appServicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: '${resourceGroupName}-plan'
  location: resourceGroupLocation
  kind: 'web'
  sku: {
    name: 'S1'
    capacity: 1
  }
}

module silo 'app-service.bicep' = {
  name: replace(resourceGroupName, '-resourcegroup', 'SiloModule')
  params: {
    appName: replace(resourceGroupName, '-resourcegroup', '-app-silo')
    resourceGroupLocation: resourceGroupLocation
    appServicePlanId: appServicePlan.id
    vnetSubnetId: vnet.properties.subnets[0].id
    envVars: union(sharedConfig, siloConfig)
  }
}
