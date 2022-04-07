namespace Orleans.ShoppingCart.Abstractions;

public interface IInventoryGrain : IGrainWithStringKey
{    
    Task<ISet<Product>> GetAllProductsAsync();
}
