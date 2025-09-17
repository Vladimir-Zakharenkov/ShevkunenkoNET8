
namespace ShevkunenkoSite.Services.Interfaces;

public class AudioBookImplementation(SiteDbContext siteContext) : IAudioBookRepository
{
    IQueryable<AudioBookModel> IAudioBookRepository.AudioBooks => siteContext.AudioBook;

    #region Добавить книгу (статью)

    public async Task AddAudioBookAsync(AudioBookModel audioBook)
    {
        _ = await siteContext.AudioBook.AddAsync(audioBook);
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
        if (await siteContext.AudioBook.Where(b => b.AudioBookModelId == audioBookId).AnyAsync())
        {
            AudioBookModel audioBookToDelete = await siteContext.AudioBook.FirstAsync(b => b.AudioBookModelId == audioBookId);

            _ = siteContext.AudioBook.Remove(audioBookToDelete);
            _ = await siteContext.SaveChangesAsync();
        }
    }

    #endregion
}
