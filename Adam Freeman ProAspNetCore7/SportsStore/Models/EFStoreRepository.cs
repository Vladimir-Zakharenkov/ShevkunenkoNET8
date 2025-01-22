#region Listing 7.19 The contents of the EFStoreRepository.cs file in the SportsStore/Models folder

namespace SportsStore.Models
{
    public class EFStoreRepository(StoreDbContext ctx) : IStoreRepository
    {
        private readonly StoreDbContext context = ctx;

        public IQueryable<Product> Products => context.Products;
    }
}

#endregion