using Orleans.Concurrency;
using Orleans.ShoppingCart.Abstractions;

namespace Orleans.ShoppingCart.Grains;

[Reentrant]
public sealed class InventoryGrain : Grain, IInventoryGrain
{
    // TODO: Figure out how to actually initialize a data store with seed data
    // Figure out how to expose that data through this implementation.
    Task<ISet<Product>> IInventoryGrain.GetAllProductsAsync()
    {
        throw new NotImplementedException();
    }
}
