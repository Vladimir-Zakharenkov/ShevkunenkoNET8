namespace ShevkunenkoSite.Views.Shared.Components.Code;

[ViewComponent]
public class Menu : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        if (HttpContext.Request.Path.ToString().ToLower().Contains("admin"))
        {
            return View("Admin");
        }
        else
        {
            return View();
        }
    }
}