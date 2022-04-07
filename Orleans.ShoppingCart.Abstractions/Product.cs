using Orleans.Concurrency;

namespace Orleans.ShoppingCart.Abstractions;

[Serializable, Immutable]
public record class Product(
    string Id,
    string Name,
    int Quantity);
