namespace Orleans.ShoppingCart.Abstractions;

public interface IShoppingCartGrain : IGrainWithStringKey
{
    /// <summary>
    /// Adds the given <paramref name="item"/> to the shopping cart.
    /// </summary>
    Task AddItem(CartItem item);
    
    /// <summary>
    /// Removes the givne <paramref name="item" /> from the shopping cart.
    /// </summary>
    Task RemoveItem(CartItem item);
    
    /// <summary>
    /// Gets all the items in the shopping cart.
    /// </summary>
    Task<IList<CartItem>> GetAllItems();
    
    /// <summary>
    /// Removes all items from the shopping cart.
    /// </summary>
    Task Clear();
}