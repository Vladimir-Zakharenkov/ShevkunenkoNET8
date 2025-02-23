namespace ShevkunenkoSite.Services.Interfaces;

public class BooksAndArticlesImplementation(SiteDbContext siteContext) : IBooksAndArticlesRepository
{
    public IQueryable<BooksAndArticlesModel> BooksAndArticles => siteContext.BooksAndArticles;

    #region Добавить книгу (статью)

    public async Task AddBookOrArticleAsync(BooksAndArticlesModel book)
    {
        _ = await siteContext.BooksAndArticles.AddAsync(book);
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Сохранить изменения при редактировании

    public async Task SaveChangesInBookOrArticleAsync()
    {
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Удалить текст

    public async Task DeleteBookOrArticleAsync(Guid bookId)
    {
        if (await siteContext.BooksAndArticles.Where(b => b.BooksAndArticlesModelId == bookId).AnyAsync())
        {
            BooksAndArticlesModel bookToDelete = await siteContext.BooksAndArticles.FirstAsync(b => b.BooksAndArticlesModelId == bookId);

            _ = siteContext.BooksAndArticles.Remove(bookToDelete);
            _ = await siteContext.SaveChangesAsync();
        }
    }

    #endregion
}
