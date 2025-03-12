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
        AddAndEditArticleViewModel newBook = new();

        return View(newBook);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBook(AddAndEditArticleViewModel addBook)
    {
        if (ModelState.IsValid)
        {
            #region Тип текста

            if (addBook.BookOrArticle.TypeOfText != null)
            {
                _ = addBook.BookOrArticle.TypeOfText.Trim();
            }
            else
            {
                addBook.BookOrArticle.TypeOfText = string.Empty;
            }

            #endregion

            #region Издатель книги или статьи

            _ = addBook.BookOrArticle.Publisher.Trim();

            #endregion

            #region Логотип статьи

            if (addBook.LogoOfArticleFormFile != null)
            {
                if (addBook.BookOrArticle.TypeOfText == "book")
                {
                    addBook.BookOrArticle.LogoOfArticleId = null;
                }
                else
                {
                    if (await imageContext.ImageFiles.Where(i => i.ImageFileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.IconFileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == addBook.LogoOfArticleFormFile.FileName).AnyAsync())
                    {
                        var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == addBook.LogoOfArticleFormFile.FileName);

                        addBook.BookOrArticle.LogoOfArticleId = newImage.ImageFileModelId;
                    }
                    else
                    {
                        ModelState.AddModelError("LogoOfArticleFormFile", $"Добавьте картинку «{addBook.LogoOfArticleFormFile.FileName}» в базу данных");

                        return View(nameof(AddBook), new AddAndEditArticleViewModel
                        {
                            BookOrArticle = addBook.BookOrArticle
                        });
                    }

                }
            }
            else
            {
                _ = addBook.BookOrArticle.LogoOfArticleId;
            }

            #endregion

            #region Автор книги или статьи

            if (addBook.BookOrArticle.AuthorOfText != null)
            {
                _ = addBook.BookOrArticle.AuthorOfText.Trim();
            }
            else
            {
                addBook.BookOrArticle.AuthorOfText = string.Empty;
            }

            #endregion

            #region Название книги или статьи

            _ = addBook.BookOrArticle.BookDescription.Trim();

            #endregion

            #region Описание книги или статьи

            addBook.BookOrArticle.CaptionOfText = addBook.BookOrArticle.CaptionOfText.Trim();

            #endregion

            #region Колличество страниц

            _ = addBook.BookOrArticle.NumberOfPages;

            #endregion

            #region Дата публикации книги (статьи)

            _ = addBook.BookOrArticle.DateOfPublication;

            #endregion

            #region Теги по содержанию книги (статьи)

            _ = addBook.BookOrArticle.TagsForBook.Trim();

            #endregion

            #region Добавить в БД

            await bookContext.AddBookOrArticleAsync(addBook.BookOrArticle);

            #endregion

            #region Открытие параметров добавленной книги (статьи)

            var newBook = await bookContext.BooksAndArticles.FirstAsync(book => book.CaptionOfText == addBook.BookOrArticle.CaptionOfText);

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

            #region Издатель книги или статьи

            bookUpdate.Publisher = bookItem.BookOrArticle.Publisher.Trim();

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

            #region Дата публикации книги (статьи)

            bookUpdate.DateOfPublication = bookItem.BookOrArticle.DateOfPublication;

            #endregion

            #region Теги по содержанию книги (статьи)

            bookUpdate.TagsForBook = bookItem.BookOrArticle.TagsForBook.Trim();

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