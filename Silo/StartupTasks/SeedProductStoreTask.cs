namespace Orleans.ShoppingCart.Silo.StartupTasks
{
    public sealed class SeedProductStoreTask : IStartupTask
    {
        readonly IGrainFactory _grainFactory;

        public SeedProductStoreTask(IGrainFactory grainFactory) =>
            _grainFactory = grainFactory;

        async Task IStartupTask.Execute(CancellationToken cancellationToken)
        {            
            var faker = new ProductDetails().GetBogusFaker();
            var inventoryGrains = Enum.GetValues<ProductCategory>()
                .Select(category => (
                    Category: category,
                    Grain: _grainFactory.GetGrain<IInventoryGrain>(category.ToString())))
                .ToDictionary(
                    keySelector: t => t.Category,
                    elementSelector: t => t.Grain);

            foreach (var product in faker.GenerateLazy(50))
            {
                var grain = inventoryGrains[product.Category];
                await grain.AddOrUpdateProductAsync(product);
            }
        }
    }
}
