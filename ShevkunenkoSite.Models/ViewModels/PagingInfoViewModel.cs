namespace ShevkunenkoSite.Models.ViewModels;

public class PagingInfoViewModel
{
    public int TotalItems { get; set; } = 1;

    public int ItemsPerPage { get; set; } = 12;

    public int CurrentPage { get; set; } = 1;

    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
}