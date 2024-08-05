namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class LinksBelow3 : ViewComponent
{
    private readonly IMovieFileRepository _movieContext;
    public LinksBelow3(IMovieFileRepository movieContext)
    {
        _movieContext = movieContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(MovieFileModel movie)
    {
        if (!movie.AllMoviesFromDB3)
        {
            MoviesListViewModel movies = new()
            {
                Movies = await _movieContext.MovieFiles
                    .Where(p => p.MovieInMainList == true & p.SearchFilter.Contains(movie.SearchFilter3 ?? string.Empty))
                    .OrderBy(p => p.MovieUploadDate)
                    .ToArrayAsync(),

                IsImage = movie.IsImage3,

                IconType = movie.IconType3,

               IsPartsMoreOne = movie.IsPartsMoreOne3,

                PageHeadTitle = movie.HeadTitleForVideoLinks3
            };

            return View(movies);
        }
        else
        {
            MoviesListViewModel movies = new()
            {
                Movies = await _movieContext.MovieFiles
                      .Where(p => p.SearchFilter.Contains(movie.SearchFilter3 ?? string.Empty))
                      .OrderBy(p => p.MovieDatePublished)
                      .ToArrayAsync(),

                IsImage = movie.IsImage3,

                IconType = movie.IconType3,

               IsPartsMoreOne = movie.IsPartsMoreOne3,

                PageHeadTitle = movie.HeadTitleForVideoLinks3
            };

            return View(movies);
        }
    }
}