
namespace ShevkunenkoSite.Services.Interfaces;

public class AudioBookImplementation(SiteDbContext siteContext) : IAudioBookRepository
{
    IQueryable<AudioBookModel> IAudioBookRepository.AudioBooks => siteContext.AudioBookModel;

    #region Добавить книгу (статью)

    public async Task AddAudioBookAsync(AudioBookModel audioBook)
    {
        _ = await siteContext.AudioBookModel.AddAsync(audioBook);
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Сохранить изменения при редактировании

    public async Task SaveChangesInAudioBookAsync()
    {
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Удалить текст

    public async Task DeleteAudioBookAsync(Guid audioBookId)
    {
        if (await siteContext.AudioBookModel.Where(b => b.AudioBookModelId == audioBookId).AnyAsync())
        {
            AudioBookModel audioBookToDelete = await siteContext.AudioBookModel.FirstAsync(b => b.AudioBookModelId == audioBookId);

            _ = siteContext.AudioBookModel.Remove(audioBookToDelete);
            _ = await siteContext.SaveChangesAsync();
        }
    }

    #endregion
}
