namespace Orleans.ShoppingCart.Abstractions;

public interface IInventoryGrain : IGrainWithIntegerKey
{    
    Task<HashSet<ProductDetails>> GetAllProductsAsync();
    
    Task AddProductAsync(ProductDetails productDetails);
}
