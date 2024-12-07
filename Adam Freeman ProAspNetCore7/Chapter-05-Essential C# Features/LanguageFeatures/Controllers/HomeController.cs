#region Listing 5.4 The contents of the HomeController.cs file in the Controllers folder

//using Microsoft.AspNetCore.Mvc;

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            return View(new string[] { "C#", "Language", "Features" });
//        }
//    }
//}

#endregion

#region Listing 5.8 Adding a statement in the HomeController.cs file in the Controllers folder

//using Microsoft.AspNetCore.Mvc;
//using LanguageFeatures.Models;

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Product[] products = Product.GetProducts();
//            return View(new string[] { products[0].Name });
//        }
//    }
//}

#endregion

#region Listing 5.9 The contents of the GlobalUsings.cs file in the LanguageFeatures folder

////using Microsoft.AspNetCore.Mvc;
////using LanguageFeatures.Models;

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Product[] products = Product.GetProducts();

//            return View(new string[] { products[0].Name });
//        }
//    }
//}

#endregion

#region Listing 5.15 Changing Type in the HomeController.cs File in the Controllers Folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Product?[] products = Product.GetProducts();

//            return View(new string[] { products[0].Name });
//        }
//    }
//}

#endregion

#region Listing 5.16 Guarding against null in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Product?[] products = Product.GetProducts();
//            Product? p = products[0];
//            string val;

//            if (p != null)
//            {
//                val = p.Name;
//            }
//            else
//            {
//                val = "No value";
//            }

//            return View(new string[] { val });
//        }
//    }
//}

#endregion

#region Listing 5.17 Null conditional operator in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Product?[] products = Product.GetProducts();
//            string? val = products[0]?.Name;

//            if (val != null)
//            {
//                return View(new string[] { val });
//            }
//            return View(new string[] { "No Value" });
//        }
//    }
//}

#endregion

#region Listing 5-18. Using the Null-Coalescing Operator in the HomeController.cs File in the Controllers Folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Product?[] products = Product.GetProducts();

//            return View(new string[] { products[0]?.Name ?? "No Value" });
//        }
//    }
//}

#endregion

#region Listing 5.19 Using the null-forgiving operator in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Product?[] products = Product.GetProducts();

//            return View(new string[] { products[0]!.Name });
//        }
//    }
//}

#endregion

#region Listing 5.20 Disabling warnings in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Product?[] products = Product.GetProducts();

//#pragma warning disable CS8602
//            return View(new string[] { products[0].Name });
//        }
//    }
//}

#endregion

#region Listing 5.21 Using string interpolation in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Product?[] products = Product.GetProducts();

//            return View(new string[] { $"Name: {products[0]?.Name}, Price: {products[0]?.Price}" });
//        }
//    }
//}

#endregion

#region Listing 5.22 Initializing an object in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            string[] names = new string[3];
//            names[0] = "Bob";
//            names[1] = "Joe";
//            names[2] = "Alice";

//            return View("Index", names);
//        }
//    }
//}

#endregion

#region Listing 5.23 A collection initializer in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            return View("Index", new string[] { "Bob", "Joe", "Alice" });
//        }
//    }
//}

#endregion

#region Listing 5-24. Initializing a Dictionary in the HomeController.cs File in the Controllers Folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Dictionary<string, Product> products = new Dictionary<string, Product>
//            {
//                {
//                "Kayak",
//                new Product { Name = "Kayak", Price = 275M }
//                },
//                {
//                "Lifejacket",
//                new Product{ Name = "Lifejacket", Price = 48.95M }
//                }
//            };

//            return View("Index", products.Keys);
//        }
//    }
//}

#endregion

#region Listing 5.25 Using collection initializer syntax in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Dictionary<string, Product> products = new Dictionary<string, Product>
//            {
//                ["Kayak"] = new Product { Name = "Kayak", Price = 275M },
//                ["Lifejacket"] = new Product { Name = "Lifejacket", Price = 48.95M }
//            };

//            return View("Index", products.Keys);
//        }
//    }
//}

#endregion

#region Listing 5.26 Using a target-typed new expression in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            Dictionary<string, Product> products = new()
//            {
//                ["Kayak"] = new Product { Name = "Kayak", Price = 275M },
//                ["Lifejacket"] = new Product
//                {
//                    Name = "Lifejacket",
//                    Price = 48.95M
//                }
//            };

//            return View("Index", products.Keys);
//        }
//    }
//}

#endregion

#region Listing 5.27 Testing a type in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            object[] data = new object[] { 275M, 29.95M, "apple", "orange", 100, 10 };
//            decimal total = 0;

//            for (int i = 0; i < data.Length; i++)
//            {
//                if (data[i] is decimal d)
//                {
//                    total += d;
//                }
//            }

//            return View("Index", new string[] { $"Total: {total:C2}" });
//        }
//    }
//}

#endregion

#region Listing 5.28 Pattern matching in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            object[] data = new object[] { 275M, 29.95M, "apple", "orange", 100, 10 };
//            decimal total = 0;

//            for (int i = 0; i < data.Length; i++)
//            {
//                switch (data[i])
//                {
//                    case decimal decimalValue:
//                        total += decimalValue;
//                        break;
//                    case int intValue when intValue > 50:
//                        total += intValue;
//                        break;
//                }
//            }
//            return View("Index", new string[] { $"Total: {total:C2}" });
//        }
//    }
//}

#endregion

#region Listing 5.31 Applying an extension method in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };

//            decimal cartTotal = cart.TotalPrices();

//            return View("Index", new string[] { $"Total: {cartTotal:C2}" });
//        }
//    }
//}

#endregion

#region Listing 5.34 Applying an extension method in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };

//            Product[] productArray = {
//                new Product {Name = "Kayak", Price = 275M},
//                new Product {Name = "Lifejacket", Price = 48.95M}
//                };

//            decimal cartTotal = cart.TotalPrices();
//            decimal arrayTotal = productArray.TotalPrices();

//            return View("Index", new string[] { $"Cart Total: {cartTotal:C2}", $"Array Total: {arrayTotal:C2}" });
//        }
//    }
//}

#endregion

#region Listing 5.36 Using the filtering extension method in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };

//            Product[] productArray = {
//                new Product {Name = "Kayak", Price = 275M},
//                new Product {Name = "Lifejacket", Price = 48.95M},
//                new Product {Name = "Soccer ball", Price = 19.50M},
//                new Product {Name = "Corner flag", Price = 34.95M}
//             };

//            decimal arrayTotal = productArray.FilterByPrice(20).TotalPrices();

//            return View("Index", new string[] { $"Array Total: {arrayTotal:C2}" });
//        }
//    }
//}

#endregion

#region Listing 5.38 Using two filter methods in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };

//            Product[] productArray = {
//                new Product {Name = "Kayak", Price = 275M},
//                new Product {Name = "Lifejacket", Price = 48.95M},
//                new Product {Name = "Soccer ball", Price = 19.50M},
//                new Product {Name = "Corner flag", Price = 34.95M}
//                };

//            decimal priceFilterTotal = productArray.FilterByPrice(20).TotalPrices();
//            decimal nameFilterTotal = productArray.FilterByName('S').TotalPrices();

//            return View("Index", new string[] { $"Price Total: {priceFilterTotal:C2}", $"Name Total: {nameFilterTotal:C2}" });
//        }
//    }
//}

#endregion

#region Listing 5.40 Using a function to filter objects in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        bool FilterByPrice(Product? p)
//        {
//            return (p?.Price ?? 0) >= 20;
//        }

//        public ViewResult Index()
//        {
//            ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };

//            Product[] productArray = {
//                new Product {Name = "Kayak", Price = 275M},
//                new Product {Name = "Lifejacket", Price = 48.95M},
//                new Product {Name = "Soccer ball", Price = 19.50M},
//                new Product {Name = "Corner flag", Price = 34.95M}
//                };

//            Func<Product?, bool> nameFilter = delegate (Product? prod)
//            {
//                return prod?.Name?[0] == 'S';
//            };

//            decimal priceFilterTotal = productArray
//                .Filter(FilterByPrice)
//                .TotalPrices();

//            decimal nameFilterTotal = productArray
//                .Filter(nameFilter)
//                .TotalPrices();

//            return View("Index", new string[] {
//                $"Price Total: {priceFilterTotal:C2}",
//                $"Name Total: {nameFilterTotal:C2}" });
//        }
//    }
//}

#endregion

#region Listing 5.41 Using a lambda expression in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        //bool FilterByPrice(Product? p) {
//        // return (p?.Price ?? 0) >= 20;
//        //}

//        public ViewResult Index()
//        {
//            ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };

//            Product[] productArray = {
//                new Product {Name = "Kayak", Price = 275M},
//                new Product {Name = "Lifejacket", Price = 48.95M},
//                new Product {Name = "Soccer ball", Price = 19.50M},
//                new Product {Name = "Corner flag", Price = 34.95M}
//                };

//            //Func<Product?, bool> nameFilter = delegate (Product? prod) {
//            // return prod?.Name?[0] == 'S';
//            //};

//            decimal priceFilterTotal = productArray
//                .Filter(p => (p?.Price ?? 0) >= 20)
//                .TotalPrices();

//            decimal nameFilterTotal = productArray
//                .Filter(p => p?.Name?[0] == 'S')
//                .TotalPrices();

//            return View("Index", new string[] {
//                $"Price Total: {priceFilterTotal:C2}",
//                $"Name Total: {nameFilterTotal:C2}" });
//        }
//    }
//}

#endregion

#region Listing 5.42 Creating a common action pattern in the HomeController.cs file in the Controllers folder

//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            return View(Product.GetProducts().Select(p => p?.Name));
//        }
//    }
//}

#endregion

#region Listing 5.43 A lambda action method in the HomeController.cs file in the Controllers folder

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() => View(Product.GetProducts().Select(p => p?.Name));
    }
}

#endregion