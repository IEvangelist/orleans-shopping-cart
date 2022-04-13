namespace Orleans.ShoppingCart.Silo.Services;

public sealed class InventoryService : BaseClusterService
{
    public InventoryService(
        IHttpContextAccessor httpContextAccessor, IClusterClient client) :
        base(httpContextAccessor, client)
    {
    }

    public Task<HashSet<ProductDetails>> GetAllProductsAsync() =>
        _client.GetGrain<IInventoryGrain>(0)
            .GetAllProductsAsync();        

    public Task CreateProductAsync(ProductDetails product) =>
        _client.GetGrain<IInventoryGrain>(0)
            .AddProductAsync(product);
}
