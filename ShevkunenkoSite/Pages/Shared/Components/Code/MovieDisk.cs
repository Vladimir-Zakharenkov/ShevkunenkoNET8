namespace ShevkunenkoSite.Pages.Shared.Components.Code;

public class MovieDisk : ViewComponent
{
    private readonly IMovieFileRepository _movieContext;
    public MovieDisk(IMovieFileRepository movieContext)
    {
        _movieContext = movieContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(string movieCaption)
    {
        if (await _movieContext.MovieFiles.Where(m => m.MovieCaption == movieCaption).AnyAsync())
        {
            MovieFileModel movieFileModel = await _movieContext.MovieFiles.FirstAsync(m => m.MovieCaption == movieCaption);

            return View(movieFileModel);
        }
        else
        {
            return View("MovieFalse");
        }
    }
}
