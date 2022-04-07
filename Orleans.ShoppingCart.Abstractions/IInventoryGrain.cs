namespace Orleans.ShoppingCart.Abstractions;

public interface IInventoryGrain : IGrainWithStringKey
{
    Task<ProductDetails?> TakeProductAsync(string productId, int quantity);

    Task ReturnProductAsync(string productId, int quantity);

    Task<int> GetProductAvailabilitySnapshotAsync(string productId);
    
    Task<ISet<ProductDetails>> GetAllProductsSnapshotAsync();
}
