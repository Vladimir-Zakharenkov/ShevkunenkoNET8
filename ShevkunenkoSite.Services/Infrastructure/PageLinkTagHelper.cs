using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ShevkunenkoSite.Models.ViewModels;

namespace ShevkunenkoSite.Services.Infrastructure;

[HtmlTargetElement("div", Attributes = "page-model")]
public class PageLinkTagHelper : TagHelper
{
    private readonly IUrlHelperFactory urlHelperFactory;

    public PageLinkTagHelper(IUrlHelperFactory helperFactory) => urlHelperFactory = helperFactory;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }

    public PagingInfoViewModel? PageModel { get; set; }

    public ImageListViewModel ImageSearch { get; set; } = new();

    public PagesListViewModel PageSearch {  get; set; } = new();

    public MoviesListViewModel MovieSearch { get; set; } = new();

    public TopicMovieViewModel TopicSearch { get; set; } = new();

    public string? PageAction { get; set; }

    public bool PageClassesEnabled { get; set; } = false;

    public string PageClass { get; set; } = String.Empty;

    public string PageClassNormal { get; set; } = String.Empty;

    public string PageClassSelected { get; set; } = String.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext != null && PageModel != null)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder result = new("div");

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder tag = new("a");

                tag.Attributes["href"] = urlHelper.Action(PageAction, 
                    new { pageNumber = i,  
                        imageSearchString = ImageSearch.ImageSearchString ?? string.Empty,
                        iconList = ImageSearch.IconList,
                        pageCard = PageSearch.PageCard,
                        pageTitleSearchString = PageSearch.PageTitleSearchString ?? string.Empty,
                        pageDescriptionSearchString = PageSearch.PageDescriptionSearchString ?? string.Empty,
                        keyWordSearchString = PageSearch.KeyWordSearchString ?? string.Empty,
                        movieCaptionSearchString = MovieSearch.MovieCaptionSearchString ?? string.Empty,
                        movieDescriptionForSchemaOrgSearchString = MovieSearch.MovieDescriptionForSchemaOrgSearchString ?? string.Empty,
                        movieGenreSearchString = MovieSearch.MovieGenreSearchString ?? string.Empty,
                        movieDirectorSearchString = MovieSearch.MovieDirectorSearchString ?? string.Empty,
                        movieMusicBySearchString = MovieSearch.MovieMusicBySearchString ?? string.Empty,
                        movieActorSearchString = MovieSearch.MovieActorSearchString ?? string.Empty,
                        topicMovieSearchString = TopicSearch.TopicMovieSearchString ?? string.Empty
                    });

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