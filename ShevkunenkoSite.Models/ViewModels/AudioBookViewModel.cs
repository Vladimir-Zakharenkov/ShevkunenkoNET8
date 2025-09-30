namespace ShevkunenkoSite.Models.ViewModels;

public class AudioBookViewModel : AudioBookModel
{
    public List<SelectListItem> BooksOnSite { get; set; } = [];
}
