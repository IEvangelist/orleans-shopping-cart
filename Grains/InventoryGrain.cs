namespace Orleans.ShoppingCart.Grains;

[Reentrant]
public sealed class InventoryGrain : Grain, IInventoryGrain
{
    readonly IPersistentState<Dictionary<string, ProductDetails>> _products;
    
    public InventoryGrain(
        [PersistentState(
            stateName: "Inventory",
            storageName: "shopping-cart")]
        IPersistentState<Dictionary<string, ProductDetails>> products) => _products = products;

    Task<HashSet<ProductDetails>> IInventoryGrain.GetAllProductsAsync() =>
        Task.FromResult(_products.State.Values.ToHashSet());
    
    async Task IInventoryGrain.AddOrUpdateProductAsync(ProductDetails product)
    {
        _products.State[product.Id] = product;
        
        await _products.WriteStateAsync();
        await GrainFactory.GetGrain<IProductGrain>(product.Id)
            .CreateOrUpdateProductAsync(product);
    }
}
