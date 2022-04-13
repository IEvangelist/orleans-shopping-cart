namespace Orleans.ShoppingCart.Silo.Pages;

public sealed partial class Cart
{
    private HashSet<CartItem>? _cartItems;

    [Inject]
    public ShoppingCartService ShoppingCart { get; set; } = null!;

    [Inject]
    public ComponentStateChangedObserver Observer { get; set; } = null!;

    protected override async Task OnInitializedAsync() =>
        _cartItems = await ShoppingCart.GetAllItemsAsync();

    private async Task OnItemRemoved(ProductDetails product)
    {
        await ShoppingCart.RemoveItemAsync(product);
        await Observer.NotifyStateChangedAsync();

        _ = _cartItems?.RemoveWhere(item => item.Product == product);
    }
}
