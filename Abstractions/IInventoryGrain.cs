namespace Orleans.ShoppingCart.Abstractions;

public interface IInventoryGrain : IGrainWithStringKey
{    
    Task<HashSet<Product>> GetAllProductsAsync();

    Task AddProductAsync(ProductDetails productDetails);
}
