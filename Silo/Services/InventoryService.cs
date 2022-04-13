namespace Orleans.ShoppingCart.Silo.Services;

public sealed class InventoryService : BaseClusterService
{
    public InventoryService(
        IHttpContextAccessor httpContextAccessor, IClusterClient client) :
        base(httpContextAccessor, client)
    {
    }

    public Task<HashSet<Product>> GetAllProductsAsync() =>
        TryUseGrain<IInventoryGrain, Task<HashSet<Product>>>(
            inventory => inventory.GetAllProductsAsync(),
            () => Task.FromResult(new HashSet<Product>()));

    public Task CreateProductAsync(ProductDetails product) =>
        TryUseGrain<IInventoryGrain, Task>(
            inventory => inventory.AddProductAsync(product),
            () => Task.CompletedTask);
}
