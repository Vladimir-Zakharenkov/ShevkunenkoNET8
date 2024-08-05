namespace ShevkunenkoSite.Models.ViewModels;

public class PagesListViewModel
{
    public IEnumerable<PageInfoModel> Pages { get; set; } = Enumerable.Empty<PageInfoModel>();

    public PagingInfoViewModel PagingInfo { get; set; } = new();

    public string PageTitleSearchString { get; set; } = string.Empty;

    public string PageDescriptionSearchString { get; set; } = string.Empty;

    public string KeyWordSearchString { get; set; } = string.Empty;

    public bool PageCard {  get; set; } = false;

    public int PageNumber { get; set; } = 0;
}