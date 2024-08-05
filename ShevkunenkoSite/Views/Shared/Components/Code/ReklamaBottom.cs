// Ignore Spelling: Reklama

namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class ReklamaBottom : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        if (HttpContext.Request.Host.ToString().Contains("shevkunenko.site"))
        {
            return View("ShevkunenkoSiteBottom");
        }
        else
        {
            return View("SergeyshefSiteBottom");
        }
    }
}
