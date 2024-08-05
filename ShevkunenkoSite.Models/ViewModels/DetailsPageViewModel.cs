namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsPageViewModel
{
    public PageInfoModel PageItem { get; set; } = new();

    public IconFileModel IconItem { get; set; } = new();

    public PageInfoModel[] LinksOnPages { get; set; } =Array.Empty<PageInfoModel>();

    public PageInfoModel[] LinksFromPages { get; set; } = Array.Empty<PageInfoModel>();
}