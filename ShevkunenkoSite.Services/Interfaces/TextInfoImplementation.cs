namespace ShevkunenkoSite.Services.Interfaces;

public class TextInfoImplementation(SiteDbContext siteContext) : ITextInfoRepository
{
    public IQueryable<TextInfoModel> Texts => siteContext.TextFile;

    #region Добавить текст

    public async Task AddNewTextAsync(TextInfoModel text)
    {
        _ = await siteContext.TextFile.AddAsync(text);
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Сохранить изменения при редактировании

    public async Task SaveChangesInTextAsync()
    {
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Удалить текст

    public async Task DeleteTextAsync(Guid textId)
    {
        if (await siteContext.TextFile.Where(i => i.TextInfoModelId == textId).AnyAsync())
        {
            TextInfoModel textToDelete = await siteContext.TextFile.FirstAsync(i => i.TextInfoModelId == textId);

            _ = siteContext.TextFile.Remove(textToDelete);
            _ = await siteContext.SaveChangesAsync();
        }
    }

    #endregion
}