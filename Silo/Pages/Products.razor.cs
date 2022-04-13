using Orleans.ShoppingCart.Silo.Services;

namespace Orleans.ShoppingCart.Silo.Pages;

public sealed partial class Products
{
    HashSet<ProductDetails>? _products;

    [Parameter]
    public string? Id { get; set; }

    [Inject]
    public ProductService ProductService { get; set; } = null!;

    [Inject]
    public InventoryService InventoryService { get; set; } = null!;    

    protected override async Task OnInitializedAsync()
    {
        _products = await InventoryService.GetAllProductsAsync();
    }
}
