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
                builder.AddAzureTableGrainStorage(
                    "shopping-cart",
                    options =>
                    {
                        const string key = "ORLEANS_AZURE_STORAGE_CONNECTION_STRING";
                        var connectionString = context.Configuration[key];
                        options.ConfigureTableServiceClient(connectionString);
                        options.UseJson = true;
                    });
            }
        })
    .ConfigureWebHostDefaults(
        webBuilder => webBuilder.UseStartup<Startup>())
    .RunConsoleAsync();