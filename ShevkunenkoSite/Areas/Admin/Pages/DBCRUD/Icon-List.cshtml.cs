namespace ShevkunenkoSite.Areas.Admin.Pages.DBCRUD

{
    public class Icon_ListModel : PageModel
    {
        private readonly IIconFileRepository _iconContext;
        public Icon_ListModel(IIconFileRepository iconContext) => _iconContext = iconContext;

        public IList<IconFileModel> AllIcons { get; set; } = new List<IconFileModel>();

        public async Task OnGetAsync()
        {
            AllIcons = await _iconContext.IconFiles.ToListAsync();
        }
    }
}
