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

    public IActionResult Books()
    {
        return View();
    }

    public async Task<IActionResult> Article(Guid? articleId, int? pageNumber, bool? scan)
    {
        if (articleId != null
            && await articleContext.BooksAndArticles.Where(article => article.BooksAndArticlesModelId == articleId).AnyAsync()
            && pageNumber != null
            && pageNumber > -1
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

            // Если для статьи или книги отсутствует текст
            if (!await textContext.Texts.Where(text => text.BooksAndArticlesModelId == articleId & text.SequenceNumber == pageNumber).AnyAsync())
            {
                if (articleContext.BooksAndArticles.First(article => article.BooksAndArticlesModelId == articleId).TypeOfText == "book")
                {
                    return RedirectToAction(nameof(Books));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var textForBookOrArticle = await textContext.Texts.FirstAsync(text => text.BooksAndArticlesModelId == articleId & text.SequenceNumber == pageNumber);

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