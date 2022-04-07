using Orleans.ShoppingCart.Abstractions;

namespace Orleans.ShoppingCart.Grains;

internal class ProductGrain : Grain, IProductGrain
{
    Task<int> IProductGrain.GetProductAvailabilityAsync()
    {
        throw new NotImplementedException();
    }

    Task IProductGrain.ReturnProductAsync(int quantity)
    {
        throw new NotImplementedException();
    }

    Task<ProductDetails?> IProductGrain.TakeProductAsync(int quantity)
    {
        throw new NotImplementedException();
    }
}
