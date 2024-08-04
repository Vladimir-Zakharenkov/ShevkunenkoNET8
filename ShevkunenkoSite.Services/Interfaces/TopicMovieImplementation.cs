using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Services.Interfaces;

public class TopicMovieImplementation : ITopicMovieRepository
{
    private readonly SiteDbContext _siteContext;
    public TopicMovieImplementation(SiteDbContext siteContext) => _siteContext = siteContext;

    public IQueryable<TopicMovieModel> TopicMovies => _siteContext.TopicMovie;

    public async Task AddNewTopicMovieAsync(TopicMovieModel topic)
    {
        _ = await _siteContext.TopicMovie.AddAsync(topic);
        _ = await _siteContext.SaveChangesAsync();
    }

    #region SaveChangesInTopicMovieAsync

    public async Task SaveChangesInTopicMovieAsync()
    {
        _ = await _siteContext.SaveChangesAsync();
    }

    #endregion

    public async Task DeleteTopicMovieAsync(Guid topicId)
    {
        if (await _siteContext.TopicMovie.Where(i => i.TopicMovieModelId == topicId).AnyAsync())
        {
            TopicMovieModel topicToDelete = await _siteContext.TopicMovie.FirstAsync(i => i.TopicMovieModelId == topicId);

            _ = _siteContext.TopicMovie.Remove(topicToDelete);
            _ = await _siteContext.SaveChangesAsync();
        }
    }
}