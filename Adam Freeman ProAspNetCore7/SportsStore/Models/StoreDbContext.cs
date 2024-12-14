#region Listing 7.16 The contents of the StoreDbContext.cs file in the SportsStore/Models folder

using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
    }
}

#endregion

