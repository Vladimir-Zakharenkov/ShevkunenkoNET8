namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class ViewImage(
    IImageFileRepository imageFileContext
    ) : ViewComponent
{
    ViewImageViewModel viewImageViewModel = new();

    public async Task<IViewComponentResult> InvokeAsync(string? imageId, string? cssClass, string? iconType)
    {
        if (Guid.TryParse(imageId, out Guid imageIdGuid))
        {
            if (await imageFileContext.ImageFiles.Where(img => img.ImageFileModelId == imageIdGuid).AnyAsync())
            {
                viewImageViewModel.ImageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == imageIdGuid);
            }
        }
        else if (await imageFileContext.ImageFiles.Where(img => img.WebImageFileName == imageId || img.ImageFileName == imageId).AnyAsync())
        {
            viewImageViewModel.ImageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.WebImageFileName == imageId || img.ImageFileName == imageId);
        }
        else
        {
            viewImageViewModel.ImageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == Guid.Parse(DataConfig.NoImage));
        }

        if (iconType == "webhd" || iconType == "webimage" || iconType == "webicon300" || iconType == "webicon200" || iconType == "webicon100"
                || iconType == "hd" || iconType == "image" || iconType == "icon300" || iconType == "icon200" || iconType == "icon100")
        {
            viewImageViewModel.IconType = iconType;
        }
        else
        {
            viewImageViewModel.IconType = "webimage";
        }

        viewImageViewModel.CssClass = cssClass ?? string.Empty;
        viewImageViewModel.CssClass = "img-fluid img-thumbnail " + viewImageViewModel.CssClass;

        return View(viewImageViewModel);
    }
}