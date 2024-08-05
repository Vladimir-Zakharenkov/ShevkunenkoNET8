namespace ShevkunenkoSite.Views.Shared.Components.Code;

[ViewComponent]
public class BackgroundFoto : ViewComponent
{
    private readonly IPageInfoRepository _pageInfoContext;
    private readonly IMovieFileRepository _movieFileContext;
    private Guid pageIdGuid;
    private Guid movieIdGuid;

    public BackgroundFoto(IPageInfoRepository pageInfoContext, IMovieFileRepository movieFileContext)
    {
        _pageInfoContext = pageInfoContext;
        _movieFileContext = movieFileContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(bool left)
    {
        PageInfoModel pageInfoModel = await _pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        if (HttpContext.Request.Path.ToString().Contains("details")
            || HttpContext.Request.Path.ToString().Contains("update")
            || HttpContext.Request.Path.ToString().Contains("editpage")
            || HttpContext.Request.Path.ToString().Contains("detailspage")
            || HttpContext.Request.Path.ToString().Contains("editmovie")
            || HttpContext.Request.Path.ToString().Contains("detailsmovie"))
        {
            if (Guid.TryParse(HttpContext.Request.Query["pageId"].ToString(), out pageIdGuid) & await _pageInfoContext.PagesInfo.Where(g => g.PageInfoModelId == pageIdGuid).AnyAsync())
            {
                pageInfoModel = await _pageInfoContext.PagesInfo.AsNoTracking().FirstAsync(p => p.PageInfoModelId == pageIdGuid);
            }

            if (Guid.TryParse(HttpContext.Request.Query["movieId"].ToString(), out movieIdGuid) & await _movieFileContext.MovieFiles.Where(m => m.MovieFileModelId == movieIdGuid).AnyAsync())
            {
                var movie = await _movieFileContext.MovieFiles.AsNoTracking().FirstAsync(m => m.MovieFileModelId == movieIdGuid);

                if (movie.PageInfoModelId != null)
                {
                    pageInfoModel = await _pageInfoContext.PagesInfo.AsNoTracking().FirstAsync(p => p.PageInfoModelId == movie.PageInfoModelId);
                }
            }
        }

        if (pageInfoModel.BackgroundFileModel != null)
        {
            if (!string.IsNullOrEmpty(pageInfoModel.BackgroundFileModel.WebLeftBackground) & !string.IsNullOrEmpty(pageInfoModel.BackgroundFileModel.WebRightBackground))
            {
                return left
                    ? View("BackgroundFoto", pageInfoModel.BackgroundFileModel.WebLeftBackground)
                    : View("BackgroundFoto", pageInfoModel.BackgroundFileModel.WebRightBackground);
            }
            else
            {
                return left
                    ? View("BackgroundFoto", pageInfoModel.BackgroundFileModel.LeftBackground)
                    : View("BackgroundFoto", pageInfoModel.BackgroundFileModel.RightBackground);
            }
        }
        else
        {
            return View("BackgroundFoto", string.Empty);
        }
    }
}