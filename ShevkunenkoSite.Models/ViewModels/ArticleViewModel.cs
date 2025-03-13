namespace ShevkunenkoSite.Models.ViewModels;

public class ArticleViewModel
{
    // данные статьи или книги
    public BooksAndArticlesModel BookOrArticle { get; set; } = new();

    // данные страницы (статьи или книги)
    public PageInfoModel PageInfo { get; set; } = new();
}