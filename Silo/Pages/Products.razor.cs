namespace Orleans.ShoppingCart.Silo.Pages;

public sealed partial class Products
{
    [Inject]
    public ProductService ProductService { get; set; } = null!;
}
