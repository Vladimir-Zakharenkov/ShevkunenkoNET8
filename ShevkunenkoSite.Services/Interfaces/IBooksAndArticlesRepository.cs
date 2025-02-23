namespace ShevkunenkoSite.Services.Interfaces;

public interface IBooksAndArticlesRepository
{
    IQueryable<BooksAndArticlesModel> BooksAndArticles { get; }

    Task AddBookOrArticleAsync(BooksAndArticlesModel book);

    Task SaveChangesInBookOrArticleAsync();

    Task DeleteBookOrArticleAsync(Guid bookId);
}