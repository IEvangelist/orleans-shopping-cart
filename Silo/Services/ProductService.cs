﻿namespace Orleans.ShoppingCart.Silo.Services;

public sealed class ProductService : BaseClusterService
{
    public ProductService(
        IHttpContextAccessor httpContextAccessor, IClusterClient client) :
        base(httpContextAccessor, client)
    {
    }

    public Task<(bool IsAvailable, ProductDetails? ProductDetails)> TryTakeProductAsync(
        int quantity) =>
        TryUseGrain<IProductGrain, Task<(bool IsAvailable, ProductDetails? ProductDetails)>>(
            products => products.TryTakeProductAsync(quantity),
            () => Task.FromResult<(bool IsAvailable, ProductDetails? ProductDetails)>(
                (false, null)));

    public Task ReturnProductAsync(string productId, int quantity) =>
        TryUseGrain<IProductGrain, Task>(
            products => products.ReturnProductAsync(quantity),
            productId,
            () => Task.CompletedTask);

    public Task<int> GetProductAvailability(string productId) =>
        TryUseGrain<IProductGrain, Task<int>>(
            products => products.GetProductAvailabilityAsync(),
            productId,
            () => Task.FromResult(0));
}
