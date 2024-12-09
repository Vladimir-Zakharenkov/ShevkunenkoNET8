#region Listing 6.5 The contents of the HomeController.cs file in the Controllers folder

using Microsoft.AspNetCore.Mvc;
using SimpleApp.Models;

namespace SimpleApp.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View(Product.GetProducts());
        }
    }
}

#endregion

