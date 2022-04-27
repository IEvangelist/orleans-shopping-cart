param operationalInsightsName string
param appServiceName string
param appInsightsName string
param resourceGroupLocation string

resource appServiceSettings 'Microsoft.Web/sites/config@2021-03-01' = {
  name: '${appServiceName}/appsettings'
  properties: {
    APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.properties.InstrumentationKey
    APPLICATIONINSIGHTS_CONNECTION_STRING: appInsights.properties.ConnectionString
  }
  dependsOn: [
    appServiceExtensions
  ]
}

resource appServiceExtensions 'Microsoft.Web/sites/siteextensions@2021-03-01' = {
  name: '${appServiceName}/Microsoft.ApplicationInsights.AzureWebsites'
  dependsOn: [
    appInsights
  ]
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: resourceGroupLocation
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logs.id
  }
}

resource logs 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: operationalInsightsName
  location: resourceGroupLocation
  properties: {
    retentionInDays: 30
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  }
}
