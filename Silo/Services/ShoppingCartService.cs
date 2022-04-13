namespace Orleans.ShoppingCart.Silo.Services;

public sealed class ShoppingCartService : BaseClusterService
{
    public ShoppingCartService(
        IHttpContextAccessor httpContextAccessor, IClusterClient client) :
        base(httpContextAccessor, client)
    {
    }

    public Task<HashSet<CartItem>> GetAllItemsAsync() =>
        TryUseGrain<IShoppingCartGrain, Task<HashSet<CartItem>>>(
            cart => cart.GetAllItemsAsync(),
            () => Task.FromResult(new HashSet<CartItem>()));

    public Task<int> GetCartCountAsync() =>
        TryUseGrain<IShoppingCartGrain, Task<int>>(
            cart => cart.GetTotalItemsInCartAsync(),
            () => Task.FromResult(0));

    public Task EmptyCartAsync() =>
        TryUseGrain<IShoppingCartGrain, Task>(
            cart => cart.ClearAsync(), 
            () => Task.CompletedTask);

    public Task<bool> AddItemAsync(ProductDetails product) =>
        TryUseGrain<IShoppingCartGrain, Task<bool>>(
            cart => cart.AddItemAsync(product),
            () => Task.FromResult(false));

    public Task RemoveItemAsync(ProductDetails product) =>
        TryUseGrain<IShoppingCartGrain, Task>(
            cart => cart.RemoveItemAsync(product),
            () => Task.CompletedTask);
}
