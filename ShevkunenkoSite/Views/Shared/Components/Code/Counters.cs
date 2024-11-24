namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class Counters : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        if (HttpContext.Request.Host.ToString().Contains("shevkunenko.site"))
        {
            return View("ShevkunenkoSite");
        }
        else if (HttpContext.Request.Host.ToString().Contains("sergeyshef.ru"))
        {
            return View("SergeyshefRu");
        }
        else
        {
            return View();
        }
    }
}