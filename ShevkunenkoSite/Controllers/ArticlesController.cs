namespace ShevkunenkoSite.Controllers;

public class ArticlesController (
    IBooksAndArticlesRepository articleContext,
    ITextInfoRepository textContext) : Controller
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
                BookOrArticle = await articleContext.BooksAndArticles.FirstAsync(article => article.BooksAndArticlesModelId == articleId)
            };

            return View("Article", bookOrArticle);
        }
        else
        {
            return View();
        }
    }
}