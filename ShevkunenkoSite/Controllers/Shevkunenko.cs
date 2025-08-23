namespace ShevkunenkoSite.Controllers;

public class Shevkunenko(
    IImageFileRepository imageContext,
    IPageInfoRepository pageContext
        ) : Controller
{
    public IActionResult Index() => View();

    public async Task<IActionResult> Biography()
    {
        var pageInfoModel = await pageContext.GetPageInfoByPathAsync(HttpContext);

        return View(pageInfoModel);
    }

    public IActionResult Navigation() => View();

    public async Task<IActionResult> PhotoAlbum(Guid? imageId, int imagesPerPage = 12, int pageNumber = 1)
    {
        var allPhotoes = from m in imageContext.ImageFiles
            .Where(p => p.SearchFilter.Contains("Криминальная звезда"))
            .OrderBy(p => p.WebImageFileName)
                         select m;

        ImageListViewModel imageList = new()
        {
            AllImages = await allPhotoes
                .Skip((pageNumber - 1) * imagesPerPage)
                .Take(imagesPerPage)
                .ToArrayAsync(),

            PagingInfo = new PagingInfoViewModel
            {
                TotalItems = allPhotoes.Count(),
                ItemsPerPage = imagesPerPage,
                CurrentPage = pageNumber
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

    public IActionResult Error404() => View();
}