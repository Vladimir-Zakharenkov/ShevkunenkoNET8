namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class HeadMain(IPageInfoRepository pageInfoContext, IIconFileRepository iconFileContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        PageInfoModel pageInfoModel = await pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        List<IconFileModel> iconList = await iconFileContext.IconFiles.Where(icon => icon.IconPath == pageInfoModel.PageIconPath).AsNoTracking().ToListAsync();

        if (iconList.Count == 0)
        {
            iconList = await iconFileContext.IconFiles.Where(icon => icon.IconPath == "main/").AsNoTracking().ToListAsync();
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