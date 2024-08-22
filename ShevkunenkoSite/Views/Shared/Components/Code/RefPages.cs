namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class RefPages(IPageInfoRepository pageInfoContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        PageInfoModel pageInfoModel = await pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        string[] pageIdOut = [];

        List<PageInfoModel> onList = [];

        PageInfoModel[] refPages = [];

        if (!string.IsNullOrEmpty(pageInfoModel.PageFilterOut))
        {
#pragma warning disable CA1861 // Avoid constant arrays as arguments
            pageIdOut = pageInfoModel.PageFilterOut.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
#pragma warning restore CA1861 // Avoid constant arrays as arguments

            for (int i = 0; i < pageIdOut.Length; i++)
            {
                refPages = await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageIdOut[i].Trim() + ",")).OrderBy(p => p.PageCardText).ToArrayAsync();

                onList.AddRange(refPages);
            }

            refPages = onList.Distinct().ToArray();
        }
        else
        {
            refPages = await pageInfoContext.PagesInfo.Where(p => p.RefPages.Contains(pageInfoModel.PageInfoModelId.ToString())).OrderBy(p => p.PageCardText).ToArrayAsync();
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