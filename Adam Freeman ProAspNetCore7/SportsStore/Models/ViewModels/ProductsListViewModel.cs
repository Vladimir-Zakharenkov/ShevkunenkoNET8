#region Listing 7.30 The contents of the ProductsListViewModel.cs file in the SportsStore/Models / ViewModels folder

namespace SportsStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

        public PagingInfo PagingInfo { get; set; } = new();
    }
}

#endregion