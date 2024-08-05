namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class RefPages : ViewComponent
{
    private readonly IPageInfoRepository _pageInfoContext;
    public RefPages(IPageInfoRepository pageInfoContext) => _pageInfoContext = pageInfoContext;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        PageInfoModel pageInfoModel = await _pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        string[] pageFiltersOut = Array.Empty<string>();

        List<PageInfoModel> onList = new();

        PageInfoModel[] refPages = Array.Empty<PageInfoModel>();

        if (!string.IsNullOrEmpty(pageInfoModel.PageFilterOut))
        {
            pageFiltersOut = pageInfoModel.PageFilterOut.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < pageFiltersOut.Length; i++)
            {
                refPages = await _pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFiltersOut[i].Trim() + ",")).OrderBy(p => p.PageCardText).ToArrayAsync();

                onList.AddRange(refPages);
            }

            refPages = onList.Distinct().ToArray();
        }
        else
        {
            refPages = await _pageInfoContext.PagesInfo.Where(p => p.RefPages.Contains(pageInfoModel.PageInfoModelId.ToString())).OrderBy(p => p.PageCardText).ToArrayAsync();
        }

        if (refPages.Length > 0 & pageInfoModel.PageLinks == true)
        {
            return View(refPages);
        }
        else
        {
            return View("Empty");
        }
    }
}