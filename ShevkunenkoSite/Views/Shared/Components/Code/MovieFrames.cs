namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class MovieFrames(IImageFileRepository imageFileContext) : ViewComponent
{
    ImageFileModel[] imageItems = [];

    private readonly int frameAroundMovie = 10;

    public async Task<IViewComponentResult> InvokeAsync(string? imageFilter, bool leftSide, bool oneFrame)
    {
        if (!string.IsNullOrEmpty(imageFilter))
        {
            if (await imageFileContext.ImageFiles.Where(img => img.SearchFilter.Contains(imageFilter)).AnyAsync())
            {
                imageItems = await imageFileContext.ImageFiles.Where(img => img.SearchFilter.Contains(imageFilter)).OrderBy(img => img.WebImageFileName).ToArrayAsync();

                if (imageItems.Length > frameAroundMovie)
                {
                    imageItems = imageItems.Take(frameAroundMovie).ToArray();
                }
            }
            else
            {
                return View(imageItems);
            }
        }
        else
        {
            return View(imageItems);
        }

        if (oneFrame)
        {
            Random r = new();

            ImageFileModel imageItem = imageItems[r.Next(0, imageItems.Length)];

            return View("OneFrame", imageItem);
        }
        else
        {
            if (imageItems.Length == 1)
            {
                return View(imageItems);
            }
            else if (imageItems.Length > 1)
            {
                if (leftSide)
                {
                    int arrayLength = imageItems.Length / 2;

                    return View(imageItems.Take(arrayLength).ToArray());
                }
                else
                {
                    int arrayLength = imageItems.Length / 2;

                    return View(imageItems.Skip(arrayLength).ToArray());
                }
            }
            else
            {
                return View(imageItems);
            }
        }
    }
}