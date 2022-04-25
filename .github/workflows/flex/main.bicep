param name string = resourceGroup().name
param location string = resourceGroup().location

module storage 'storage.bicep' = {
  name: toLower('${replace(name, '-resourcegroup', '')}-storage')
  params: {
    name: name
    location: location
  }
}

var shared_config = [
  {
    name: 'ORLEANS_AZURE_STORAGE_CONNECTION_STRING'
    value: format('DefaultEndpointsProtocol=https;AccountName=${storage.outputs.storageName};AccountKey=${storage.outputs.accountKey};EndpointSuffix=core.windows.net')
  }
]

var silo_config = [
  {
    name: 'ORLEANS_SILO_NAME'
    value: 'Orleans Shopping Cart'
  }
]

module logs 'logs-and-insights.bicep' = {
  name: 'logs-and-insights'
  params: {
    name: name
    location: location
  }
}

resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = {
  name: replace(name, 'resourcegroup', 'vnet')
  location: location
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
  name: '${name}-plan'
  location: location
  kind: 'app'
  sku: {
    name: 'S1'
    capacity: 1
  }
}

module silo 'app-service.bicep' = {
  name: 'silo'
  params: {
    name: replace(name, 'resourcegroup', 'silo')
    appServicePlanId: appServicePlan.id
    vnetSubnetId: vnet.properties.subnets[0].id
    envVars: union(shared_config, silo_config)
    location: location
  }
}
