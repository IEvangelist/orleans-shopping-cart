using Orleans.Concurrency;
using Orleans.ShoppingCart.Abstractions;

namespace Orleans.ShoppingCart.Grains;

[Reentrant]
public sealed class InventoryGrain : Grain, IInventoryGrain
{
    // TODO: Figure out how to actually initialize a data store with seed data
    // Figure out how to expose that data through this implementation.

    Task<ProductDetails?> IInventoryGrain.TakeProductAsync(string productId, int quantity)
    {
        throw new NotImplementedException();
    }

    Task IInventoryGrain.ReturnProductAsync(string productId, int quantity)
    {
        throw new NotImplementedException();
    }

    Task<int> IInventoryGrain.GetProductAvailabilitySnapshotAsync(string productId)
    {
        throw new NotImplementedException();
    }

    Task<ISet<ProductDetails>> IInventoryGrain.GetAllProductsSnapshotAsync()
    {
        throw new NotImplementedException();
    }
}
