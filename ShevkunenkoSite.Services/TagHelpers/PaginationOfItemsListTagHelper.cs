﻿using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShevkunenkoSite.Services.TagHelpers;

[HtmlTargetElement("div", Attributes = "pagination")]
public class PaginationOfItemsListTagHelper(IUrlHelperFactory helperFactory) : TagHelper
{
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }

    public PagingInfoViewModel? Pagination { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext != null && Pagination != null)
        {
            IUrlHelper urlHelper = helperFactory.GetUrlHelper(ViewContext);

            TagBuilder divForPagination = new("div");
            divForPagination.AddCssClass("flex-wrap btn-group btn-group-sm my-3 p-2 maxwidthcontent mx-auto");

            for (int i = 1; i <= Pagination.TotalPages; i++)
            {
                TagBuilder newRef = new("a");

                if (ViewContext.RouteData.Values.TryGetValue("action", out object? actionValue))
                {
                    if (actionValue != null)
                    {
                        newRef.Attributes["href"] = urlHelper.Action(actionValue.ToString(),
                            new
                            {
                                pageNumber = i,
                                searchString = ViewContext.HttpContext.Request.Query["searchString"].ToString() ?? string.Empty
                            });

                        newRef.Attributes["title"] = "страница " + i;
                    }
                }

                newRef.AddCssClass("btn border border-1 border-secondary rounded ten minwidth50px maxwidth100px mx-auto me-2");
                newRef.AddCssClass(i == Pagination.CurrentPage ? "btn-danger" : "btn-outline-dark");

                newRef.InnerHtml.Append(i.ToString());

                divForPagination.InnerHtml.AppendHtml(newRef);
            }

            output.Content.AppendHtml(divForPagination);
        }
    }
}