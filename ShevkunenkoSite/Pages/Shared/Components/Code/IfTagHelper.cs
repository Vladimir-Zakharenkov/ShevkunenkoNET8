//Setting the Attributes property ensures the Tag Helper is triggered by an if attribute.

namespace ShevkunenkoSite.Pages.Shared.Components.Code;

[HtmlTargetElement(Attributes = "if")]
public class IfTagHelper : TagHelper
{
    //Binds the value of the if attribute to the RenderContent property
    [HtmlAttributeName("if")]
    public bool RenderContent { get; set; } = true;

    //The Razor engine calls Process() to execute the Tag Helper.
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        //If the RenderContent property evaluates to false, removes the element
        if (RenderContent == false)
        {
            //Sets the element the Tag Helper resides on to null, removing it from the page
            output.TagName = null;

            //Doesn’t render or evaluate the inner content of the element
            output.SuppressOutput();
        }
    }

    //Ensures this Tag Helper runs before any others attached to the element
    public override int Order => int.MinValue;
}