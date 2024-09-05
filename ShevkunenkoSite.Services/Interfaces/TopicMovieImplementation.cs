namespace ShevkunenkoSite.Services.Interfaces;

public class TopicMovieImplementation(SiteDbContext siteContext) : ITopicMovieRepository
{
    public IQueryable<TopicMovieModel> TopicMovies => siteContext.TopicMovie;

    #region Добавить тему видео
    public async Task AddNewTopicMovieAsync(TopicMovieModel topic)
    {
        _ = await siteContext.TopicMovie.AddAsync(topic);
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Сохранить изменения при редактировании

    public async Task SaveChangesInTopicMovieAsync()
    {
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Удалить тему видео

    public async Task DeleteTopicMovieAsync(Guid topicId)
    {
        if (await siteContext.TopicMovie.Where(i => i.TopicMovieModelId == topicId).AnyAsync())
        {
            TopicMovieModel topicToDelete = await siteContext.TopicMovie.FirstAsync(i => i.TopicMovieModelId == topicId);

            _ = siteContext.TopicMovie.Remove(topicToDelete);
            _ = await siteContext.SaveChangesAsync();
        }
    }

    #endregion
}