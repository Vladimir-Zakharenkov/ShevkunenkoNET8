namespace ShevkunenkoSite.Controllers;

public class Shevkunenko(IImageFileRepository imageContext) : Controller
{
    private readonly IImageFileRepository _imageContext = imageContext;

    public IActionResult Index() => View();

    public IActionResult Biography() => View();

    public IActionResult Navigation() => View();

    public async Task<IActionResult> PhotoAlbum(Guid? imageId, int imagesPerPage = 12, int pageNumber = 1)
    {
        var allPhotoes = from m in _imageContext.ImageFiles
            .OrderBy(p => p.WebImageFileName)
            .Where(p => p.SearchFilter
            .Contains("Криминальная звезда"))
                         select m;

        ImageListViewModel imageList = new()
        {
            AllImages = await allPhotoes
                .Skip((pageNumber - 1) * imagesPerPage)
                .Take(imagesPerPage)
                .ToArrayAsync(),

            PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = pageNumber,
                ItemsPerPage = imagesPerPage,
                TotalItems = allPhotoes.Count()
            },

            ImageSearchString = string.Empty,

            IconList = false,

            CurrentImageId = imageId
        };

        if (imageId == null)
        {
            return View(imageList);
        }
        else
        {
            return View("Foto", imageList);
        }
    }

    public IActionResult Press() => Redirect("https://shevkunenko.ru/pressa/index.htm");

    public IActionResult Test() => View();
}