namespace ShevkunenkoSite.Services.Interfaces;

public class MovieFileImplementation : IMovieFileRepository
{
    private readonly SiteDbContext _siteContext;
    public MovieFileImplementation(SiteDbContext siteContext) => _siteContext = siteContext;

    public IQueryable<MovieFileModel> MovieFiles => _siteContext.MovieFile.Include(p => p.PageInfoModel).Include(i => i.ImageFileModel);

    public async Task SaveChangesInMovieAsync()
    {
        await _siteContext.SaveChangesAsync();
    }

    public async Task AddNewMovieAsync(MovieFileModel movie)
    {
        await _siteContext.MovieFile.AddAsync(movie);
        await SaveChangesInMovieAsync();
    }

    public async Task DeleteMovieAsync(Guid movieId)
    {
        if (await _siteContext.MovieFile.Where(i => i.MovieFileModelId == movieId).AnyAsync())
        {
            MovieFileModel movieToDelete = await _siteContext.MovieFile.FirstAsync(i => i.MovieFileModelId == movieId);

            _ = _siteContext.MovieFile.Remove(movieToDelete);
            _ = await _siteContext.SaveChangesAsync();
        }
    }
}