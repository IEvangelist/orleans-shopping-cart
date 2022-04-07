namespace Orleans.ShoppingCart.Abstractions;

public interface IProductGrain : IGrainWithStringKey
{
    Task<ProductDetails?> TakeProductAsync(int quantity);

    Task ReturnProductAsync(int quantity);

    Task<int> GetProductAvailabilityAsync();    
}
