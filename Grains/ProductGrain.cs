namespace Orleans.ShoppingCart.Grains;

internal class ProductGrain : Grain, IProductGrain
{
    readonly IPersistentState<ProductDetails> _product;

    public ProductGrain(
        [PersistentState(
            stateName: "Product",
            storageName: "shopping-cart")]
        IPersistentState<ProductDetails> product) => _product = product;

    Task<int> IProductGrain.GetProductAvailabilityAsync() => Task.FromResult(_product.State.Quantity);

    Task<ProductDetails> IProductGrain.GetProductDetailsAsync() => Task.FromResult(_product.State);

    async Task IProductGrain.ReturnProductAsync(int quantity)
    {
        await UpdateStateAsync(_product.State with
        {
            Quantity = _product.State.Quantity + quantity
        });
    }

    async Task<(bool IsAvailable, ProductDetails? ProductDetails)> IProductGrain.TryTakeProductAsync(int quantity)
    {
        if (_product.State.Quantity < quantity)
        {
            return (false, null);
        }

        var updatedState = _product.State with
        {
            Quantity = _product.State.Quantity - quantity
        };

        await UpdateStateAsync(updatedState);

        return (true, _product.State);
    }

    async Task IProductGrain.CreateOrUpdateProductAsync(ProductDetails productDetails)
    {
        await UpdateStateAsync(productDetails);
    }

    private async Task UpdateStateAsync(ProductDetails value)
    {
        var oldCategory = _product.State.Category;

        // Write the new state.
        _product.State = value;
        await _product.WriteStateAsync();

        // Update the inventory grain.
        var inventoryGrain = GrainFactory.GetGrain<IInventoryGrain>(_product.State.Category.ToString());
        await inventoryGrain.AddOrUpdateProductAsync(value);

        if (oldCategory != value.Category)
        {
            // Category changed! Remove the product from the old inventory grain.
            var oldInventoryGrain = GrainFactory.GetGrain<IInventoryGrain>(oldCategory.ToString());
            await oldInventoryGrain.RemoveProductAsync(value.Id);
        }
    }
}
