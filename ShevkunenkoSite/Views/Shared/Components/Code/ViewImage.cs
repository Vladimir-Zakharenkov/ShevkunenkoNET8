namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class ViewImage(
    IImageFileRepository imageFileContext
    ) : ViewComponent
{
    readonly ViewImageViewModel viewImageViewModel = new();

    public async Task<IViewComponentResult> InvokeAsync(string? imageId, string? cssClass, string? iconType)
    {
        // Поиск картинки по GUID
        if (Guid.TryParse(imageId, out Guid imageIdGuid))
        {
            if (await imageFileContext.ImageFiles.Where(img => img.ImageFileModelId == imageIdGuid).AnyAsync())
            {
                viewImageViewModel.ImageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == imageIdGuid);
            }
        }
        // Поиск картинки по названию файла
        else if (await imageFileContext.ImageFiles.Where(img => img.WebImageFileName == imageId || img.ImageFileName == imageId).AnyAsync())
        {
            viewImageViewModel.ImageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.WebImageFileName == imageId || img.ImageFileName == imageId);
        }
        // Если ничего не найдено, выводим картинку NoImage
        else
        {
            viewImageViewModel.ImageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == Guid.Parse(DataConfig.NoImage));
        }

        // Параметры файла картинки
        if (iconType == "webhd" || iconType == "webimage" || iconType == "webicon300" || iconType == "webicon200" || iconType == "webicon100"
                || iconType == "hd" || iconType == "image" || iconType == "icon300" || iconType == "icon200" || iconType == "icon100")
        {
            viewImageViewModel.IconType = iconType;
        }
        else
        {
            viewImageViewModel.IconType = "webimage";
        }

        // CSS для картинки
        viewImageViewModel.CssClass = cssClass ?? string.Empty;
        viewImageViewModel.CssClass = ($"img-fluid img-thumbnail {viewImageViewModel.CssClass}");

        return View(viewImageViewModel);
    }
}