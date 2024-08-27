namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsPageViewModel
{
    public PageInfoModel PageItem { get; set; } = new();

    public IconFileModel IconItem { get; set; } = new();

    public List<PageInfoModel> LinksRefPages { get; set; } =[];

    public List<PageInfoModel> LinksOnPages { get; set; } = [];

    public List<PageInfoModel> LinksFromPages { get; set; } = [];
}