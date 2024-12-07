#region Listing 5-29. The Contents of the MyExtensionMethods.cs File in the Models Folder

//namespace LanguageFeatures.Models
//{
//    public class ShoppingCart
//    {
//        public IEnumerable<Product?>? Products { get; set; }
//    }
//}

#endregion

#region Listing 5.32 Implementing an interface in the ShoppingCart.cs file in the Models folder

//using System.Collections;

//namespace LanguageFeatures.Models
//{
//    public class ShoppingCart : IEnumerable<Product?>
//    {
//        public IEnumerable<Product?>? Products { get; set; }

//        public IEnumerator<Product?> GetEnumerator() =>
//            Products?.GetEnumerator()
//                ?? Enumerable.Empty<Product?>().GetEnumerator();

//        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//    }
//}

#endregion

#region Listing 5.49 Implementing an interface in the ShoppingCart.cs file in the Models folder

namespace LanguageFeatures.Models
{
    public class ShoppingCart : IProductSelection
    {
        private List<Product> products = new();

        public ShoppingCart(params Product[] prods)
        {
            products.AddRange(prods);
        }

        public IEnumerable<Product>? Products { get => products; }
    }
}

#endregion