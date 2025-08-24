namespace ShevkunenkoSite.Services.Infrastructure;

[HtmlTargetElement("div", Attributes = "album-page")]
public class AlbumPageLinkTagHelper(IUrlHelperFactory helperFactory) : TagHelper
{
    private readonly IUrlHelperFactory urlHelperFactory = helperFactory;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }

    public PagingInfoViewModel? AlbumPage { get; set; }

    public string? PageAction { get; set; }

    public bool PageClassesEnabled { get; set; } = false;

    public string PageClass { get; set; } = string.Empty;

    public string PageClassNormal { get; set; } = string.Empty;

    public string PageClassSelected { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext != null && AlbumPage != null)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder result = new("div");

            for (int i = 1; i <= AlbumPage.TotalPages; i++)
            {
                TagBuilder tag = new("a");

                tag.Attributes["href"] = urlHelper.Action(PageAction,
                    new
                    {
                        pageNumber = i,
                        imageId = 1
                    });

                if (PageClassesEnabled)
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == AlbumPage.CurrentPage ? PageClassSelected : PageClassNormal);
                }

                tag.InnerHtml.Append(i.ToString());

                result.InnerHtml.AppendHtml(tag);
            }

            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}