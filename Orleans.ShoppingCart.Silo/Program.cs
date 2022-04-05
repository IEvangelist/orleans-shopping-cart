using Orleans.Hosting;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(
        siloBuilder => siloBuilder.UseLocalhostClustering()
            .AddMemoryGrainStorageAsDefault()
            .UseTransactions())
    .RunConsoleAsync();