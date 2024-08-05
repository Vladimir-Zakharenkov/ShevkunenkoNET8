using Microsoft.AspNetCore.Authorization;
using ShevkunenkoSite.Areas.Education.Models;

namespace ShevkunenkoSite.Areas.Education.Controllers;

[Area("Education")]
[Authorize]
public class LanguageFeatures : Controller
{
    //public IActionResult Index()
    //{
    //return View(new string[] { "C#", "Language", "Features" });

    //-------------------------------------------------------------

    //Product?[] products = Product.GetProducts();

    //return View(new string[] { products[0].Name });

    //-------------------------------------------------------------

    //Product? p = products[0];

    //string val;

    //if (p != null)
    //{
    //    val = p.Name;
    //}
    //else
    //{
    //    val = "No value";
    //}

    //return View(new string[] { val });

    //-------------------------------------------------------------

    //string? val = products[0]?.Name;

    //if (val != null)
    //{
    //    return View(new string[] { val });
    //}
    //return View(new string[] { "No Value" });

    //-------------------------------------------------------------

    //return View(new string[] { products[0]?.Name ?? "No Value" });

    //-------------------------------------------------------------

    //return View(new string[] { products[0]!.Name });

    //-------------------------------------------------------------

    //return View(new string[] {
    //    $"Name: {products[0]?.Name}, Price: {products[0]?.Price:C2}"});
    //}

    //-------------------------------------------------------------

    //public ViewResult Index()
    //{
    //    string[] names = new string[3];
    //    names[0] = "Bob";
    //    names[1] = "Joe";
    //    names[2] = "Alice";
    //    return View("Index", names);
    //}

    //-------------------------------------------------------------

    //public ViewResult Index()
    //{
    //    return View("Index", new string[] { "Bob", "Joe", "Alice" });
    //}

    //-------------------------------------------------------------

    //public ViewResult Index()
    //{
    //    Dictionary<string, Product> products = new Dictionary<string, Product> {
    //        { "Kayak", new Product { Name = "Kayak", Price = 275M } },
    //        { "Lifejacket", new Product{ Name = "Lifejacket", Price = 48.95M } }
    //     };

    //    return View("Index", products.Keys);
    //}

    //-------------------------------------------------------------

    //public ViewResult Index()
    //{
    //    Dictionary<string, Product> products = new Dictionary<string, Product>
    //    {
    //        ["Kayak"] = new Product { Name = "Kayak", Price = 275M },
    //        ["Lifejacket"] = new Product { Name = "Lifejacket", Price = 48.95M }
    //    };
    //    return View("Index", products.Keys);
    //}

    //-------------------------------------------------------------

    //public ViewResult Index()
    //{
    //    object[] data = new object[] { 275M, 29.95M, "apple", "orange", 100, 10 };

    //    decimal total = 0;

    //for (int i = 0; i < data.Length; i++)
    //{
    //    if (data[i] is decimal d)
    //    {
    //        total += d;
    //    }
    //}

    //-------------------------------------------------------------

    //for (int i = 0; i < data.Length; i++)
    //{
    //    switch (data[i])
    //    {
    //        case decimal decimalValue:
    //            total += decimalValue;
    //            break;
    //        case int intValue when intValue > 50:
    //            total += intValue;
    //            break;
    //    }
    //}

    //return View("Index", new string[] { $"Total: {total:C2}" });
    //}

    //-------------------------------------------------------------
    bool FilterByPrice(Product? p)
    {
        return (p?.Price ?? 0) >= 20;
    }

    [Route("[area]")]
    [Route("[area]/[controller]")]
    [Route("[area]/[controller]/[action]")]
    public ViewResult Index()
    {
        ShoppingCart cart = new() { Products = Product.GetProducts() };

        //decimal cartTotal = cart.TotalPrices();

        //return View("Index", new string[] { $"Total: {cartTotal:C2}" });

        //-------------------------------------------------------------

        //Product[] productArray = {
        //    new Product {Name = "Kayak", Price = 275M},
        //    new Product {Name = "Lifejacket", Price = 48.95M}
        // };

        //decimal cartTotal = cart.TotalPrices();

        //decimal arrayTotal = productArray.TotalPrices();

        //return View("Index", new string[] {
        //    $"Cart Total: {cartTotal:C2}",
        //    $"Array Total: {arrayTotal:C2}" });

        //-------------------------------------------------------------

        Product[] productArray = {
            new Product {Name = "Kayak", Price = 275M},
            new Product {Name = "Lifejacket", Price = 48.95M},
            new Product {Name = "Soccer ball", Price = 19.50M},
            new Product {Name = "Corner flag", Price = 34.95M}
         };

        //decimal arrayTotal = productArray.FilterByPrice(20).TotalPrices();

        //return View("Index", new string[] { $"Array Total: {arrayTotal:C2}" });

        //decimal priceFilterTotal = productArray.FilterByPrice(20).TotalPrices();
        //decimal nameFilterTotal = productArray.FilterByName('S').TotalPrices();

        Func<Product?, bool> nameFilter = delegate (Product? prod)
        {
            return prod?.Name?[0] == 'S';
        };

        decimal priceFilterTotal = productArray
            .Filter(FilterByPrice)
            .TotalPrices();

        decimal nameFilterTotal = productArray
            .Filter(nameFilter)
            .TotalPrices();

        return View("Index", new string[] {
            $"Price Total: {priceFilterTotal:C2}",
            $"Name Total: {nameFilterTotal:C2}" });
    }
}
