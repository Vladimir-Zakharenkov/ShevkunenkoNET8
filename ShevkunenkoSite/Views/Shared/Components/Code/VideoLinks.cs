namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class VideoLinks(
    IMovieFileRepository movieContext
    ) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(VideoLinksViewModel moviesList)
    {
        if (moviesList.MovieInMainList == true)
        {
            MoviesListViewModel movies = new()
            {
                Movies = await movieContext.MovieFiles
                  .Where(p => p.MovieInMainList == moviesList.MovieInMainList)
                  .Where(p => p.SearchFilter.Contains(moviesList.SearchFilter ?? string.Empty))
                  .OrderBy(p => p.MovieDatePublished)
                  .ToArrayAsync(),

                IsPartsMoreOne = moviesList.IsPartsMoreOne,

                PageHeadTitle = moviesList.HeadTitleForVideoLinks,

                IsImage = moviesList.IsImage,

                IconType = moviesList.IconType
            };

            if (movies.Movies.Where(m => m.MoviePosterId == null).Any())
            {
                movies.IsImage = true;
            }

            return View(movies);
        }
        else
        {
            MoviesListViewModel movies = new()
            {
                Movies = await movieContext.MovieFiles
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