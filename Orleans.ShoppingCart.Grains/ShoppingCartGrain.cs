using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.ShoppingCart.Abstractions;

namespace Orleans.ShoppingCart.Grains;

[Reentrant]
public sealed class ShoppingCartGrain : Grain, IShoppingCartGrain
{
    readonly IPersistentState<HashSet<CartItem>> _cart;

    public ShoppingCartGrain(
        [PersistentState(
            stateName: "ShoppingCart",
            storageName: "CartState")]
        IPersistentState<HashSet<CartItem>> cart) =>
        _cart = cart;

    Task IShoppingCartGrain.AddItem(CartItem item)
    {
        _cart.State.Add(item);
        return _cart.WriteStateAsync();
    }

    Task IShoppingCartGrain.Clear()
    {
        _cart.State.Clear();
        return _cart.ClearStateAsync();
    }

    Task<IList<CartItem>> IShoppingCartGrain.GetAllItems() =>
        Task.FromResult<IList<CartItem>>(_cart.State.ToList());

    Task IShoppingCartGrain.RemoveItem(CartItem item)
    {
        _cart.State.Remove(item);
        return _cart.WriteStateAsync();
    }
}
