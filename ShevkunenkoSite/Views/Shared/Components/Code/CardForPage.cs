namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class CardForPage : ViewComponent
{
    private readonly IPageInfoRepository _pageInfoContext;
    private Guid pageIdGuid;

    public CardForPage(IPageInfoRepository pageInfoContext) => _pageInfoContext = pageInfoContext;

    public async Task<IViewComponentResult> InvokeAsync(string? pageLoc)
    {
        PageInfoModel page;

        PageInfoModel errorPage = await _pageInfoContext.PagesInfo.AsNoTracking().FirstAsync(p => p.PageInfoModelId == Guid.Parse("A195DA69-34CC-4215-9DBA-38D949685EEE"));

        if (!string.IsNullOrEmpty(pageLoc))
        {
            if (Guid.TryParse(pageLoc, out pageIdGuid))
            {
                if (await _pageInfoContext.PagesInfo.Where(g => g.PageInfoModelId == pageIdGuid).AnyAsync())
                {
                    page = await _pageInfoContext.PagesInfo.AsNoTracking().FirstAsync(p => p.PageInfoModelId == pageIdGuid);

                    return View(page);
                }
                else
                {
                    return View(errorPage);
                }
            }
            else
            {
                if (await _pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == pageLoc).AnyAsync())
                {
                    page = await _pageInfoContext.PagesInfo.FirstAsync(p => p.PageFullPathWithData == pageLoc);

                    return View(page);
                }
                else if (await _pageInfoContext.PagesInfo.Where(p => p.PagePathNickNameWithData == pageLoc).AnyAsync())
                {
                    page = await _pageInfoContext.PagesInfo.FirstAsync(p => p.PagePathNickNameWithData == pageLoc);

                    return View(page);
                }
                else
                {
                    return View(errorPage);
                }
            }
        }
        else
        {
            return View(errorPage);
        }
    }
}