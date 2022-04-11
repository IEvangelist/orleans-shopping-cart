using Orleans.Runtime;
using Orleans.ShoppingCart.Abstractions;

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

    async Task<ProductDetails?> IProductGrain.TakeProductAsync(int quantity)
    {
        _product.State = _product.State with
        {
            Quantity = _product.State.Quantity - quantity
        };

        await _product.WriteStateAsync();

        return _product.State with { Quantity = quantity };
    }
}
