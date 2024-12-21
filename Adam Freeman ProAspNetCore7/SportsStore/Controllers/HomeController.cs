#region Listing 7.9 The contents of the HomeController.cs file in the SportsStore/Controllers folder

//using Microsoft.AspNetCore.Mvc;

//namespace SportsStore.Controllers
//{
//    public class HomeController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}

#endregion

#region Listing 7.24 Preparing the controller in the HomeController.cs file in the SportsStore/Controllers folder

//using Microsoft.AspNetCore.Mvc;
//using SportsStore.Models;

//namespace SportsStore.Controllers
//{
//    public class HomeController : Controller
//    {
//        private IStoreRepository repository;

//        public HomeController(IStoreRepository repo)
//        {
//            repository = repo;
//        }

//        public IActionResult Index() => View(repository.Products);
//    }
//}

#endregion

#region Listing 7.26 Adding pagination in the HomeController.cs file in the SportsStore/Controllers folder

//using Microsoft.AspNetCore.Mvc;
//using SportsStore.Models;

//namespace SportsStore.Controllers
//{
//    public class HomeController : Controller
//    {
//        private IStoreRepository repository;

//        public int PageSize = 4;

//        public HomeController(IStoreRepository repo)
//        {
//            repository = repo;
//        }

//        public ViewResult Index(int productPage = 1)
//            => View(repository.Products
//            .OrderBy(p => p.ProductID)
//            .Skip((productPage - 1) * PageSize)
//            .Take(PageSize));
//    }
//}

#endregion

#region Listing 7.31 Updating the action method in the HomeController.cs file in the Sports Store / Controllers folder

//using Microsoft.AspNetCore.Mvc;
//using SportsStore.Models;
//using SportsStore.Models.ViewModels;

//namespace SportsStore.Controllers
//{
//    public class HomeController : Controller
//    {
//        private IStoreRepository repository;

//        public int PageSize = 4;
//        public HomeController(IStoreRepository repo)
//        {
//            repository = repo;
//        }

//        public ViewResult Index(int productPage = 1) => View(new ProductsListViewModel
//        {
//            Products = repository.Products
//                .OrderBy(p => p.ProductID)
//                .Skip((productPage - 1) * PageSize)
//                .Take(PageSize),

//            PagingInfo = new PagingInfo
//            {
//                CurrentPage = productPage,
//                ItemsPerPage = PageSize,
//                TotalItems = repository.Products.Count()
//            }
//        });
//    }
//}

#endregion

#region Listing 8.2. Supporting categories in the HomeController.cs file in the SportsStore/Controllers folder

using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository repository;

        public int PageSize = 4;

        public HomeController(IStoreRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(string? category, int productPage = 1) =>
            View
            (
                new ProductsListViewModel
                {
                    Products = repository.Products.Where(p => category == null || p.Category == category)
                        .OrderBy(p => p.ProductID)
                        .Skip((productPage - 1) * PageSize)
                        .Take(PageSize),

                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = repository.Products.Count()
                    },

                    CurrentCategory = category
                }
             );
    }
}

#endregion

