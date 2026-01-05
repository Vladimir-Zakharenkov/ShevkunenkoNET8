namespace ShevkunenkoSite.Services.TagHelpers;

[HtmlTargetElement("div", Attributes = "album-paging")]
public class AlbumPagingTagHelper(IUrlHelperFactory helperFactory) : TagHelper
{
    private readonly IUrlHelperFactory urlHelperFactory = helperFactory;

    [ViewContext]
    [HtmlAttributeNotBound]
    public required ViewContext ViewContext { get; set; }

    public required PagingInfoViewModel Pagination { get; set; }

    public required string AlbumCaption { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext != null && Pagination != null)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder result = new("div");
            result.AddCssClass("col-11 btn-group flex-wrap btn-group-sm m-2 p-1 mx-auto ten");

            for (int i = 1; i <= Pagination.TotalPages; i++)
            {
                TagBuilder tag = new("a");

                if (ViewContext.RouteData.Values.TryGetValue("action", out object? actionValue))
                {
                    if (actionValue != null)
                    {
                        tag.Attributes["href"] = urlHelper.Action(actionValue.ToString(),
                            new
                            {
                                pageNumber = i,
                                albumCaption = AlbumCaption,
                                imageId = string.Empty
                            });

                        tag.Attributes["title"] = "страница " + i;
                    }
                }

                tag.AddCssClass("btn me-1");
                tag.AddCssClass(i == Pagination.CurrentPage ? "btn-danger" : "btn-outline-dark");

                tag.InnerHtml.Append(i.ToString());

                result.InnerHtml.AppendHtml(tag);
            }

            output.Content.AppendHtml(result);
        }
    }
}