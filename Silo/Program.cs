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
                const string key = "ORLEANS_AZURE_STORAGE_CONNECTION_STRING";
                var connectionString = context.Configuration[key];

                builder.UseAzureStorageClustering(                    
                    options => options.ConfigureTableServiceClient(connectionString));
                builder.AddAzureTableGrainStorage(
                    "shopping-cart",
                    options =>
                    {
                        options.ConfigureTableServiceClient(connectionString);
                    });
            }
        })
    .ConfigureWebHostDefaults(
        webBuilder => webBuilder.UseStartup<Startup>())
    .RunConsoleAsync();