namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class MovieOnline(IPageInfoRepository pageInfoContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(HttpContext httpContext)
    {
        PageInfoModel pageInfoModel = await pageInfoContext.GetPageInfoByPathAsync(httpContext);

        if (pageInfoModel.OgType != "movie" || pageInfoModel.MovieFile == null)
        {
            return View("NoMovie");
        }

        string queryString = HttpContext.Request.QueryString.ToString();

        bool sergeyshefRu = false;

        Uri? videoRef;

        string? youtubeImageBorder;

        string? vkImageBorder;

        string? okImageBorder;

        string? mailruImageBorder;

        string? sergeyshefImageBorder;

        if (queryString.Contains("vk.com") || queryString.Contains("vkvideo.ru"))
        {
            videoRef = pageInfoModel.MovieFile.MovieVkVideo;

            youtubeImageBorder = null;
            vkImageBorder = "setimage_border";
            okImageBorder = null;
            mailruImageBorder = null;
            sergeyshefImageBorder = null;
        }
        else if (queryString.Contains("mail.ru"))
        {
            videoRef = pageInfoModel.MovieFile.MovieMailRuVideo;

            youtubeImageBorder = null;
            vkImageBorder = null;
            okImageBorder = null;
            mailruImageBorder = "setimage_border";
            sergeyshefImageBorder = null;
        }
        else if (queryString.Contains("ok.ru"))
        {
            videoRef = pageInfoModel.MovieFile.MovieOkVideo;

            youtubeImageBorder = null;
            vkImageBorder = null;
            okImageBorder = "setimage_border";
            mailruImageBorder = null;
            sergeyshefImageBorder = null;
        }
        else if (queryString.Contains("sergeyshef.ru"))
        {
            videoRef = pageInfoModel.MovieFile.MovieContentUrl;

            youtubeImageBorder = null;
            vkImageBorder = null;
            okImageBorder = null;
            mailruImageBorder = null;
            sergeyshefImageBorder = "setimage_border";

            sergeyshefRu = true;
        }
        else
        {
            videoRef = pageInfoModel.MovieFile.MovieYouTube;

            youtubeImageBorder = "setimage_border";
            vkImageBorder = null;
            okImageBorder = null;
            mailruImageBorder = null;
            sergeyshefImageBorder = null;
        }

        return View(new MovieOnlineViewModel
        {
            PageInfo = pageInfoModel,
            VideoRef = videoRef!,
            YoutubeImageBorder = youtubeImageBorder,
            OkImageBorder = okImageBorder,
            MailruImageBorder = mailruImageBorder,
            VkImageBorder = vkImageBorder,
            SergeyshefBorder = sergeyshefImageBorder,
            SergeyshefRu = sergeyshefRu,
            SitePath = $"{httpContext.Request.Protocol}//{httpContext.Request.Host}"
        });
    }
}