// Ignore Spelling: Reklama

namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class ReklamaTop : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        if (HttpContext.Request.Host.ToString().Contains("shevkunenko.site") || HttpContext.Request.Host.ToString().Contains("sergeyshef.ru"))
        {
            return View("ShevkunenkoSiteTop");
        }
        else
        {
            return View("SergeyshefSiteTop");
        }
    }
}