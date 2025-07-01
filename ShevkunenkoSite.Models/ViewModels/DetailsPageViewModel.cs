namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsPageViewModel
{
    #region Экземпляр страницы

    public PageInfoModel PageItem { get; set; } = new();

    #endregion

    #region Экземпляр иконки странницы

    public IconFileModel IconItem { get; set; } = new();

    #endregion

    #region Список ссылок на страницы по GUID (1)

    public List<PageInfoModel> LinksToPagesByGuid { get; set; } = [];

    #endregion

    #region Список ссылок на страницы по GUID (2)

    public List<PageInfoModel> LinksToPagesByGuid2 { get; set; } = [];

    #endregion

    #region Список ссылок на страницы по фильтру

    public List<PageInfoModel> LinksToPagesByFilterOut { get; set; } = [];

    #endregion

    #region Список ссылок на видео по фильтру

    public List<VideoLinksViewModel> LinksToVideosByFilterOut = [];

    #endregion

    #region Список списков видео

    public List<List<MovieFileModel>> ListsMoviesFileModel { get; set; } = [];

    #endregion

    #region Список ссылок на текущую страницу по GUID (1)

    public List<PageInfoModel> LinksFromPagesByGuid { get; set; } = [];

    #endregion

    #region Список ссылок на текущую страницу по GUID (2)

    public List<PageInfoModel> LinksFromPagesByGuid2 { get; set; } = [];

    #endregion

    #region Список ссылок на текущую страницу по фильтру

    public List<PageInfoModel> LinksFromPagesByPageFilter { get; set; } = [];

    #endregion

    #region Словарь страниц по текстовым фильтрам

    public Dictionary<string, List<PageInfoModel>> DictionaryOfOutPages { get; set; } = [];

    #endregion

    #region Кадры слева и справа от текста

    public FramesAroundMainContentModel FramesAroundMainContent { get; set; } = new();

    #endregion
}