namespace Orleans.ShoppingCart.Silo.Pages;

public sealed partial class Shop
{
    [Inject]
    public InventoryService InventoryService { get; set; } = null!;
}
