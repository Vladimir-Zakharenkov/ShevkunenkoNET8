#region Listing 8.6 The contents of the NavigationMenuViewComponent.cs file in the SportsStore / Components folder

//using Microsoft.AspNetCore.Mvc;
//using SportsStore.Models;

//namespace SportsStore.Components
//{
//    public class NavigationMenuViewComponent : ViewComponent
//    {
//        public string Invoke()
//        {
//            return "Hello from the Nav View Component";
//        }
//    }
//}

#endregion

#region Listing 8.8 Adding categories in the NavigationMenuViewComponent.cs file in the SportsStore / Components folder

//using Microsoft.AspNetCore.Mvc;
//using SportsStore.Models;

//namespace SportsStore.Components
//{
//    public class NavigationMenuViewComponent : ViewComponent
//    {
//        private IStoreRepository repository;

//        public NavigationMenuViewComponent(IStoreRepository repo)
//        {
//            repository = repo;
//        }

//        public IViewComponentResult Invoke()
//        {
//            return View(repository.Products
//            .Select(x => x.Category)
//            .Distinct()
//            .OrderBy(x => x));
//        }
//    }
//}

#endregion

#region Listing 8.10 Passing the selected category in the NavigationMenuViewComponent.cs file in the SportsStore/Components folder

using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IStoreRepository repository;

        public NavigationMenuViewComponent(IStoreRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];

            return View(repository.Products
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x));
        }
    }
}

#endregion

