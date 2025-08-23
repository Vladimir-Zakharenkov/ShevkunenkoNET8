namespace ShevkunenkoSite.Services.TagHelpers;

[HtmlTargetElement("div", Attributes = "album-paging")]
public class AlbumPagingTagHelper : TagHelper
{
    public int? AlbumPaging { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var address = AlbumPaging + "@" + AlbumPaging;

        output.Content.SetContent(address);
    }
}
