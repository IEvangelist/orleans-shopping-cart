using Orleans.Concurrency;
using System.Text.Json.Serialization;

namespace Orleans.ShoppingCart.Abstractions;

[Serializable, Immutable]
public sealed record class ProductDetails(
    string Id,
    string Name,
    string Description,
    ProductCategory Category,
    int Quantity,
    decimal UnitPrice,
    string DetailsUrl,
    string ImageUrl) : Product(Id, Name, Quantity)
{
    [JsonIgnore]
    public decimal TotalPrice =>
        Math.Round(Quantity * UnitPrice, 2);
}
