namespace ShevkunenkoSite.Areas.Movies.Pages;

public class Pavel_LuspekaevModel : PageModel
{
    [BindProperty]
    public VideoLinksViewModel RoliKino { get; set; } = new()
    {
        HeadTitleForVideoLinks = string.Empty,
        IsImage = false,
        IconType = "webicon300",
        SearchFilter = "������ � �������� ����������,",
        MovieInMainList = true,
        IsPartsMoreOne = true
    };

    public void OnGet()
    {
    }
}