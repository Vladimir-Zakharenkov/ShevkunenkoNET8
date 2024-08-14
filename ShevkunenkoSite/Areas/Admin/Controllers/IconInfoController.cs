namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class IconInfoController(IIconFileRepository iconContext) : Controller
{
    public async Task<IActionResult> Index(Guid? iconId)
    {
        IconFileModel icon;

#pragma warning disable CA1862
        var typesOfIcons = iconContext.IconFiles
            .Where(icon => icon.IconFileNameExtension.ToLower().Contains("ico"));
#pragma warning restore CA1862

        if (!iconId.HasValue)
        {
            return View(typesOfIcons);
        }
        else
        {
            if (await iconContext.IconFiles.Where(icon => icon.IconFileModelId == iconId).AnyAsync())
            {
                icon = await iconContext.IconFiles.FirstAsync(icon => icon.IconFileModelId == iconId);

                var listOfIcons = await iconContext.IconFiles
                    .Where(ic => ic.IconPath == icon.IconPath)
                    .OrderBy(ic => ic.IconFileName)
                    .ToArrayAsync();

                return View("IconsList", listOfIcons);
            }
            else
            {
                return View(typesOfIcons);
            }
        }
    }
}