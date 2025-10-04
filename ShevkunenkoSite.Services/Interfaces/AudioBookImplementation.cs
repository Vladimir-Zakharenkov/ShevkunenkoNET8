
namespace ShevkunenkoSite.Services.Interfaces;

public class AudioBookImplementation(SiteDbContext siteContext) : IAudioBookRepository
{
    #region Получить все аудиокниги

    IQueryable<AudioBookModel> IAudioBookRepository.AudioBooks => siteContext.AudioBookModel;

    #endregion

    #region Добавить аудиокнигу

    public async Task AddAudioBookAsync(AudioBookModel audioBook)
    {
        _ = await siteContext.AudioBookModel
            .AddAsync(audioBook);

        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Сохранить изменения при редактировании

    public async Task SaveChangesInAudioBookAsync()
    {
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Удалить аудиокнигу

    public async Task DeleteAudioBookAsync(Guid audioBookId)
    {
        if (await siteContext.AudioBookModel
            .Where(audioBook => audioBook.AudioBookModelId == audioBookId)
            .AnyAsync())
        {
            AudioBookModel audioBookToDelete = await siteContext.AudioBookModel
                .FirstAsync(audioBook => audioBook.AudioBookModelId == audioBookId);

            _ = siteContext.AudioBookModel
                .Remove(audioBookToDelete);

            _ = await siteContext.SaveChangesAsync();
        }
    }

    #endregion
}
