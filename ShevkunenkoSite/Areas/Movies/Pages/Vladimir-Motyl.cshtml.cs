namespace ShevkunenkoSite.Areas.Movies.Pages;

public class Vladimir_MotylModel : PageModel
{
    [BindProperty]
    public VideoLinksViewModel MotylFilmy { get; set; } = new()
    {
        HeadTitleForVideoLinks = string.Empty,
        IsImage = false,
        IconType = "webicon300",
        SearchFilter = "Фильмы Владимира Мотыля,",
        MovieInMainList = true,
        IsPartsMoreOne = true
    };

    public void OnGet()
    {
    }
}