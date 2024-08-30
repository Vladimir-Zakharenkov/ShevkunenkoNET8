namespace ShevkunenkoSite.Views.Shared.Components.Code;

[ViewComponent]
public class PageHeading(IPageInfoRepository pageInfoContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        PageInfoModel pageInfoModel = await pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        if (!string.IsNullOrEmpty(pageInfoModel.PageHeading))
        {
            string pageHeading = pageInfoModel.PageHeading;

            return View("Default", pageHeading);
        }
        else
        {
            return View("Empty");
        }
    }
}