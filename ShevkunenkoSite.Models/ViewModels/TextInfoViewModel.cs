namespace ShevkunenkoSite.Models.ViewModels;

public class TextInfoViewModel
{
    public TextInfoModel[] AllTexts { get; set; } = [];

    public PagingInfoViewModel PagingInfo { get; set; } = new();

    public string? TextSearchString { get; set; }
}
