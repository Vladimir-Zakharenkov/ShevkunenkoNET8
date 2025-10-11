namespace ShevkunenkoSite.Services.Interfaces;

public class AudioInfoImplementation(SiteDbContext siteContext) : IAudioInfoRepository
{
    #region Получить все аудиофайлы

    IQueryable<AudioInfoModel> IAudioInfoRepository.AudioFiles => siteContext.AudioInfoModel;

    #endregion

    #region Добавить аудиофайл

    public async Task AddAudioFileAsync(AudioInfoModel audioFile)
    {
        _ = await siteContext.AudioInfoModel
            .AddAsync(audioFile);

        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Сохранить изменения при редактировании

    public async Task SaveChangesInAudioFileAsync()
    {
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Удалить аудиофайл

    public async Task DeleteAudioFileAsync(Guid audioFileId)
    {
        if (await siteContext.AudioInfoModel
            .Where(audioFile => audioFile.AudioInfoModelId == audioFileId)
            .AnyAsync())
        {
            AudioInfoModel audioFileToDelete = await siteContext.AudioInfoModel
                .FirstAsync(audioFile => audioFile.AudioBookModelId == audioFileId);

            _ = siteContext.AudioInfoModel
                .Remove(audioFileToDelete);

            _ = await siteContext.SaveChangesAsync();
        }
    }

    #endregion
}