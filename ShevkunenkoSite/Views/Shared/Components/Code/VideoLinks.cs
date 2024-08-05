namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class VideoLinks : ViewComponent
{
    private readonly IMovieFileRepository _movieContext;
    public VideoLinks(IMovieFileRepository movieContext)
    {
        _movieContext = movieContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(VideoLinksViewModel moviesList)
    {
        if (moviesList.MovieInMainList == true)
        {
            MoviesListViewModel movies = new()
            {
                Movies = await _movieContext.MovieFiles
                  .Where(p => p.MovieInMainList == moviesList.MovieInMainList)
                  .Where(p => p.SearchFilter.Contains(moviesList.SearchFilter ?? string.Empty))
                  .OrderBy(p => p.MovieDatePublished)
                  .ToArrayAsync(),

               IsPartsMoreOne = moviesList.IsPartsMoreOne,

                PageHeadTitle = moviesList.HeadTitleForVideoLinks,

                IsImage = moviesList.IsImage,

                IconType = moviesList.IconType
            };

            return View(movies);
        }
        else
        {
            MoviesListViewModel movies = new()
            {
                Movies = await _movieContext.MovieFiles
                 .Where(p => p.SearchFilter.Contains(moviesList.SearchFilter ?? string.Empty))
                 .OrderBy(p => p.MovieDatePublished)
                 .ToArrayAsync(),

               IsPartsMoreOne = moviesList.IsPartsMoreOne,

                PageHeadTitle = moviesList.HeadTitleForVideoLinks,

                IsImage = moviesList.IsImage,

                IconType = moviesList.IconType
            };

            return View(movies);
        }
    }
}