namespace ShevkunenkoSite.Pages.Shared.Components.Code;

public class PageLinks : ViewComponent
{
    public IViewComponentResult Invoke(string[] links, int? size)
    {
        if (links == null || links == Array.Empty<string>())
        {
            return View("NoLinks");
        }
        else if (size == null || size > 3 || size == 1)
        {
            return View(links);
        }
        else if (size == 2)
        {
            return View("Links", links);
        }
        else
        {
            return View("BigLinks", links);
        }
    }
}