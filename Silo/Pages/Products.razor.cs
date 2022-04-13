using Orleans.ShoppingCart.Silo.Components;

namespace Orleans.ShoppingCart.Silo.Pages;

public sealed partial class Products
{
    HashSet<ProductDetails>? _products;
    CreateProductModal _modal;

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

    async Task OnProductCreated(ProductDetails product)
    {
        await InventoryService.CreateProductAsync(product);
    }
}
