namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class CardOfPage : ViewComponent
{
    public IViewComponentResult Invoke(PageInfoModel page)
    {
            return View(page);
    }
}