namespace Orleans.ShoppingCart.Silo.StartupTasks
{
    public sealed class SeedProductStoreTask : IStartupTask
    {
        readonly IGrainFactory _grainFactory;

        public SeedProductStoreTask(IGrainFactory grainFactory) =>
            _grainFactory = grainFactory;

        async Task IStartupTask.Execute(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var faker = new Faker<ProductDetails>()
                .StrictMode(true)
                .RuleFor(p => p.Id, (f, p) => f.Random.Number(1, 10_000).ToString())
                .RuleFor(p => p.Name, (f, p) => f.Commerce.ProductName())
                .RuleFor(p => p.Description, (f, p) => f.Lorem.Sentence())
                .RuleFor(p => p.UnitPrice, (f, p) => decimal.Parse(f.Commerce.Price()))
                .RuleFor(p => p.Quantity, (f, p) => f.Random.Number(0, 1_200))
                .RuleFor(p => p.ImageUrl, (f, p) => f.Image.LoremPixelUrl(LoremPixelCategory.Technics))
                .RuleFor(p => p.Category, (f, p) => f.PickRandom<ProductCategory>())
                .RuleFor(p => p.DetailsUrl, (f, p) => f.Internet.Url());

            var inventoryGrain = _grainFactory.GetGrain<IInventoryGrain>(0);

            foreach (var product in faker.GenerateLazy(30))
            {
                await inventoryGrain.AddProductAsync(product);
            }
        }
    }
}
