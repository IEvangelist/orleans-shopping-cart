using Orleans.ShoppingCart.Silo.Components;

namespace Orleans.ShoppingCart.Silo.Pages;

public sealed partial class Products
{
    HashSet<ProductDetails>? _products;
    ManageProductModal? _modal;

    [Parameter]
    public string? Id { get; set; }

    [Inject]
    public InventoryService InventoryService { get; set; } = null!;

    protected override async Task OnInitializedAsync() =>
        _products = await InventoryService.GetAllProductsAsync();

    async Task OnProductCreated(ProductDetails product)
    {
        await InventoryService.CreateProductAsync(product);
        _products = await InventoryService.GetAllProductsAsync();

        _modal?.Close();

        StateHasChanged();
    }

    Task OnEditProduct(ProductDetails product)
    {
        if (_modal is not null)
        {
            _modal.Product = product;
            _modal.Open();
        }

        return Task.CompletedTask;
    }
}
