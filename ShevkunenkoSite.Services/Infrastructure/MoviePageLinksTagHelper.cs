using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ShevkunenkoSite.Services.Infrastructure;

[HtmlTargetElement("div", Attributes = "movies-model")]
public class MoviePageLinksTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
    }
}
