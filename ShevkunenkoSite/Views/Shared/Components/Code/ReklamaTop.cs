// Ignore Spelling: Reklama

namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class ReklamaTop : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        if (HttpContext.Request.Host.ToString().Contains("shevkunenko.site"))
        {
            return View("ShevkunenkoSiteTop");
        }
        else
        {
            return View("SergeyshefSiteTop");
        }
    }
}