#region Listing 6.5 The contents of the HomeController.cs file in the Controllers folder

//using Microsoft.AspNetCore.Mvc;
//using SimpleApp.Models;

//namespace SimpleApp.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            return View(Product.GetProducts());
//        }
//    }
//}

#endregion

#region Listing 6.17 Adding a property in the HomeController.cs file in the Controllers folder

using Microsoft.AspNetCore.Mvc;
using SimpleApp.Models;

namespace SimpleApp.Controllers
{
    public class HomeController : Controller
    {
        public IDataSource dataSource = new ProductDataSource();

        public ViewResult Index()
        {
            return View(dataSource.Products);
        }
    }
}

#endregion

