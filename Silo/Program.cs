using Orleans.Hosting;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(
        (context, siloBuilder) => siloBuilder.UseLocalhostClustering()
            .AddAzureTableGrainStorage(
                "shopping-cart",
                options =>
                {
                    options.ConfigureTableServiceClient(
                        context.Configuration["ShoppingCartConnectionString"]);
                })
            .UseTransactions())
    .RunConsoleAsync();