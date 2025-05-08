namespace ShevkunenkoSite.Controllers;

public class BooksController(
    IBooksAndArticlesRepository articleContext,
    ITextInfoRepository textContext,
    IPageInfoRepository pageContext,
    IWebHostEnvironment hostEnvironment) : Controller
{
    private readonly string rootPath = hostEnvironment.WebRootPath;

    public IActionResult Index() => View();

    public async Task<IActionResult> Book(Guid? bookId, int? pageNumber, bool? scan)
    {
        if (bookId != null
            && await articleContext.BooksAndArticles.Where(article => article.BooksAndArticlesModelId == bookId).AnyAsync()
            && pageNumber != null
            && pageNumber > -1
            && pageNumber <= articleContext.BooksAndArticles.First(article => article.BooksAndArticlesModelId == bookId).NumberOfPages)
        {
            // Если есть только скан
            if (!await textContext.Texts.Where(text => text.BooksAndArticlesModelId == bookId).AnyAsync() & scan == true)
            {
                ArticleViewModel scanForArticle = new()
                {
                    BookOrArticle = await articleContext.BooksAndArticles
                       .Include(logo => logo.LogoOfArticle)
                       .Include(scan => scan.ScanOfArticle)
                       .Include(movie => movie.VideoForBookOrArticle)
                       .FirstAsync(article => article.BooksAndArticlesModelId == bookId),

                    PageInfo = await pageContext.GetPageInfoByPathAsync(HttpContext),

                    HtmlText = null,

                    PageNumber = null,

                    Scan = true
                };

                return View("Book", scanForArticle);
            }

            // Если для статьи или книги отсутствует текст
            if (!await textContext.Texts.Where(text => text.BooksAndArticlesModelId == bookId & text.SequenceNumber == pageNumber).AnyAsync())
            {
                return RedirectToAction(nameof(Index));
            }

            var textForBookOrArticle = await textContext.Texts.FirstAsync(text => text.BooksAndArticlesModelId == bookId & text.SequenceNumber == pageNumber);

            if (!System.IO.File.Exists(rootPath + DataConfig.TextsFolderPath + textForBookOrArticle.FolderForText + textForBookOrArticle.HtmlFileName))
            {
                return RedirectToAction(nameof(Index));
            }

            using StreamReader htmlText = new(rootPath + DataConfig.TextsFolderPath + textForBookOrArticle.FolderForText + textForBookOrArticle.HtmlFileName);

            ArticleViewModel bookOrArticle = new()
            {
                BookOrArticle = await articleContext.BooksAndArticles
                        .Include(logo => logo.LogoOfArticle)
                        .Include(scan => scan.ScanOfArticle)
                        .Include(movie => movie.VideoForBookOrArticle)
                        .FirstAsync(article => article.BooksAndArticlesModelId == bookId),

                PageInfo = await pageContext.GetPageInfoByPathAsync(HttpContext),

                HtmlText = htmlText.ReadToEnd(),

                PageNumber = pageNumber,

                Scan = scan
            };

            return View("Book", bookOrArticle);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

}
