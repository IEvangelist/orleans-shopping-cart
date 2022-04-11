﻿using Orleans.Concurrency;
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
            storageName: "shopping-cart")]
        IPersistentState<HashSet<CartItem>> cart) => _cart = cart;

    async Task<bool> IShoppingCartGrain.AddItemAsync(ProductDetails product)
    {
        // TODO: I'm assuming that there is a more optimal and reliable way to do this.
        // Should this be a transaction, should we rely on the leasing options Reuben mentioned?
        // Are there atomic operations? What guarantees are made?

        var products = GrainFactory.GetGrain<IProductGrain>(product.Id);

        var (isAvailable, claimedProduct) = await products.TryTakeProductAsync(product.Quantity);
        if (isAvailable && claimedProduct is not null)
        {
            var item = ToCartItem(claimedProduct);
            _cart.State.Add(item);

            await _cart.WriteStateAsync();
            return true;
        }

        return false;
    }

    Task IShoppingCartGrain.ClearAsync()
    {
        _cart.State.Clear();
        return _cart.ClearStateAsync();
    }

    Task<HashSet<CartItem>> IShoppingCartGrain.GetAllItemsAsync() =>
        Task.FromResult<HashSet<CartItem>>(_cart.State.ToHashSet());

    async Task IShoppingCartGrain.RemoveItemAsync(ProductDetails product)
    {
        var products = GrainFactory.GetGrain<IProductGrain>(product.Id);
        await products.ReturnProductAsync(product.Quantity);

        _cart.State.Remove(ToCartItem(product));
        await _cart.WriteStateAsync();
    }

    CartItem ToCartItem(ProductDetails product) =>
        new(this.GetPrimaryKeyString(), product);
}
