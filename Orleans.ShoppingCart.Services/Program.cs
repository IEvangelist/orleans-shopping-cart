using Orleans;
using Orleans.Configuration;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "dev";
        options.ServiceId = "ShoppingCart.Services";
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .Build();

await client.Connect();

// TODO: use the client.
// Might create a UI, then the client would be used via DI.