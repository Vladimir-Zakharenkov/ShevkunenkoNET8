using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class BooksAndArticlesController(
    IBooksAndArticlesRepository bookContext,
    ITextInfoRepository textContext,
    IImageFileRepository imageContext) : Controller
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
                .Include(inc => inc.LogoOfArticle)
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

    [HttpGet]
    public ViewResult AddBook()
    {
        BooksAndArticlesModel newBook = new();

        return View(newBook);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBook(BooksAndArticlesModel addBook)
    {
        if (ModelState.IsValid)
        {
            #region Тип текста

            if (addBook.TypeOfText != null)
            {
                addBook.TypeOfText = addBook.TypeOfText.Trim();
            }
            else
            {
                addBook.TypeOfText = string.Empty;
            }

            #endregion


            #region Автор книги или статьи

            if (addBook.AuthorOfText != null)
            {
                addBook.AuthorOfText = addBook.AuthorOfText.Trim();
            }
            else
            {
                addBook.AuthorOfText = string.Empty;
            }

            #endregion

            #region Название книги или статьи

            addBook.BookDescription = addBook.BookDescription.Trim();

            #endregion

            #region Описание книги или статьи

            addBook.CaptionOfText = addBook.CaptionOfText.Trim();

            #endregion

            #region Колличество страниц

            addBook.NumberOfPages = addBook.NumberOfPages;

            #endregion

            #region добавить в БД

            await bookContext.AddBookOrArticleAsync(addBook);

            #endregion

            #region Открытие параметров добавленной книги (статьи)

            var newBook = await bookContext.BooksAndArticles.FirstAsync(book => book.CaptionOfText == addBook.CaptionOfText);

            return RedirectToAction(nameof(DetailsBook), new { bookId = newBook.BooksAndArticlesModelId });

            #endregion
        }
        else
        {
            return View();
        }
    }

    #endregion

    #region Изменить книгу или статью

    [HttpGet]
    public async Task<IActionResult> EditBook(Guid? bookId)
    {
        if (bookId.HasValue & await bookContext.BooksAndArticles.Where(book => book.BooksAndArticlesModelId == bookId).AnyAsync())
        {
            var editBook = await bookContext.BooksAndArticles.FirstAsync(book => book.BooksAndArticlesModelId == bookId);

            return View(new AddAndEditArticleViewModel
            {
                BookOrArticle = editBook
            });
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBook(AddAndEditArticleViewModel bookItem)
    {
        if (ModelState.IsValid)
        {
            BooksAndArticlesModel bookUpdate = await bookContext.BooksAndArticles
                .FirstAsync(book => book.BooksAndArticlesModelId == bookItem.BookOrArticle.BooksAndArticlesModelId);

            #region Тип текста

            bookUpdate.TypeOfText = bookItem.BookOrArticle.TypeOfText.Trim();

            #endregion

            #region Автор книги или статьи

            bookUpdate.AuthorOfText = bookItem.BookOrArticle.AuthorOfText.Trim();

            #endregion

            #region Логотип статьи

            if (bookItem.LogoOfArticleFormFile != null)
            {
                if (bookUpdate.TypeOfText == "book")
                {
                    bookUpdate.LogoOfArticleId = null;
                }
                else
                {
                    if (await imageContext.ImageFiles.Where(i => i.ImageFileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.IconFileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == bookItem.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == bookItem.LogoOfArticleFormFile.FileName);

                        bookUpdate.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else
                    {
                        ModelState.AddModelError("LogoOfArticleFormFile", $"Добавьте картинку «{bookItem.LogoOfArticleFormFile.FileName}» в базу данных");

                        return View(nameof(EditBook), new AddAndEditArticleViewModel
                        {
                            BookOrArticle = bookUpdate
                        });
                    }

                }
            }
            else
            {
                bookUpdate.LogoOfArticleId = bookItem.BookOrArticle.LogoOfArticleId;
            }

            #endregion

            #region Название книги или статьи

            bookUpdate.BookDescription = bookItem.BookOrArticle.BookDescription.Trim();

            #endregion

            #region Описание книги или статьи

            bookUpdate.CaptionOfText = bookItem.BookOrArticle.CaptionOfText.Trim();

            #endregion

            #region Колличество страниц

            bookUpdate.NumberOfPages = bookItem.BookOrArticle.NumberOfPages;

            #endregion

            #region Сохранить и перейти к DetailsBook

            await bookContext.SaveChangesInBookOrArticleAsync();

            return RedirectToAction(nameof(DetailsBook), new { bookId = bookUpdate.BooksAndArticlesModelId });

            #endregion
        }
        else
        {
            return View(bookItem);
        }
    }

    #endregion

    #region Удалить книгу или статью

    [HttpGet]
    public async Task<IActionResult> DeleteBook(Guid? bookId)
    {
        if (bookId.HasValue & await bookContext.BooksAndArticles.Where(book => book.BooksAndArticlesModelId == bookId).AnyAsync())
        {
            var deleteBook = await bookContext.BooksAndArticles.FirstAsync(book => book.BooksAndArticlesModelId == bookId);

            return View(deleteBook);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBook(BooksAndArticlesModel? deleteBook)
    {
        if (deleteBook != null)
        {
            if (await textContext.Texts.Where(text => text.BooksAndArticlesModelId == deleteBook.BooksAndArticlesModelId).AnyAsync())
            {
                ModelState.AddModelError("SequenceNumber", "Ссылки на книгу в базе текстов сайта");

                return View(deleteBook);
            }

            if (await bookContext.BooksAndArticles.Where(book => book.BooksAndArticlesModelId == deleteBook.BooksAndArticlesModelId).AnyAsync())
            {
                await bookContext.DeleteBookOrArticleAsync(deleteBook.BooksAndArticlesModelId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion
}