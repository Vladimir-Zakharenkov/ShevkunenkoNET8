namespace ShevkunenkoSite.Controllers;

public class ArticlesController(
    IBooksAndArticlesRepository articleContext,
    ITextInfoRepository textContext,
    IPageInfoRepository pageContext) : Controller
{
    public async Task<IActionResult> Index(Guid? articleId, int? pageNumber)
    {
        if (articleId != null
            && pageNumber != null
            && await articleContext.BooksAndArticles.Where(article => article.BooksAndArticlesModelId == articleId).AnyAsync()
            && await textContext.Texts.Where(text => text.BooksAndArticlesModelId == articleId).AnyAsync()
            && pageNumber > 0
            && pageNumber <= articleContext.BooksAndArticles.First(article => article.BooksAndArticlesModelId == articleId).NumberOfPages)
        {
            ArticleViewModel bookOrArticle = new()
            {
                BookOrArticle = await articleContext.BooksAndArticles
                    .Include(logo => logo.LogoOfArticle)
                    .Include(scan => scan.ScanOfArticle)
                    .FirstAsync(article => article.BooksAndArticlesModelId == articleId),

                PageInfo = await pageContext.GetPageInfoByPathAsync(HttpContext)
            };

            return View("Article", bookOrArticle);
        }
        else
        {
            return View();
        }
    }
}