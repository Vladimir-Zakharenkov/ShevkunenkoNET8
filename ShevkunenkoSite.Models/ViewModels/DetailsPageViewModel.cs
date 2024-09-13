namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsPageViewModel
{
    public PageInfoModel PageItem { get; set; } = new();

    public IconFileModel IconItem { get; set; } = new();

    // список ссылок на страницы по GUID (1)
    public List<PageInfoModel> LinksToPagesByGuid { get; set; } =[];

    // список ссылок на страницы по GUID (2)
    public List<PageInfoModel> LinksToPagesByGuid2 { get; set; } = [];

    // список ссылок на страницы по фильтру
    public List<PageInfoModel> LinksToPagesByFilterOut { get; set; } = [];

    // список ссылок на видео по фильтру
    public List<VideoLinksViewModel> LinksToVideosByFilterOut = [];

    // список списков видео
    public List<List<MovieFileModel>> ListsMoviesFileModel { get; set; } = [];

    // список ссылок на текущую страницу по GUID (1)
    public List<PageInfoModel> LinksFromPagesByGuid { get; set; } = [];

    // список ссылок на текущую страницу по GUID (2)
    public List<PageInfoModel> LinksFromPagesByGuid2 { get; set; } = [];

    // список ссылок на текущую страницу по фильтру
    public List<PageInfoModel> LinksFromPagesByPageFilter { get; set; } = [];
}