using Orleans.Concurrency;
using System.Text.Json.Serialization;

namespace Orleans.ShoppingCart.Abstractions;

[Serializable, Immutable]
public sealed record class CartItem(
    string UserId,
    string ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    string DetailsUrl)
{
    [JsonIgnore]
    public decimal TotalPrice =>
        Math.Round(Quantity * UnitPrice, 2);
}