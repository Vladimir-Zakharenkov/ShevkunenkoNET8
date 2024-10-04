namespace ShevkunenkoSite.Models.ViewModels;

public class RefPagesViewModel
{
    // связанные страницы по GUID (1)
    public List<PageInfoModel> LinksToPagesByGuid { get; set; } = [];

    // связанные страницы по GUID (2)
    public List<PageInfoModel> LinksToPagesByGuid2 { get; set; } = [];

    // список списков связанных страниц по текстовому фильтру
    public List<List<PageInfoModel>> ListsOfFilterOut { get; set; } = [];

    // список VideoLinksViewModel связанных видео
    public List<VideoLinksViewModel> ListOfVideoLinksViewModel { get; set; } = [];
}