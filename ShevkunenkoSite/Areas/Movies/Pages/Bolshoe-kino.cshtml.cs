namespace ShevkunenkoSite.Areas.Movies.Pages;

public class Bolshoe_kinoModel : PageModel
{
    private readonly IPageInfoRepository _pageInfoContext;

    public Bolshoe_kinoModel(IPageInfoRepository pageInfoContext)
    {
        _pageInfoContext = pageInfoContext;
    }

    [BindProperty]
    public PageInfoModel[] PageLinks { get; set; } = Array.Empty<PageInfoModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        PageLinks =  await _pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains("������� ����,")).ToArrayAsync();

        return Page();
    }
}