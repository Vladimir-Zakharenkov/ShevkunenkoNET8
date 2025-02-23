namespace ShevkunenkoSite.Services.Extensions;

public static class BooksAndArticlesModelExtension
{
    public static IEnumerable<BooksAndArticlesModel> BookSearch(this IEnumerable<BooksAndArticlesModel> booksAndArticlesModel, string? bookSearchString)
    {
        foreach (var foundBook in booksAndArticlesModel)
        {
            if (foundBook.AuthorOfText.Contains((bookSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                || foundBook.BookDescription.Contains((bookSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                || foundBook.CaptionOfText.Contains((bookSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
            {
                yield return foundBook;
            }
        }
    }
}