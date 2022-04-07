using Orleans.Concurrency;
using System.Text.Json.Serialization;

namespace Orleans.ShoppingCart.Abstractions;

[Serializable, Immutable]
public sealed record class ProductDetails(
    string Id,
    string Name,
    int Quantity,
    decimal UnitPrice,
    string DetailsUrl) : Product(Id, Name, Quantity)
{
    [JsonIgnore]
    public decimal TotalPrice =>
        Math.Round(Quantity * UnitPrice, 2);
}
