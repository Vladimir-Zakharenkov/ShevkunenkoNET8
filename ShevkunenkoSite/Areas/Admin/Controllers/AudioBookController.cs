namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AudioBookController(IAudioBookRepository audioBookContext) : Controller
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

    #region Добавить книгу или статью

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
}