namespace Orleans.ShoppingCart.Silo.Pages;

public sealed partial class Cart
{
    private HashSet<CartItem>? _cartItems;

    [Inject]
    public ShoppingCartService ShoppingCart { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _cartItems = await ShoppingCart.GetAllItemsAsync();
    }

    private async Task OnItemRemoved(ProductDetails product)
    {
        await ShoppingCart.RemoveItemAsync(product);

        _ = _cartItems?.RemoveWhere(item => item.Product == product);
    }
}
