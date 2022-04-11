using Azure.Identity;
using Orleans.Hosting;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(
        (context, siloBuilder) => siloBuilder.UseLocalhostClustering()
            .AddAzureTableGrainStorage(
                "shopping-cart",
                options =>
                {
                    options.UseJson = true;
                    var serviceUri = new Uri(context.Configuration["ServiceUri"]);
                    options.ConfigureTableServiceClient(
                        serviceUri, new DefaultAzureCredential());
                }))
    .RunConsoleAsync();