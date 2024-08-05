namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class LinksBelow2 : ViewComponent
{
    private readonly IMovieFileRepository _movieContext;
    public LinksBelow2(IMovieFileRepository movieContext)
    {
        _movieContext = movieContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(MovieFileModel movie)
    {
        if (!movie.AllMoviesFromDB2)
        {
            MoviesListViewModel movies = new()
            {
                Movies = await _movieContext.MovieFiles
                    .Where(p => p.MovieInMainList == true & p.SearchFilter.Contains(movie.SearchFilter2 ?? string.Empty))
                    .OrderBy(p => p.MovieDatePublished)
                    .ToArrayAsync(),

                IsImage = movie.IsImage2,

                IconType = movie.IconType2,

               IsPartsMoreOne = movie.IsPartsMoreOne2,

                PageHeadTitle = movie.HeadTitleForVideoLinks2
            };

            return View(movies);
        }
        else
        {
            MoviesListViewModel movies = new()
            {
                Movies = await _movieContext.MovieFiles
                      .Where(p => p.SearchFilter.Contains(movie.SearchFilter2 ?? string.Empty))
                      .OrderBy(p => p.MovieDatePublished)
                      .ToArrayAsync(),

                IsImage = movie.IsImage2,

                IconType = movie.IconType2,

               IsPartsMoreOne = movie.IsPartsMoreOne2,

                PageHeadTitle = movie.HeadTitleForVideoLinks2
            };

            return View(movies);
        }
    }
}