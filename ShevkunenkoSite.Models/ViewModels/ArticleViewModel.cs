namespace ShevkunenkoSite.Models.ViewModels;

public class ArticleViewModel
{
    #region Экземпляр статьи или книги

    public BooksAndArticlesModel BookOrArticle { get; set; } = new();

    #endregion

    #region Экземпляр страницы (статьи или книги)

    public PageInfoModel PageInfo { get; set; } = new();

    #endregion

    #region Текст с разметкой

    public string? HtmlText { get; set; }

    #endregion

    #region Номер страницы

    public int? PageNumber { get; set; }

    #endregion

    #region Скан статьи

    public bool? Scan { get; set; }

    #endregion

    #region Кадры слева и справа от текста

    public FramesAroundMainContentModel FramesAroundMainContent { get; set; } = new();

    #endregion
}