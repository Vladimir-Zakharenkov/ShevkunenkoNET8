namespace ShevkunenkoSite.Views.Shared.Components.Code;

[ViewComponent]
public class PageHeading(IPageInfoRepository pageInfoContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        PageInfoModel pageInfoModel = await pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        return View(pageInfoModel);
    }
}