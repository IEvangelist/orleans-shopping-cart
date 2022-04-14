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

    async Task IProductGrain.ReturnProductAsync(int quantity)
    {
        _product.State = _product.State with
        {
            Quantity = _product.State.Quantity + quantity
        };

        await _product.WriteStateAsync();
    }

    async Task<(bool IsAvailable, ProductDetails? ProductDetails)> IProductGrain.TryTakeProductAsync(int quantity)
    {
        if (_product.State.Quantity < quantity)
        {
            return (false, null);
        }

        _product.State = _product.State with
        {
            Quantity = _product.State.Quantity - quantity
        };

        await GrainFactory.GetGrain<IInventoryGrain>(_product.State.Category.ToString())
            .AddOrUpdateProductAsync(_product.State);

        return (true, _product.State);
    }

    async Task IProductGrain.CreateOrUpdateProductAsync(ProductDetails productDetails)
    {
        _product.State = productDetails;
        await _product.WriteStateAsync();
    }
}
