namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class PagesByModel : ViewComponent
{
    public IViewComponentResult Invoke(IEnumerable<PageInfoModel> pages)
    {
            return View(pages);
    }
}