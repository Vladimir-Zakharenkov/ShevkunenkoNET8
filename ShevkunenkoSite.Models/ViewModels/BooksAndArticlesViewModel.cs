namespace ShevkunenkoSite.Models.ViewModels;

public class BooksAndArticlesViewModel
{
    public BooksAndArticlesModel[] AllBooks { get; set; } = [];

    public PagingInfoViewModel PagingInfo { get; set; } = new();

    public string? BookSearchString { get; set; }
}