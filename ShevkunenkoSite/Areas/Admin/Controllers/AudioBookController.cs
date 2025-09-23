using Microsoft.AspNetCore.Mvc.RazorPages;

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

    #region Добавить книгу или статью

    [HttpGet]
    public ViewResult AddAudioBook()
    {
        AudioBookModel newAudioBook = new();

        return View(newAudioBook);
    }

    #endregion
}