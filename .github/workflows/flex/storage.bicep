param name string = resourceGroup().name
param location string = resourceGroup().location

resource storage 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: '${replace(name, '-resourcegroup', '')}storage'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

var key = listKeys(storage.name, storage.apiVersion).keys[0].value

output storageName string = storage.name
output accountKey string = key
