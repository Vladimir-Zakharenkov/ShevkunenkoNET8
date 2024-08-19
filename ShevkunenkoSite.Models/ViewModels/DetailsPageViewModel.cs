namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsPageViewModel
{
    public PageInfoModel PageItem { get; set; } = new();

    public IconFileModel IconItem { get; set; } = new();

    public PageInfoModel[] LinksOnPages { get; set; } = [];

    public PageInfoModel[] LinksFromPages { get; set; } = [];
}