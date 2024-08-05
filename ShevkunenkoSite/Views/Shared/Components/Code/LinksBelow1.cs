namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class LinksBelow1 : ViewComponent
{
    private readonly IMovieFileRepository _movieContext;
    public LinksBelow1(IMovieFileRepository movieContext)
    {
        _movieContext = movieContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(MovieFileModel movie)
    {
        string[] movieFilters = Array.Empty<string>();

        IEnumerable<MovieFileModel> refMovies = Enumerable.Empty<MovieFileModel>();

        List<MovieFileModel> onList = new();

        if (!string.IsNullOrEmpty(movie.SearchFilter1))
        {
            movieFilters = movie.SearchFilter1.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!movie.AllMoviesFromDB1)
            {
                for (int i = 0; i < movieFilters.Length; i++)
                {
                    refMovies = await _movieContext.MovieFiles.AsNoTracking().Where(mov => mov.SearchFilter.Contains(movieFilters[i].Trim() + ",") & mov.MovieInMainList == true).ToArrayAsync();

                    onList.AddRange(refMovies);
                }
            }
            else
            {
                for (int i = 0; i < movieFilters.Length; i++)
                {
                    refMovies = await _movieContext.MovieFiles.AsNoTracking().Where(mov => mov.SearchFilter.Contains(movieFilters[i].Trim() + ",")).ToArrayAsync();

                    onList.AddRange(refMovies);
                }
            }
        }

        refMovies = onList.Distinct();

        MoviesListViewModel movies = new()
        {
            Movies = refMovies,
            IsImage = movie.IsImage1,
            IconType = movie.IconType1,
            IsPartsMoreOne = movie.IsPartsMoreOne1,
            PageHeadTitle = movie.HeadTitleForVideoLinks1
        };

        return View(movies);
    }
}