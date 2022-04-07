namespace Orleans.ShoppingCart.Abstractions;

public interface IShoppingCartGrain : IGrainWithStringKey
{
    /// <summary>
    /// Adds the given <paramref name="product"/> to the shopping cart.
    /// </summary>
    Task<bool> AddItemAsync(ProductDetails product);

    /// <summary>
    /// Removes the givne <paramref name="product" /> from the shopping cart.
    /// </summary>
    Task RemoveItemAsync(ProductDetails product);
    
    /// <summary>
    /// Gets all the items in the shopping cart.
    /// </summary>
    Task<ISet<CartItem>> GetAllItemsAsync();
    
    /// <summary>
    /// Removes all items from the shopping cart.
    /// </summary>
    Task ClearAsync();
}