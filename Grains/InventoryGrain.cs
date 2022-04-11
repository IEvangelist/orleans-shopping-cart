using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.ShoppingCart.Abstractions;

namespace Orleans.ShoppingCart.Grains;

[Reentrant]
public sealed class InventoryGrain : Grain, IInventoryGrain
{
    readonly IPersistentState<HashSet<ProductDetails>> _products;

    public InventoryGrain(
        [PersistentState(
            stateName: "Products",
            storageName: "shopping-cart")]
        IPersistentState<HashSet<ProductDetails>> products) => _products = products;

    Task<HashSet<Product>> IInventoryGrain.GetAllProductsAsync() =>
        Task.FromResult<HashSet<Product>>(_products.State.Cast<Product>().ToHashSet());

    async Task IInventoryGrain.AddProductAsync(ProductDetails productDetails)
    {
        if (_products.State.Add(productDetails))
        {
            await _products.WriteStateAsync();
        }
    }
}
