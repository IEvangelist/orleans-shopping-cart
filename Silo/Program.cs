using Orleans.Configuration;
using System.Net;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(
        (context, builder) =>
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                builder.UseLocalhostClustering()
                    .AddMemoryGrainStorage("shopping-cart")
                    .AddStartupTask<SeedProductStoreTask>();
            }
            else
            {
                // make sure we see the App Service configuration variables we expect to see 
                // that are there when the App Service is on a regional virtual network 
                // and has been assigned a vnetPrivatePortsCount property to >= 2.
                // presume the app is running in Web Apps on App Service and start up
                // launchSettings.json also has these so you can experiment on localhost.

                var endpointAddress = IPAddress.Parse(context.Configuration["WEBSITE_PRIVATE_IP"]);
                var strPorts = context.Configuration["WEBSITE_PRIVATE_PORTS"].Split(',');
                if (strPorts.Length < 2) throw new Exception("Insufficient private ports configured.");

                var connectionString = context.Configuration["ORLEANS_AZURE_STORAGE_CONNECTION_STRING"];

                int siloPort = int.Parse(strPorts[0]);
                int gatewayPort = int.Parse(strPorts[1]);

                builder
                    .ConfigureEndpoints(endpointAddress, siloPort, gatewayPort)
                    .Configure<ClusterOptions>(
                        options =>
                        {
                            options.ClusterId = "ShoppingCartCluster";
                            options.ServiceId = "ShoppingCartService";
                        })
                    .UseAzureStorageClustering(
                        options => options.ConfigureTableServiceClient(connectionString))
                    .AddAzureTableGrainStorage(
                        "shopping-cart",
                        options =>
                        {
                            options.ConfigureTableServiceClient(connectionString);
                        })
                    .AddStartupTask<SeedProductStoreTask>();

            }
        })
    .ConfigureWebHostDefaults(
        webBuilder => webBuilder.UseStartup<Startup>())
    .RunConsoleAsync();