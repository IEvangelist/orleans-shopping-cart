using Orleans.Concurrency;

namespace Orleans.ShoppingCart.Abstractions;

[Serializable, Immutable]
public sealed record class Product(
    string ProductId,
    string Name,
    int Quantity);
