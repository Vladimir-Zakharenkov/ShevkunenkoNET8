namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class ViewImage(
    IImageFileRepository imageFileContext
    ) : ViewComponent
{
    ImageFileModel imageItem = new();

    public async Task<IViewComponentResult> InvokeAsync(string? imageId, string? cssClass, string? iconType)
    {
        if (Guid.TryParse(imageId, out Guid imageIdGuid))
        {
            if (await imageFileContext.ImageFiles.Where(img => img.ImageFileModelId == imageIdGuid).AnyAsync())
            {
                imageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == imageIdGuid);
            }
        }
        else if (await imageFileContext.ImageFiles.Where(img => img.WebImageFileName == imageId || img.ImageFileName == imageId).AnyAsync())
        {
            imageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.WebImageFileName == imageId || img.ImageFileName == imageId);
        }
        else
        {
            imageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == Guid.Parse("29F51581-8F24-4EDC-1F48-08DAE2B985EA"));
        }

        if (iconType == "webimage")
        {
            iconType = "webimage";
        }
        else if (iconType == "webhd")
        {
            iconType = "webhd";
        }
        else if (iconType == "webicon300")
        {
            iconType = "webicon300";
        }
        else if (iconType == "webicon200")
        {
            iconType = "webicon200";
        }
        else if (iconType == "webicon100")
        {
            iconType = "webicon100";
        }
        else if (iconType == "image")
        {
            iconType = "image";
        }
        else if (iconType == "hd")
        {
            iconType = "hd";
        }
        else if (iconType == "icon300")
        {
            iconType = "icon300";
        }
        else if (iconType == "icon200")
        {
            iconType = "icon200";
        }
        else if (iconType == "icon100")
        {
            iconType = "icon100";
        }
        else
        {
            iconType = string.Empty;
        }

        return View(new ViewImageViewModel
        {
            ImageItem = imageItem,
            CssClass = cssClass ?? string.Empty,
            IconType = iconType
        });
    }
}