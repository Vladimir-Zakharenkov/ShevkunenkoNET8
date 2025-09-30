using Microsoft.AspNetCore.Mvc.Rendering;
using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AudioBookController(
    IAudioBookRepository audioBookContext,
    IBooksAndArticlesRepository bookContext) : Controller
{
    #region Список аудиокниг

    public IActionResult Index(string? searchString, int pageNumber = 1)
    {
        var allAudioBooks = audioBookContext.AudioBooks.AudioBookSearch(searchString);

        return View(new ItemsListViewModel
        {
            AllAudioBooks = [.. allAudioBooks
                     .Skip((pageNumber - 1) * DataConfig.NumberOfItemsPerPage)
                     .Take(DataConfig.NumberOfItemsPerPage)],

            CurrentPage = pageNumber,
            ItemsPerPage = DataConfig.NumberOfItemsPerPage,
            TotalItems = allAudioBooks.Count(),

            SearchString = searchString ?? string.Empty
        });
    }

    #endregion

    #region Информация об аудиокниге

    public async Task<IActionResult> DetailsAudioBook(Guid? audioBookId)
    {
        if (audioBookId.HasValue & await audioBookContext.AudioBooks.Where(audioBook => audioBook.AudioBookModelId == audioBookId).AnyAsync())
        {
            var audioBook = await audioBookContext.AudioBooks
                .FirstAsync(audioBook => audioBook.AudioBookModelId == audioBookId);

            return View(audioBook);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion

    #region Добавить аудиокнигу

    [HttpGet]
    public ViewResult AddAudioBook()
    {
        AudioBookModel newAudioBook = new();

        return View(newAudioBook);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAudioBook(AudioBookModel addAudioBook)
    {
        if (ModelState.IsValid)
        {
            #region Название аудиокниги

            _ = addAudioBook.CaptionOfAudioBook.Trim();

            #endregion

            #region Описание аудиокниги

            _ = addAudioBook.AudioBookDescription.Trim();

            #endregion

            #region Исполнитель

            _ = addAudioBook.ActorOfAudioBook.Trim();

            #endregion

            #region Количество файлов

            _ = addAudioBook.NumberOfFiles;

            #endregion

            #region Добавить в БД

            await audioBookContext.AddAudioBookAsync(addAudioBook);

            #endregion

            #region Открытие параметров добавленной аудиокниги

            var newAudioBook = await audioBookContext.AudioBooks.FirstAsync(audioBook => audioBook.CaptionOfAudioBook == addAudioBook.CaptionOfAudioBook & audioBook.ActorOfAudioBook == addAudioBook.ActorOfAudioBook);

            return RedirectToAction(nameof(DetailsAudioBook), new { audiBbookId = newAudioBook.AudioBookModelId });

            #endregion
        }
        else
        {
            return View(nameof(AddAudioBook), addAudioBook);
        }
    }

    #endregion

    #region Изменить аудиокнигу

    [HttpGet]
    public async Task<IActionResult> EditAudioBook(Guid? audioBookId)
    {
        if (audioBookId.HasValue & await audioBookContext.AudioBooks.Where(audioBook => audioBook.AudioBookModelId == audioBookId).AnyAsync())
        {
            #region Экземпляр редактируемой аудиокниги

            var editAudioBook = await audioBookContext.AudioBooks
                .Include(inc => inc.BookForAudioBook)
                .FirstAsync(audioBook => audioBook.AudioBookModelId == audioBookId);

            #endregion

            #region SelectListItem из названий книг на сайте

            var booksOnSite = await bookContext.BooksAndArticles
                .Where(booksAndArticles => booksAndArticles.TypeOfText == "book")
                .ToListAsync();

            var seletListItemFromBookOnSite = booksOnSite
                .OrderBy(book => book.CaptionOfText)
                .Select(selectListItem => new SelectListItem
                {
                    Value = selectListItem.CaptionOfText,
                    Text = selectListItem.CaptionOfText
                })
                .ToList();

            seletListItemFromBookOnSite.Insert(0, new SelectListItem { Text = "книга не задана", Value = "книга не задана" });

            #endregion

            return View(new AudioBookViewModel
            {
                AudioBookModelId = editAudioBook.AudioBookModelId,
                CaptionOfAudioBook = editAudioBook.CaptionOfAudioBook,
                AudioBookDescription = editAudioBook.AudioBookDescription,
                ActorOfAudioBook = editAudioBook.ActorOfAudioBook,
                NumberOfFiles = editAudioBook.NumberOfFiles,
                BookForAudioBookId = editAudioBook.BookForAudioBookId,
                PageInfoModelId = editAudioBook.PageInfoModelId,
                BooksOnSite = seletListItemFromBookOnSite
            });
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAudioBook(AudioBookModel audioBookItem)
    {
        AudioBookModel audioBookUpdate = await audioBookContext.AudioBooks
            .FirstAsync(audioBook => audioBook.AudioBookModelId == audioBookItem.AudioBookModelId);

        if (ModelState.IsValid)
        {
            #region Название аудиокниги

            audioBookUpdate.CaptionOfAudioBook = audioBookItem.CaptionOfAudioBook.Trim();

            #endregion

            #region Описание аудиокниги

            audioBookUpdate.AudioBookDescription = audioBookItem.AudioBookDescription.Trim();

            #endregion

            #region Исполнитель

            audioBookUpdate.ActorOfAudioBook = audioBookItem.ActorOfAudioBook.Trim();

            #endregion

            #region Количество файлов

            audioBookUpdate.NumberOfFiles = audioBookItem.NumberOfFiles;

            #endregion

            #region Сохранить и перейти к DetailsAudioBook

            await audioBookContext.SaveChangesInAudioBookAsync();

            return RedirectToAction(nameof(DetailsAudioBook), new { audioBookId = audioBookUpdate.AudioBookModelId });

            #endregion

        }
        else
        {
            return View(nameof(EditAudioBook), audioBookItem);
        }
    }

    #endregion
}