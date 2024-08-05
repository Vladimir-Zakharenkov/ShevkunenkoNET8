//The Markdown Tag Helper will use the <markdown> element.

namespace ShevkunenkoSite.Pages.Shared.Components.Code;

public class MarkdownTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        //Retrieve the contents of the <markdown> element.
        TagHelperContent markdownRazorContent = await output.GetChildContentAsync(NullHtmlEncoder.Default);

        //Render the Razor contents to a string.
        string markdown = markdownRazorContent.GetContent(NullHtmlEncoder.Default);

        //Convert the Markdown string to HTML using Markdig.
        string html = Markdig.Markdown.ToHtml(markdown);

        //Write the HTML content to the output
        output.Content.SetHtmlContent(html);

        //Remove the <markdown> element from the content.
        output.TagName = null;
    }
}