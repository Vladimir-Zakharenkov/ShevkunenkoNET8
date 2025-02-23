using System.Configuration;

namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class BooksAndArticlesController(
    IBooksAndArticlesRepository bookContext) : Controller
{
    #region Список книг и статей

    public int booksPerPage = 16;

    public IActionResult Index(string? bookSearchString, int pageNumber = 1)
    {
        var allBooks = bookContext.BooksAndArticles.BookSearch(bookSearchString);

        return View(new BooksAndArticlesViewModel
        {
            AllBooks = [.. allBooks
                     .Skip((pageNumber - 1) * booksPerPage)
                     .Take(booksPerPage)],

            PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = pageNumber,
                ItemsPerPage = booksPerPage,
                TotalItems = allBooks.Count()
            },

            BookSearchString = bookSearchString ?? string.Empty
        });
    }

    #endregion

    #region Информация о книге или статье

    public async Task<IActionResult> DetailsBook(Guid? bookId)
    {
        if (bookId.HasValue & await bookContext.BooksAndArticles.Where(b => b.BooksAndArticlesModelId == bookId).AnyAsync())
        {
            var bookItem = await bookContext.BooksAndArticles
                 .AsNoTracking()
                 .FirstAsync(b => b.BooksAndArticlesModelId == bookId);

            return View(bookItem);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion

    #region Добавить книгу или статью

    public IActionResult AddBook()
    {
        return View();
    }

    #endregion
}