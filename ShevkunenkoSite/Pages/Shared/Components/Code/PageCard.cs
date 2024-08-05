namespace ShevkunenkoSite.Pages.Shared.Components.Code;

public class PageCard : ViewComponent
{
    private readonly IPageInfoRepository _pageContext;
    public PageCard(IPageInfoRepository pageContext) => _pageContext = pageContext;

    public async Task<IViewComponentResult> InvokeAsync(string? pageLoc)
    {
        PageInfoModel page;

        if (pageLoc != null & await _pageContext.PagesInfo.Where(p => p.PageFullPathWithData == pageLoc).AnyAsync())
        {
            page = await _pageContext.PagesInfo.FirstAsync(p => p.PageFullPath == pageLoc);

            return View(page);
        }
        else if (pageLoc != null & await _pageContext.PagesInfo.Where(p => p.PagePathNickNameWithData == pageLoc).AnyAsync())
        {
            page = await _pageContext.PagesInfo.FirstAsync(p => p.PagePathNickNameWithData == pageLoc);

            return View(page);
        }
        else
        {
            page = await _pageContext.PagesInfo.FirstAsync(p => p.PageLoc == "/error404");

            return View(page);
        }
    }
}