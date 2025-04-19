namespace ShevkunenkoSite.Controllers;

public class ArticlesController(
    IBooksAndArticlesRepository articleContext,
    ITextInfoRepository textContext,
    IPageInfoRepository pageContext,
    IWebHostEnvironment hostEnvironment) : Controller
{
    private readonly string rootPath = hostEnvironment.WebRootPath;

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Article(Guid? articleId, int? pageNumber, bool? scan)
    {
        if (articleId != null
            && await articleContext.BooksAndArticles.Where(article => article.BooksAndArticlesModelId == articleId).AnyAsync()
            && pageNumber != null
            && pageNumber > 0
            && pageNumber <= articleContext.BooksAndArticles.First(article => article.BooksAndArticlesModelId == articleId).NumberOfPages)
        {
            // Если есть только скан
            if (!await textContext.Texts.Where(text => text.BooksAndArticlesModelId == articleId).AnyAsync() & scan == true)
            {
                ArticleViewModel scanForArticle = new()
                {
                    BookOrArticle = await articleContext.BooksAndArticles
                       .Include(logo => logo.LogoOfArticle)
                       .Include(scan => scan.ScanOfArticle)
                       .Include(movie => movie.VideoForBookOrArticle)
                       .FirstAsync(article => article.BooksAndArticlesModelId == articleId),

                    PageInfo = await pageContext.GetPageInfoByPathAsync(HttpContext),

                    HtmlText = null,

                    PageNumber = null,

                    Scan = true
                };

                return View("Article", scanForArticle);
            }

            if (!await textContext.Texts.Where(text => text.BooksAndArticlesModelId == articleId & text.SequenceNumber == pageNumber).AnyAsync())
            {
                return RedirectToAction(nameof(Index));
            }

            var textForBookOrArticle = await textContext.Texts.FirstAsync(text => text.BooksAndArticlesModelId == articleId & text.SequenceNumber == pageNumber);

            if (!System.IO.File.Exists(rootPath + DataConfig.TextsFolderPath + textForBookOrArticle.HtmlFileName))
            {
                return RedirectToAction(nameof(Index));
            }

            using StreamReader htmlText = new(rootPath + DataConfig.TextsFolderPath + textForBookOrArticle.HtmlFileName);

            ArticleViewModel bookOrArticle = new()
            {
                BookOrArticle = await articleContext.BooksAndArticles
                        .Include(logo => logo.LogoOfArticle)
                        .Include(scan => scan.ScanOfArticle)
                        .Include(movie => movie.VideoForBookOrArticle)
                        .FirstAsync(article => article.BooksAndArticlesModelId == articleId),

                PageInfo = await pageContext.GetPageInfoByPathAsync(HttpContext),

                HtmlText = htmlText.ReadToEnd(),

                PageNumber = pageNumber,

                Scan = scan
            };

            return View("Article", bookOrArticle);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }
}