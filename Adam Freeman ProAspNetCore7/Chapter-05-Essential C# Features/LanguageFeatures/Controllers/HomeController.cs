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

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            Product?[] products = Product.GetProducts();

            return View(new string[] { products[0].Name });
        }
    }
}

#endregion