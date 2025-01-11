namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class TopicMovieController(ITopicMovieRepository topicContext, IMovieFileRepository movieContext) : Controller
{
    #region Список тем видео

    public int topicsPerPage = 10;

    public async Task<IActionResult> Index(string? topicMovieSearchString,
                                                                int pageNumber = 1)
    {
        var allTopics = from m in topicContext.TopicMovies
            .Where
            (
                s => s.TopicDescription.Contains((topicMovieSearchString ?? string.Empty).Trim())
            )
                        select m;

        return View(new TopicMovieViewModel
        {
            AllTopics = await allTopics
                     .Skip((pageNumber - 1) * topicsPerPage)
                     .Take(topicsPerPage)
                     .ToArrayAsync(),

            PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = pageNumber,
                ItemsPerPage = topicsPerPage,
                TotalItems = allTopics.Count()
            },

            TopicMovieSearchString = topicMovieSearchString ?? string.Empty
        });
    }

    #endregion

    #region Информация о теме

    public async Task<IActionResult> DetailsTopicMovie(Guid? topicId)
    {
        if (topicId.HasValue && await topicContext.TopicMovies.Where(t => t.TopicMovieModelId == topicId).AnyAsync())
        {
            TopicMovieModel topicItem = await topicContext.TopicMovies
                .AsNoTracking()
                .FirstAsync(t => t.TopicMovieModelId == topicId);

            return View(topicItem);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion

    #region Добавить тему видео

    [HttpGet]
    public ViewResult AddTopicMovie()
    {
        TopicMovieModel editTopicMovie = new();

        return View(editTopicMovie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTopicMovie(TopicMovieModel topicMovieItem)
    {
        if (ModelState.IsValid)
        {
            #region Описание

            topicMovieItem.TopicDescription = topicMovieItem.TopicDescription.Trim();

            #endregion

            #region Заголовок страницы

            topicMovieItem.TopicHeadPage = topicMovieItem.TopicHeadPage.Trim();

            #endregion

            #region Тип картинки для ссылки

            topicMovieItem.ImageForRef = topicMovieItem.ImageForRef;

            #endregion

            #region Формат картинки для ссылки

            topicMovieItem.IconTypeForRef = topicMovieItem.IconTypeForRef;

            #endregion

            #region Ссылка на страницу серий

            topicMovieItem.GeneralPageForMovieEpisodes = topicMovieItem.GeneralPageForMovieEpisodes;

            #endregion

            #region Количество ссылок на странице

            topicMovieItem.NumberOfLinksPerPage = topicMovieItem.NumberOfLinksPerPage;

            #endregion

            await topicContext.AddNewTopicMovieAsync(topicMovieItem);

            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(new TopicMovieModel());
        }
    }

    #endregion

    #region Изменить тему видео

    [HttpGet]
    public async Task<IActionResult> EditTopicMovie(Guid? topicId)
    {
        TopicMovieModel editTopicMovie = new();

        if (topicId.HasValue && await topicContext.TopicMovies.Where(t => t.TopicMovieModelId == topicId).AnyAsync())
        {
            editTopicMovie = await topicContext.TopicMovies.FirstAsync(t => t.TopicMovieModelId == topicId);

            return View(editTopicMovie);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTopicMovie(TopicMovieModel editTopicMovie)
    {
        if (ModelState.IsValid)
        {
            TopicMovieModel updateTopicMovie = await topicContext.TopicMovies.FirstAsync(t => t.TopicMovieModelId == editTopicMovie.TopicMovieModelId);

            #region Описание

            updateTopicMovie.TopicDescription = editTopicMovie.TopicDescription.Trim();

            #endregion

            #region Заголовок страницы

            updateTopicMovie.TopicHeadPage = editTopicMovie.TopicHeadPage.Trim();

            #endregion

            #region Тип картинки для ссылки

            updateTopicMovie.ImageForRef = editTopicMovie.ImageForRef;

            #endregion

            #region Формат картинки для ссылки

            updateTopicMovie.IconTypeForRef = editTopicMovie.IconTypeForRef;

            #endregion

            #region Ссылка на страницу серий

            updateTopicMovie.GeneralPageForMovieEpisodes = editTopicMovie.GeneralPageForMovieEpisodes;

            #endregion

            #region Количество ссылок на странице

            updateTopicMovie.NumberOfLinksPerPage = editTopicMovie.NumberOfLinksPerPage;

            #endregion

            await topicContext.SaveChangesInTopicMovieAsync();

            return RedirectToAction("DetailsTopicMovie", new { topicId = updateTopicMovie.TopicMovieModelId, Area = "Admin" });
        }
        else
        {
            return View();
        }
    }

    #endregion

    #region Удалить тему видео

    public async Task<IActionResult> DeleteTopicMovie(Guid? topicId)
    {
        TopicMovieModel deleteTopicMovie = new();

        if (topicId.HasValue && await topicContext.TopicMovies.Where(t => t.TopicMovieModelId == topicId).AnyAsync())
        {
            deleteTopicMovie = await topicContext.TopicMovies.FirstAsync(t => t.TopicMovieModelId == topicId);

            ViewBag.moviesOfTopic = await movieContext.MovieFiles
                    .Where(p => p.TopicGuidList.Contains(topicId.ToString()!) == true)
                    .ToArrayAsync();

            return View(deleteTopicMovie);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTopicMovie(TopicMovieModel deleteTopicMovie)
    {
        if (deleteTopicMovie != null)
        {
            var moviesOfTopic = await movieContext.MovieFiles
                    .Where(p => p.TopicGuidList.Contains(deleteTopicMovie.TopicMovieModelId.ToString()!) == true)
                    .ToArrayAsync();

            if (moviesOfTopic.Length > 0)
            {
                string tempString = string.Empty;

                foreach (var movie in moviesOfTopic)
                {
                    string[] tempStrings = movie.TopicGuidList.Split(',');

                    foreach (var tempString2 in tempStrings)
                    {
                        if (tempString2.ToLower() != deleteTopicMovie.TopicMovieModelId.ToString().ToLower())
                        {
                            tempString = tempString + tempString2 + ',';
                        }

                    }

                    movie.TopicGuidList = tempString.TrimEnd(',');

                    await movieContext.SaveChangesInMovieAsync();

                    tempString = string.Empty;
                }
            }
            await topicContext.DeleteTopicMovieAsync(deleteTopicMovie.TopicMovieModelId);

            return RedirectToAction(nameof(Index));
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion
}
