namespace ShevkunenkoSite.Models.ViewModels;

public class RefPagesViewModel
{
    public List<VideoLinksViewModel> ListsOfVideoFilterOut { get; set; } = [];

    public List<List<PageInfoModel>> ListsOfFilterOut { get; set; } = [];

    public List<PageInfoModel> LinksToPagesByGuid { get; set; } = [];
}