namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class ImageView : ViewComponent
{
    private readonly IImageFileRepository _imageFileContext;
    public ImageView(IImageFileRepository imageFileContext) => _imageFileContext = imageFileContext;

    public async Task<IViewComponentResult> InvokeAsync(string fileName, string cssClass, bool? isIcon)
    {
        ImageFileModel? imageItem = await _imageFileContext.ImageFiles.AsNoTracking().FirstOrDefaultAsync(img => img.ImageFileName == fileName);

        imageItem ??= await _imageFileContext.ImageFiles.AsNoTracking().FirstOrDefaultAsync(img => img.ImageFileName == "no-image.png");

        return View(new ImageViewModel
        {
            ImageItem = imageItem ?? new(),
            CssClass = cssClass,
            IsIcon = isIcon
        });
    }
}