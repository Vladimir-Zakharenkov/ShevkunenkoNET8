namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AudioBookController(IAudioBookRepository audioBookContext) : Controller
{
    #region Список аудиокниг

    public int booksPerPage = 16;

    public IActionResult Index(string? audioBookSearchString, int pageNumber = 1)
    {
        var allAudioBooks = audioBookContext.AudioBooks.AudioBookSearch(audioBookSearchString);

        return View(new ItemsListViewModel
        {
            AllAudioBooks = [.. allAudioBooks
                     .Skip((pageNumber - 1) * booksPerPage)
                     .Take(booksPerPage)],

            CurrentPage = pageNumber,
            ItemsPerPage = booksPerPage,
            TotalItems = allAudioBooks.Count(),

            SearchString = audioBookSearchString ?? string.Empty
        });
    }

    #endregion
}