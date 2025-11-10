namespace ShevkunenkoSite.Models.ViewModels;

public class ArticleViewModel
{
    #region Экземпляр статьи или книги

    public BooksAndArticlesModel BookOrArticle { get; set; } = new();

    #endregion

    #region Экземпляр страницы (статьи или книги)

    public PageInfoModel PageInfo { get; set; } = new();

    #endregion

    #region Экземпляр текста

    public TextInfoModel? TextForBookOrArticle { get; set; }

    #endregion

    #region Экземпляр Аудиокниги

    public AudioBookModel? AudioBook { get; set; }

    #endregion

    #region Исполнитель Аудиокниги

    public string? AudioActor { get; set; }

    #endregion

    #region Список аудиофайлов к Аудиокниге

    public List<AudioInfoModel>? AudioFilesForAudioBook { get; set; }

    #endregion

    #region Текст с разметкой

    public string? HtmlText { get; set; }

    #endregion

    #region Номер страницы

    public int? PageNumber { get; set; }

    #endregion

    #region Номер аудиофайла

    public int? AudioNumber { get; set; }

    #endregion

    #region Скан статьи

    public bool? Scan { get; set; }

    #endregion

    #region Кадры слева и справа от текста

    public FramesAroundMainContentModel? FramesAroundMainContent { get; set; }

    #endregion

    #region Фото из книги

    public List<ImageFileModel>? ListOfPictures { get; set; }

    #endregion
}