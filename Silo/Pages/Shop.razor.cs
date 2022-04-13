namespace Orleans.ShoppingCart.Silo.Pages;

public sealed partial class Shop
{
    HashSet<ProductDetails>? _products;

    [Inject]
    public ProductService ProductService { get; set; } = null!;

    [Inject]
    public InventoryService InventoryService { get; set; } = null!;

    [Inject]
    public ComponentStateChangedObserver Observer { get; set; } = null!;

    [Inject]
    public ToastService ToastService { get; set; } = null!;

    protected override async Task OnInitializedAsync() =>
        _products = await InventoryService.GetAllProductsAsync();

    async Task OnAddedToCart((string ProductId, int Quantity) tuple)
    {
        var (id, qty) = tuple;
        var (isAvailable, product) =
            await ProductService.TryTakeProductAsync(id, qty);
        if (isAvailable && product is { })
        {
            _products = await InventoryService.GetAllProductsAsync();
            
            await ToastService.ShowToastAsync(
                "Added to cart",
                $"The '{product.Name}' for {product.UnitPrice:C2} was added to your cart...");
            await Observer.NotifyStateChangedAsync();

            StateHasChanged();
        }
    }
}
