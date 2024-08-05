namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class HeadMain : ViewComponent
{
    private readonly IPageInfoRepository _pageInfoContext;
    private readonly IIconFileRepository _iconFileContext;
    public HeadMain(IPageInfoRepository pageInfoContext, IIconFileRepository iconFileContext)
    {
        _pageInfoContext = pageInfoContext;
        _iconFileContext = iconFileContext;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        PageInfoModel pageInfoModel = await _pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        List<IconFileModel> iconList = await _iconFileContext.IconFiles.Where(icon => icon.IconPath == pageInfoModel.PageIconPath).AsNoTracking().ToListAsync();

        if (iconList.Count == 0)
        {
            iconList = await _iconFileContext.IconFiles.Where(icon => icon.IconPath == "/main").AsNoTracking().ToListAsync();
        }

        IconFileModel iconItem = iconList.First(icon => icon.IconFileName == @DataConfig.IconItem);

        return View(new HeadViewModel
        {
            PageInfo = pageInfoModel,
            IconList = iconList,
            IconItem = iconItem
        });
    }
}