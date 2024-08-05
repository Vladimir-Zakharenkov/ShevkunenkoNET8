namespace ShevkunenkoSite.Areas.Movies.Pages;

public class Kak_uhodili_kumiryModel : PageModel
{
    private readonly IPageInfoRepository _pageInfoContext;

    public Kak_uhodili_kumiryModel(IPageInfoRepository pageInfoContext)
    {
        _pageInfoContext = pageInfoContext;
    }

    [BindProperty]
    public IEnumerable<PageInfoModel> PageLinks { get; set; } = Enumerable.Empty<PageInfoModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        PageLinks =  await _pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains("��� ������� ������,")).ToArrayAsync();

        return Page();
    }
}