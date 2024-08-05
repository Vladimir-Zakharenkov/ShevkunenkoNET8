namespace ShevkunenkoSite.Models.ViewModels;

public class ThreeCarouselsViewModel
{
    public PageInfoModel PageInfo { get; set; } = new();

    public MovieFileModel MovieCarousel { get; set; } = new();

    public ImageFileModel[]? FirstCarousel { get; set; }

    public ImageFileModel[]? SecondCarousel { get; set; }

    public ImageFileModel[]? ThirdCarousel { get; set; }
}