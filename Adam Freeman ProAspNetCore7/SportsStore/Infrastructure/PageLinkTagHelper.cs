#region Listing 7.28 The contents of the PageLinkTagHelper.cs file in the SportsStore/Infrastructure folder

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.Routing;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.AspNetCore.Razor.TagHelpers;
//using SportsStore.Models.ViewModels;

//namespace SportsStore.Infrastructure
//{
//    [HtmlTargetElement("div", Attributes = "page-model")]
//    public class PageLinkTagHelper : TagHelper
//    {
//        private IUrlHelperFactory urlHelperFactory;

//        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
//        {
//            urlHelperFactory = helperFactory;
//        }

//        [ViewContext]
//        [HtmlAttributeNotBound]
//        public ViewContext? ViewContext { get; set; }

//        public PagingInfo? PageModel { get; set; }

//        public string? PageAction { get; set; }

//        public override void Process(TagHelperContext context, TagHelperOutput output)
//        {
//            if (ViewContext != null && PageModel != null)
//            {
//                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

//                TagBuilder result = new TagBuilder("div");

//                for (int i = 1; i <= PageModel.TotalPages; i++)
//                {
//                    TagBuilder tag = new TagBuilder("a");
//                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { productPage = i });
//                    tag.InnerHtml.Append(i.ToString());
//                    result.InnerHtml.AppendHtml(tag);
//                }

//                output.Content.AppendHtml(result.InnerHtml);
//            }
//        }
//    }
//}

#endregion

#region Listing 7.39 Adding classes to elements in the PageLinkTagHelper.cs file in the SportsStore/Infrastructure folder

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.Routing;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.AspNetCore.Razor.TagHelpers;
//using SportsStore.Models.ViewModels;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

//namespace SportsStore.Infrastructure
//{
//    [HtmlTargetElement("div", Attributes = "page-model")]
//    public class PageLinkTagHelper : TagHelper
//    {
//        private IUrlHelperFactory urlHelperFactory;

//        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
//        {
//            urlHelperFactory = helperFactory;
//        }

//        [ViewContext]
//        [HtmlAttributeNotBound]
//        public ViewContext? ViewContext { get; set; }

//        public PagingInfo? PageModel { get; set; }

//        public string? PageAction { get; set; }

//        public bool PageClassesEnabled { get; set; } = false;

//        public string PageClass { get; set; } = String.Empty;

//        public string PageClassNormal { get; set; } = String.Empty;

//        public string PageClassSelected { get; set; } = String.Empty;

//        public override void Process(TagHelperContext context, TagHelperOutput output)
//        {
//            if (ViewContext != null && PageModel != null)
//            {
//                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

//                TagBuilder result = new TagBuilder("div");

//                for (int i = 1; i <= PageModel.TotalPages; i++)
//                {
//                    TagBuilder tag = new TagBuilder("a");

//                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { productPage = i });

//                    if (PageClassesEnabled)
//                    {
//                        tag.AddCssClass(PageClass);
//                        tag.AddCssClass(i == PageModel.CurrentPage
//                        ? PageClassSelected : PageClassNormal);
//                    }

//                    tag.InnerHtml.Append(i.ToString());
//                    result.InnerHtml.AppendHtml(tag);
//                }

//                output.Content.AppendHtml(result.InnerHtml);
//            }
//        }
//    }
//}

#endregion

#region Listing 8.4 Prefixed values in the PageLinkTagHelper.cs file in the SportsStore/Infrastructure folder

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        public PagingInfo? PageModel { get; set; }

        public string? PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public bool PageClassesEnabled { get; set; } = false;

        public string PageClass { get; set; } = String.Empty;

        public string PageClassNormal { get; set; } = String.Empty;

        public string PageClassSelected { get; set; } = String.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext != null && PageModel != null)
            {
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

                TagBuilder result = new TagBuilder("div");

                for (int i = 1; i <= PageModel.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");

                    PageUrlValues["productPage"] = i;

                    tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);

                    if (PageClassesEnabled)
                    {
                        tag.AddCssClass(PageClass);
                        tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                    }

                    tag.InnerHtml.Append(i.ToString());

                    result.InnerHtml.AppendHtml(tag);
                }

                output.Content.AppendHtml(result.InnerHtml);
            }
        }
    }
}

#endregion