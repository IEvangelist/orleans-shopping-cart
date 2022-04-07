using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.ShoppingCart.Abstractions;

namespace Orleans.ShoppingCart.Grains;

[Reentrant]
public sealed class ShoppingCartGrain : Grain, IShoppingCartGrain
{
    readonly IPersistentState<HashSet<CartItem>> _cart;
    readonly IInventoryGrain _inventory;

    public ShoppingCartGrain(
        [PersistentState(
            stateName: "ShoppingCart",
            storageName: "CartState")]
        IPersistentState<HashSet<CartItem>> cart,
        IInventoryGrain inventory) =>
        (_cart, _inventory) = (cart, inventory);

    async Task<bool> IShoppingCartGrain.AddItemAsync(ProductDetails product)
    {
        // TODO: I'm assuming that there is a more optimal and reliable way to do this.
        // Should this be a transaction, should we rely on the leasing options Reuben mentioned?
        // Are there atomic operations? What guarantees are made?

        var availableQty =
            await _inventory.GetProductAvailabilitySnapshotAsync(product.Id);

        if (availableQty >= product.Quantity)
        {
            var availableProduct =
                await _inventory.TakeProductAsync(product.Id, product.Quantity);

            if (availableProduct is not null)
            {
                var item = ToCartItem(availableProduct);
                _cart.State.Add(item);

                await _cart.WriteStateAsync();
                return true;
            }
        }

        return false;
    }

    Task IShoppingCartGrain.ClearAsync()
    {
        _cart.State.Clear();
        return _cart.ClearStateAsync();
    }

    Task<ISet<CartItem>> IShoppingCartGrain.GetAllItemsAsync() =>
        Task.FromResult<ISet<CartItem>>(_cart.State.ToHashSet());

    async Task IShoppingCartGrain.RemoveItemAsync(ProductDetails product)
    {
        await _inventory.ReturnProductAsync(product.Id, product.Quantity);

        _cart.State.Remove(ToCartItem(product));
        await _cart.WriteStateAsync();
    }

    CartItem ToCartItem(ProductDetails product) =>
        new(this.GetPrimaryKeyString(), product);
}
