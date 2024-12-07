namespace ShevkunenkoSite.Models.ViewModels;

public class ImageListViewModel
{
    public IEnumerable<ImageFileModel> AllImages { get; set; } = [];

    public PagingInfoViewModel PagingInfo { get; set; } = new();

    public string? ImageSearchString { get; set; }

    public bool? IconList { get; set; } = false;

    public Guid? CurrentImageId { get; set; }
}