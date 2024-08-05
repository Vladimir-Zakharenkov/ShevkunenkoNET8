namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class LinksForPages : ViewComponent
{
    public IViewComponentResult Invoke(string[] links, int? size)
    {
        if (links == null || links == Array.Empty<string>())
        {
            return View("NoLinks");
        }
        else if (size == 1)
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