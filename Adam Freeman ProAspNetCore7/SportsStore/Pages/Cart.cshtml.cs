#region Default Cart.cshtml.cs

//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace SportsStore.Pages
//{
//    public class CartModel : PageModel
//    {
//        public void OnGet()
//        {
//        }
//    }
//}

#endregion

#region Listing 8.25 The Cart.cshtml.cs file in the SportsStore/Pages folder

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;
namespace SportsStore.Pages
{
    public class CartModel(IStoreRepository repo) : PageModel
    {
        private readonly IStoreRepository repository = repo;

        public Cart? Cart { get; set; }

        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";

            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product? product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

                Cart.AddItem(product, 1);

                HttpContext.Session.SetJson("cart", Cart);
            }

            return RedirectToPage(new { returnUrl });
        }
    }
}

#endregion

#region Listing 9.4 Using the service in the Cart.cshtml.cs file in the SportsStore/Pages folder

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using SportsStore.Infrastructure;
//using SportsStore.Models;
//namespace SportsStore.Pages
//{
//    public class CartModel : PageModel
//    {
//        private IStoreRepository repository;

//        public CartModel(IStoreRepository repo, Cart cartService)
//        {
//            repository = repo;
//            Cart = cartService;
//        }

//        public Cart Cart { get; set; }

//        public string ReturnUrl { get; set; } = "/";

//        public void OnGet(string returnUrl)
//        {
//            ReturnUrl = returnUrl ?? "/";
//            //Cart = HttpContext.Session.GetJson<Cart>("cart")
//            // ?? new Cart();
//        }

//        public IActionResult OnPost(long productId, string returnUrl)
//        {
//            Product? product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

//            if (product != null)
//            {
//                Cart.AddItem(product, 1);
//            }

//            return RedirectToPage(new { returnUrl = returnUrl });
//        }
//    }
//}

#endregion