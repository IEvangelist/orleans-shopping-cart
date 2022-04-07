using Orleans.Concurrency;
using System.Text.Json.Serialization;

namespace Orleans.ShoppingCart.Abstractions;

[Serializable, Immutable]
public sealed record class CartItem(
    string UserId,
    ProductDetails Product)
{
    [JsonIgnore]
    public decimal TotalPrice => Product.TotalPrice;
}