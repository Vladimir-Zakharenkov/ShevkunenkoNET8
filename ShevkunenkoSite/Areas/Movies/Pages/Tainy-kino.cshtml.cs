namespace ShevkunenkoSite.Areas.Movies.Pages;

public class Tainy_kinoModel : PageModel
{
    private readonly IPageInfoRepository _pageInfoContext;

    public Tainy_kinoModel(IPageInfoRepository pageInfoContext)
    {
        _pageInfoContext = pageInfoContext;
    }

    [BindProperty]
    public IEnumerable<PageInfoModel> PageLinks { get; set; } = Enumerable.Empty<PageInfoModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        PageLinks =  await _pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains("����� ����,")).ToArrayAsync();

        return Page();
    }
}