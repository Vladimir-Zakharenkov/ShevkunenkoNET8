#region Код по умолчанию

//using System.Diagnostics;
//using Microsoft.AspNetCore.Mvc;
//using PartyInvites.Models;

//namespace PartyInvites.Controllers;

//public class HomeController : Controller
//{
//    private readonly ILogger<HomeController> _logger;

//    public HomeController(ILogger<HomeController> logger)
//    {
//        _logger = logger;
//    }

//    public IActionResult Index()
//    {
//        return View();
//    }

//    public IActionResult Privacy()
//    {
//        return View();
//    }

//    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//    public IActionResult Error()
//    {
//        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//    }
//}

#endregion

#region Listing 3-3

//using Microsoft.AspNetCore.Mvc;

//namespace PartyInvites.Controllers
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

#region Listing 3-7

//using Microsoft.AspNetCore.Mvc;

//namespace PartyInvites.Controllers
//{
//    public class HomeController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//        public ViewResult RsvpForm()
//        {
//            return View();
//        }
//    }
//}

#endregion

#region Listing 3-11

//using Microsoft.AspNetCore.Mvc;
//using PartyInvites.Models;

//namespace PartyInvites.Controllers
//{
//    public class HomeController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//        [HttpGet]
//        public ViewResult RsvpForm()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ViewResult RsvpForm(GuestResponse guestResponse)
//        {
//            // TODO: store response from guest
//            return View();
//        }
//    }
//}

#endregion

#region Listing 3-13

//using Microsoft.AspNetCore.Mvc;
//using PartyInvites.Models;

//namespace PartyInvites.Controllers
//{
//    public class HomeController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//        [HttpGet]
//        public ViewResult RsvpForm()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ViewResult RsvpForm(GuestResponse guestResponse)
//        {
//            Repository.AddResponse(guestResponse);
//            return View("Thanks", guestResponse);
//        }
//    }
//}

#endregion

#region Listing 3-15

//using Microsoft.AspNetCore.Mvc;
//using PartyInvites.Models;

//namespace PartyInvites.Controllers
//{
//    public class HomeController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet]
//        public ViewResult RsvpForm()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ViewResult RsvpForm(GuestResponse guestResponse)
//        {
//            Repository.AddResponse(guestResponse);
//            return View("Thanks", guestResponse);
//        }

//        public ViewResult ListResponses()
//        {
//            return View(Repository.Responses
//                .Where(r => r.WillAttend == true));
//        }
//    }
//}

#endregion

#region Listing 3.18 Checking for errors in the HomeController.cs file in the Controllers folder

using Microsoft.AspNetCore.Mvc;
using PartyInvites.Models;

namespace PartyInvites.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View();
        }
        [HttpPost]
        public ViewResult RsvpForm(GuestResponse guestResponse)
        {
            if (ModelState.IsValid)
            {
                Repository.AddResponse(guestResponse);

                return View("Thanks", guestResponse);
            }
            else
            {
                return View();
            }
        }
        public ViewResult ListResponses()
        {
            return View(Repository.Responses.Where(r => r.WillAttend == true));
        }
    }
}

#endregion