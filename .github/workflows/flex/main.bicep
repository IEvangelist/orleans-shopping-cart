param resourceGroupName string = resourceGroup().name
param resourceGroupLocation string = resourceGroup().location

module storage 'storage.bicep' = {
  name: toLower('${resourceGroup().name}strg')
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
  name: toLower('${resourceGroup().name}monitoring')
  params: {
    operationalInsightsName: toLower('${resourceGroup().name}logs')
    appInsightsName: toLower('${resourceGroup().name}ai')
    resourceGroupLocation: resourceGroupLocation
  }
}

resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = {
  name: toLower('${resourceGroup().name}vnet')
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
  name: toLower('${resourceGroup().name}plan')
  location: resourceGroupLocation
  kind: 'app'
  sku: {
    name: 'S1'
    capacity: 1
  }
}

module silo 'app-service.bicep' = {
  name: toLower('${resourceGroup().name}silo')
  params: {
    appName: toLower('${resourceGroup().name}silo')
    resourceGroupLocation: resourceGroupLocation
    appServicePlanId: appServicePlan.id
    vnetSubnetId: vnet.properties.subnets[0].id
    envVars: union(sharedConfig, siloConfig)
  }
}
