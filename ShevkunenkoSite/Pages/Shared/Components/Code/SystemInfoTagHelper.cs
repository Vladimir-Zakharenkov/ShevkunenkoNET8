using System.Runtime.InteropServices;
using System.Text;

namespace ShevkunenkoSite.Pages.Shared.Components.Code;

//Derives from the TagHelper base class
public class SystemInfoTagHelper : TagHelper
{
    //An HtmlEncoder is necessary when writing HTML content to the page.
    private readonly HtmlEncoder _htmlEncoder;
    public SystemInfoTagHelper(HtmlEncoder htmlEncoder)
    {
        _htmlEncoder = htmlEncoder;
    }

    //Decorating properties with HtmlAttributeName allows you to set their values from Razor markup.
    [HtmlAttributeName("add-machine")]
    public bool IncludeMachine { get; set; } = true;

    //Decorating properties with HtmlAttributeName allows you to set their values from Razor markup.
    [HtmlAttributeName("add-os")]
    public bool IncludeOS { get; set; } = true;

    //The main function called when an element is rendered
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        //Replaces the <systeminfo> element with a <div> element
        output.TagName = "div";

        //Renders both the <div> </div> start and end tag
        output.TagMode = TagMode.StartTagAndEndTag;

        var sb = new StringBuilder();

        //If required, adds a <strong> element and the HTML-encoded machine name
        if (IncludeMachine)
        {
            sb.Append(" <strong>Machine</strong> ");
            sb.Append(_htmlEncoder.Encode(Environment.MachineName));
        }

        //If required, adds a <strong> element and the HTMLencoded OS name
        if (IncludeOS)
        {
            sb.Append("<br /> <strong>OS</strong> ");
            sb.Append(_htmlEncoder.Encode(RuntimeInformation.OSDescription));
        }

        //Sets the inner content of the <div> tag with the HTML-encoded value stored in the string builder
        output.Content.SetHtmlContent(sb.ToString());
    }
}