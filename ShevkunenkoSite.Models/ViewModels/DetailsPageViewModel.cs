namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsPageViewModel
{
    public PageInfoModel PageItem { get; set; } = new();

    public IconFileModel IconItem { get; set; } = new();

    public List<PageInfoModel> LinksToPagesByGuid { get; set; } =[];

    public List<PageInfoModel> LinksToPagesByFilterOut { get; set; } = [];

    public List<PageInfoModel> LinksFromPagesByGuid { get; set; } = [];

    public List<PageInfoModel> LinksFromPagesByPageFilter { get; set; } = [];
}