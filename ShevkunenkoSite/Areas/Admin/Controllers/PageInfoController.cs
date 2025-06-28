namespace ShevkunenkoSite.Areas.Admin.Controllers;

// Имена методов не начинать со слова Page
[Area("Admin")]
[Authorize]
public class PageInfoController(
    IPageInfoRepository pageInfoContext,
    IMovieFileRepository movieContext,
    IIconFileRepository iconContext,
    IImageFileRepository imageContext,
    IBackgroundFotoRepository backgroundContext
    ) : Controller
{
    #region Список страниц сайта

    public int pagesPerPage = 10;

    private static readonly char[] separator = [','];

    public async Task<ViewResult> Index(string? pageTitleSearchString, string? pageDescriptionSearchString, string? keyWordSearchString, bool pageCard = false, int pageNumber = 1)
    {
        var allPages = from p in pageInfoContext.PagesInfo
                       select p;

        if (!string.IsNullOrEmpty(pageTitleSearchString))
        {
            allPages = allPages.Where(s => s.PageTitle.Contains(pageTitleSearchString));
        }

        if (!string.IsNullOrEmpty(pageDescriptionSearchString))
        {
            allPages = allPages.Where(s => s.PageDescription.Contains(pageDescriptionSearchString));
        }

        if (!string.IsNullOrEmpty(keyWordSearchString))
        {
            allPages = allPages.Where(s => s.PageDescription.Contains(keyWordSearchString));
        }

        if (pageCard == false)
        {
            return View(new PagesListViewModel
            {
                Pages = await allPages
                    .Skip((pageNumber - 1) * pagesPerPage)
                    .Take(pagesPerPage)
                    .ToArrayAsync(),

                PagingInfo = new PagingInfoViewModel
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = pagesPerPage,
                    TotalItems = allPages.Count()
                },

                PageTitleSearchString = pageTitleSearchString ?? string.Empty,
                PageDescriptionSearchString = pageDescriptionSearchString ?? string.Empty,
                KeyWordSearchString = keyWordSearchString ?? string.Empty,
                PageCard = false,
                PageNumber = pageNumber
            });
        }
        else
        {
            return View("PageCardList", new PagesListViewModel
            {
                Pages = await allPages
                    .Skip((pageNumber - 1) * pagesPerPage)
                    .Take(pagesPerPage)
                    .ToArrayAsync(),

                PagingInfo = new PagingInfoViewModel
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = pagesPerPage,
                    TotalItems = allPages.Count()
                },

                PageTitleSearchString = pageTitleSearchString ?? string.Empty,
                PageDescriptionSearchString = pageDescriptionSearchString ?? string.Empty,
                KeyWordSearchString = keyWordSearchString ?? string.Empty,
                PageCard = true,
                PageNumber = pageNumber
            });

        }
    }

    #endregion

    #region Информация о странице сайта

    public async Task<IActionResult> DetailsPage(Guid? pageId)
    {
        if (pageId.HasValue)
        {
            #region Инициализация экземпляра страницы

            PageInfoModel pageItem;

            if (await pageInfoContext.PagesInfo.Where(p => p.PageInfoModelId == pageId).AnyAsync())
            {
                pageItem = await pageInfoContext.PagesInfo.Include(p => p.ImagePageHeading)
                    .AsNoTracking()
                    .FirstAsync(p => p.PageInfoModelId == pageId);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            #endregion

            #region Инициализация экземпляра иконки для страницы

            IconFileModel iconItem;

            if (await iconContext.IconFiles
                .Where(icon => icon.IconPath == pageItem.PageIconPath && icon.IconFileName == DataConfig.IconItem).AnyAsync())
            {
                iconItem = await iconContext.IconFiles
                    .FirstAsync(icon => icon.IconPath == pageItem.PageIconPath && icon.IconFileName == DataConfig.IconItem);
            }
            else
            {
                iconItem = await iconContext.IconFiles.FirstAsync(icon => icon.IconPath == "main/" && icon.IconFileName == DataConfig.IconItem);
            }

            #endregion

            #region Ссылки на текущую страницу по GUID (1)

            List<PageInfoModel> linksFromPagesByGuid = [];

#pragma warning disable CA1862
            linksFromPagesByGuid
                .AddRange(await pageInfoContext.PagesInfo
                    .Where(p => p.RefPages.ToLower().Contains(pageItem.PageInfoModelId.ToString().ToLower()))
                .ToArrayAsync());
#pragma warning restore CA1862

            _ = linksFromPagesByGuid.Distinct().OrderBy(p => p.SortOfPage);

            #endregion

            #region Ссылки на текущую страницу по GUID (2)

            List<PageInfoModel> linksFromPagesByGuid2 = [];

#pragma warning disable CA1862
            linksFromPagesByGuid2
                .AddRange(await pageInfoContext.PagesInfo
                    .Where(p => p.RefPages2.ToLower().Contains(pageItem.PageInfoModelId.ToString().ToLower()))
                .ToArrayAsync());
#pragma warning restore CA1862

            _ = linksFromPagesByGuid2.Distinct().OrderBy(p => p.SortOfPage);

            #endregion

            #region Ссылки на текущую страницу по фильтру PageFilter

            string[] pageFilters = pageItem.PageFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);

            List<PageInfoModel> linksFromPagesByPageFilter = [];

            Dictionary<string, List<PageInfoModel>> dictionaryOfOutPages = [];

            if (pageFilters.Length > 0)
            {
                for (int i = 0; i < pageFilters.Length; i++)
                {
                    if (await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFilters[i] + ',')).AnyAsync())
                    {
                        var listOfFilterOut = await pageInfoContext.PagesInfo.Where(p => p.PageFilterOut.Contains(pageFilters[i] + ',')).ToListAsync();

                        _ = listOfFilterOut.Distinct().OrderBy(p => p.SortOfPage);

                        dictionaryOfOutPages[pageFilters[i]] = listOfFilterOut;
                    }
                }
            }

            #endregion

            #region Ссылки на страницы сайта по GUID (RefPages)

            string[] pageIdOut = pageItem.RefPages.Split(',', StringSplitOptions.RemoveEmptyEntries);

            List<PageInfoModel> linksToPagesByGuid = [];

            if (pageIdOut.Length > 0)
            {
                foreach (string refPageId in pageIdOut)
                {
                    if (Guid.TryParse(refPageId, out Guid pageGuid))
                    {
                        if (await pageInfoContext.PagesInfo.Where(p => p.PageInfoModelId == pageGuid).AnyAsync())
                        {
                            linksToPagesByGuid.Add(await pageInfoContext.PagesInfo.FirstAsync(p => p.PageInfoModelId == pageGuid));
                            _ = linksToPagesByGuid.Distinct().OrderBy(p => p.PageCardText);
                        }
                    }
                }
            }

            #endregion

            #region Ссылки на страницы сайта по GUID (RefPages2)

            string[] pageIdOut2 = pageItem.RefPages2.Split(',', StringSplitOptions.RemoveEmptyEntries);

            List<PageInfoModel> linksToPagesByGuid2 = [];

            if (pageIdOut.Length > 0)
            {
                foreach (string refPageId2 in pageIdOut2)
                {
                    if (Guid.TryParse(refPageId2, out Guid pageGuid2))
                    {
                        if (await pageInfoContext.PagesInfo.Where(p => p.PageInfoModelId == pageGuid2).AnyAsync())
                        {
                            linksToPagesByGuid2.Add(await pageInfoContext.PagesInfo.FirstAsync(p => p.PageInfoModelId == pageGuid2));

                            _ = linksToPagesByGuid2.Distinct().OrderBy(p => p.PageCardText);
                        }
                    }
                }
            }

            #endregion

            #region Ссылки на страницы сайта по фильтру (PageFilterOut)

            string[] pageFiltersOut = pageItem.PageFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

            List<PageInfoModel> linksToPagesByFilterOut = [];

            if (pageFiltersOut.Length > 0)
            {
                for (int i = 0; i < pageFiltersOut.Length; i++)
                {
                    linksToPagesByFilterOut.AddRange(await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFiltersOut[i].Trim() + ",")).ToArrayAsync());

                    _ = linksToPagesByFilterOut.Distinct().OrderBy(p => p.PageCardText);
                }
            }

            #endregion

            #region  Ссылки на видео сайта по текстовому фильтру (VideoFilterOut)

            List<VideoLinksViewModel> listOfVideoLinksViewModel = [];
            List<List<MovieFileModel>> listOfListMoviesFileModel = [];

            string[] videoFilterOut = pageItem.VideoFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (videoFilterOut.Length > 0)
            {
                for (int i = 0; i < videoFilterOut.Length; i++)
                {
                    if (await movieContext.MovieFiles.Where(p => p.SearchFilter.Contains(videoFilterOut[i])).AnyAsync())
                    {
                        var movies = await movieContext.MovieFiles.Where(p => p.SearchFilter.Contains(videoFilterOut[i]) & p.MovieInMainList == true).ToListAsync();
                        movies.Sort((movies1, movies2) => movies1.MovieDatePublished.CompareTo(movies2.MovieDatePublished));

                        VideoLinksViewModel videoLinksViewModel = new()
                        {
                            HeadTitleForVideoLinks = videoFilterOut[i],
                            IsImage = false,
                            IconType = "webicon300",
                            SearchFilter = videoFilterOut[i],
                            MovieInMainList = true,
                            IsPartsMoreOne = true
                        };

                        listOfVideoLinksViewModel.Add(videoLinksViewModel);

                        listOfListMoviesFileModel.Add(movies);
                    }
                }
            }

            #endregion

            #region DetailsPageViewModel

            return View(new DetailsPageViewModel
            {
                PageItem = pageItem,
                IconItem = iconItem,
                LinksToPagesByGuid = linksToPagesByGuid,
                LinksToPagesByGuid2 = linksToPagesByGuid2,
                LinksToPagesByFilterOut = linksToPagesByFilterOut,
                LinksToVideosByFilterOut = listOfVideoLinksViewModel,
                ListsMoviesFileModel = listOfListMoviesFileModel,
                LinksFromPagesByGuid = linksFromPagesByGuid,
                LinksFromPagesByGuid2 = linksFromPagesByGuid2,
                LinksFromPagesByPageFilter = linksFromPagesByPageFilter,
                DictionaryOfOutPages = dictionaryOfOutPages
            });

            #endregion
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion

    #region Добавить страницу сайта в базу данных

    [HttpGet]
    public ViewResult AddPage()
    {
        AddPageViewModel newPage = new();

        return View(newPage);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPage(AddPageViewModel addPage)
    {
        if (ModelState.IsValid)
        {
            #region Добавить картинку для страницы

            if (addPage.ImageFileFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == addPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == addPage.ImageFileFormFile.FileName);

                    addPage.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImageFileFormFile", $"Добавьте картинку «{addPage.ImageFileFormFile.FileName}» в базу данных");

                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("ImageFileFormFile", $"Выберите картинку");

                return View();
            }

            #endregion

            #region Текст карточки страницы

            addPage.PageCardText = addPage.PageCardText.Trim().ToUpper();

            #endregion

            #region Добавить фон для страницы

            if (addPage.BackgroundFormFile != null)
            {
                if (await backgroundContext.BackgroundFiles.Where(bk => bk.LeftBackground == addPage.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.LeftBackground == addPage.BackgroundFormFile.FileName);

                    addPage.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else if (await backgroundContext.BackgroundFiles.Where(bk => bk.RightBackground == addPage.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.RightBackground == addPage.BackgroundFormFile.FileName);

                    addPage.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else if (await backgroundContext.BackgroundFiles.Where(bk => bk.WebLeftBackground == addPage.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebLeftBackground == addPage.BackgroundFormFile.FileName);

                    addPage.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else if (await backgroundContext.BackgroundFiles.Where(bk => bk.WebRightBackground == addPage.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebRightBackground == addPage.BackgroundFormFile.FileName);

                    addPage.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebRightBackground == "FotoPlenka.webp");

                    addPage.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
            }
            else
            {
                var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebRightBackground == "FotoPlenka-right.webp");

                addPage.BackgroundFileModelId = newBackground.BackgroundFileModelId;
            }

            #endregion

            #region Заголовок страницы (тег <head>)

            addPage.PageTitle = addPage.PageTitle.Trim();
            addPage.PageDescription = addPage.PageDescription.Trim();
            addPage.PageKeyWords = addPage.PageKeyWords.Trim();

            if (addPage.OgType == "website")
            {
                addPage.PageIconPath = "main/";
                addPage.BrowserConfig = "main.xml";
                addPage.BrowserConfigFolder = "/main";
                addPage.Manifest = "main.json";
            }
            else if (addPage.OgType == "movie")
            {
                addPage.PageIconPath = "movie/";
                addPage.BrowserConfig = "movie.xml";
                addPage.BrowserConfigFolder = "/movie";
                addPage.Manifest = "movie.json";
            }
            else
            {
                addPage.PageIconPath = "main/";
                addPage.BrowserConfig = "main.xml";
                addPage.BrowserConfigFolder = "/main";
                addPage.Manifest = "main.json";
            }

            if (addPage.PageArea == "admin")
            {
                addPage.PageIconPath = "admin/";
                addPage.BrowserConfig = "admin.xml";
                addPage.BrowserConfigFolder = "/admin";
                addPage.Manifest = "admin.json";
            }

            #endregion

            #region Оформление заголовка страницы

            addPage.PageHeading = addPage.PageHeading.Trim();

            #endregion

            #region Текст страницы

            addPage.TextOfPage = addPage.TextOfPage;

            #endregion

            #region Добавить картинку для  заголовка страницы

            if (addPage.ImagePageHeadingFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == addPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == addPage.ImagePageHeadingFormFile.FileName);

                    addPage.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImagePageHeadingFormFile", $"Добавьте картинку «{addPage.ImagePageHeadingFormFile.FileName}» в базу данных");

                    return View();
                }
            }
            else
            {
                addPage.ImagePageHeading = null;
            }

            #endregion

            #region MVC или RazorPage

            #region Область

            if (string.IsNullOrEmpty(addPage.PageArea.Trim()) || addPage.PageArea == "Root")
            {
                addPage.PageArea = string.Empty;
            }
            else
            {
                addPage.PageArea = "/" + addPage.PageArea.Trim().Trim('/').ToLower();
            }

            #endregion

            #region MVC или RazorPage

            addPage.PageAsRazorPage = addPage.PageAsRazorPage;

            #endregion

            #region Контроллер

            if (addPage.PageAsRazorPage)
            {
                addPage.Controller = string.Empty;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(addPage.Controller) || string.IsNullOrEmpty(addPage.Controller))
                {
                    ModelState.AddModelError("PageItem.Controller", "Введите название контроллера");

                    return View();
                }
                else
                {
                    addPage.Controller = "/" + addPage.Controller.Trim().Trim('/').ToLower();
                }
            }

            #endregion

            #region Действие

            if (addPage.PageAsRazorPage)
            {
                addPage.Action = string.Empty;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(addPage.Action) || string.IsNullOrEmpty(addPage.Action))
                {
                    ModelState.AddModelError("PageItem.Action", "Введите название метода");

                    return View();
                }
                else
                {
                    addPage.Action = "/" + addPage.Action.Trim().Trim('/').ToLower();
                }
            }

            #endregion

            #region Адрес без Области (для RazorPage)

            if (addPage.PageAsRazorPage)
            {
                if (string.IsNullOrWhiteSpace(addPage.PageLoc) || string.IsNullOrEmpty(addPage.PageLoc))
                {
                    ModelState.AddModelError("PageItem.PageLoc", "Введите адрес страницы без области");

                    return View();
                }
                else if (addPage.PageLoc == "/")
                {
                    addPage.PageLoc = "/";
                }
                else
                {
                    addPage.PageLoc = "/" + addPage.PageLoc.Trim().Trim('/').TrimStart('?').ToLower();
                }
            }
            else
            {
                addPage.PageLoc = addPage.Action;
            }

            #endregion

            #region Данные (RoutData)

            if (string.IsNullOrWhiteSpace(addPage.RoutData) || string.IsNullOrEmpty(addPage.RoutData))
            {
                addPage.RoutData = string.Empty;
            }
            else
            {
                addPage.RoutData = "?" + addPage.RoutData.Trim().Trim('/').TrimStart('?').ToLower();
            }

            #endregion

            #region Псевдоним страницы

            if (string.IsNullOrWhiteSpace(addPage.PagePathNickName) || string.IsNullOrEmpty(addPage.PagePathNickName))
            {
                addPage.PagePathNickName = string.Empty;
            }
            else if (addPage.PagePathNickName == "/")
            {
                addPage.PagePathNickName = "/";
            }
            else
            {
                addPage.PagePathNickName = "/" + addPage.PagePathNickName.Trim().Trim('/').ToLower();
            }

            #endregion

            #region Проверка существующих страниц

            string checkPageFullPathWithData = string.Empty;

            if (addPage.PageAsRazorPage)
            {
                checkPageFullPathWithData = addPage.PageArea + addPage.PageLoc + addPage.RoutData;

                if (await pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == checkPageFullPathWithData).AnyAsync())
                {
                    ModelState.AddModelError("pageItem.PageLoc", $"Страница «{addPage.PageArea + addPage.PageLoc + addPage.RoutData}» уже существует");

                    return View();
                }
            }
            else
            {
                checkPageFullPathWithData = addPage.PageArea + addPage.Controller + addPage.Action + addPage.RoutData;

                if (await pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == checkPageFullPathWithData).AnyAsync())
                {
                    ModelState.AddModelError("pageItem.PageLoc", $"Страница «{addPage.PageArea + addPage.Controller + addPage.Action + addPage.RoutData}» уже существует");

                    return View();
                }
            }

            if (!string.IsNullOrEmpty(addPage.PagePathNickName) && await pageInfoContext.PagesInfo.Where(p => p.PagePathNickName == addPage.PagePathNickName).AnyAsync())
            {
                ModelState.AddModelError("pageItem.PagePathNickName", $"Страница с псевдонимом «{addPage.PagePathNickName}» уже существует");

                return View();
            }

            #endregion

            #endregion

            #region Фильтр поиска текущей страницы

            addPage.PageFilter = addPage.PageFilter.Trim();

            #endregion

            #region Данные для Sitemap

            addPage.PageLastmod = DateTime.Now;
            addPage.Changefreq = addPage.Changefreq.Trim();
            addPage.Priority = addPage.Priority.Trim();
            addPage.OgType = addPage.OgType.Trim();

            #endregion

            #region Группы связанных ссылок

            // связанные видео
            addPage.VideoLinks = addPage.VideoLinks;
            addPage.VideoFilterOut = addPage.VideoFilterOut.Trim();

            // связанные страницы по фильтрам
            addPage.PageLinksByFilters = addPage.PageLinksByFilters;
            addPage.PageFilterOut = addPage.PageFilterOut.Trim();

            // связанные страницы по GUID (1)
            addPage.PageLinks = addPage.PageLinks;
            addPage.RefPages = addPage.RefPages.ToLower().Trim();

            // связанные страницы по GUID (2)
            addPage.PageLinks2 = addPage.PageLinks2;
            addPage.RefPages2 = addPage.RefPages2.ToLower().Trim();

            #endregion

            #region Сохранить в базе данных

            await pageInfoContext.AddNewPageAsync(addPage);

            #endregion

            #region Открытие страницы DetailsPage

            PageInfoModel newPage = await pageInfoContext.PagesInfo.FirstAsync(p => p.PageFullPathWithData == checkPageFullPathWithData);

            return RedirectToAction("DetailsPage", new { pageId = newPage.PageInfoModelId, Area = "Admin" });

            #endregion
        }
        else
        {
            return View();
        }
    }

    #endregion

    #region Изменить страницу в базе данных

    [HttpGet]
    public async Task<IActionResult> EditPage(Guid? pageId)
    {
        EditPageViewModel editPage = new();

        if (pageId.HasValue)
        {
            if (await pageInfoContext.PagesInfo.Where(i => i.PageInfoModelId == pageId).AnyAsync())
            {
                editPage.PageItem = await pageInfoContext.PagesInfo.FirstAsync(i => i.PageInfoModelId == pageId);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            editPage.ImageFileFormFile = null;

            if (await iconContext.IconFiles.Where(icon => icon.IconPath == editPage.PageItem.PageIconPath && icon.IconFileName == DataConfig.IconItem).AnyAsync())
            {
                editPage.IconItem = await iconContext.IconFiles.FirstAsync(icon => icon.IconPath == editPage.PageItem.PageIconPath && icon.IconFileName == DataConfig.IconItem);
            }
            else
            {
                editPage.IconItem = await iconContext.IconFiles.FirstAsync(icon => icon.IconPath == "main/" && icon.IconFileName == DataConfig.IconItem);
            }

            #region Ссылки на текущую страницу по GUID (1)

            List<PageInfoModel> linksFromPagesByGuid = [];

#pragma warning disable CA1862
            linksFromPagesByGuid.AddRange(await pageInfoContext.PagesInfo.Where(p => p.RefPages.ToLower().Contains(editPage.PageItem.PageInfoModelId.ToString().ToLower())).ToArrayAsync());
#pragma warning restore CA1862

            _ = linksFromPagesByGuid.Distinct().OrderBy(p => p.PageCardText);

            editPage.LinksFromPagesByGuid = linksFromPagesByGuid;

            #endregion

            #region Ссылки на текущую страницу по GUID (2)

            List<PageInfoModel> linksFromPagesByGuid2 = [];

#pragma warning disable CA1862
            linksFromPagesByGuid2.AddRange(await pageInfoContext.PagesInfo.Where(p => p.RefPages2.ToLower().Contains(editPage.PageItem.PageInfoModelId.ToString().ToLower())).ToArrayAsync());
#pragma warning restore CA1862

            _ = linksFromPagesByGuid2.Distinct().OrderBy(p => p.PageCardText);

            editPage.LinksFromPagesByGuid2 = linksFromPagesByGuid2;

            #endregion

            #region Ссылки на текущую страницу по фильтру PageFilter

            string[] pageFilters = editPage.PageItem.PageFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);

            List<PageInfoModel> linksFromPagesByPageFilter = [];

            if (pageFilters.Length > 0)
            {
                for (int i = 0; i < pageFilters.Length; i++)
                {
#pragma warning disable CA1862
                    linksFromPagesByPageFilter.AddRange(await pageInfoContext.PagesInfo.Where(p => p.PageFilterOut.ToLower().Contains(pageFilters[i].ToLower().Trim() + ",")).ToArrayAsync());
#pragma warning restore CA1862
                }
            }

            _ = linksFromPagesByPageFilter.Distinct().OrderBy(p => p.PageCardText);

            editPage.LinksFromPagesByPageFilter = linksFromPagesByPageFilter;

            #endregion

            #region Ссылки на страницы сайта по фильтру (PageFilterOut)

            string[] pageFiltersOut = editPage.PageItem.PageFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

            List<PageInfoModel> linksToPagesByFilterOut = [];

            if (pageFiltersOut.Length > 0)
            {
                for (int i = 0; i < pageFiltersOut.Length; i++)
                {
                    linksToPagesByFilterOut.AddRange(await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFiltersOut[i].Trim() + ",")).ToArrayAsync());
                    _ = linksToPagesByFilterOut.Distinct().OrderBy(p => p.PageCardText);
                }
            }

            editPage.LinksToPagesByFilterOut = linksToPagesByFilterOut;

            #endregion

            #region  Ссылки на видео сайта по фильтру (VideoFilterOut)

            List<VideoLinksViewModel> listsOfVideoFilterOut = [];
            List<List<MovieFileModel>> moviesFileModel = [];

            string[] videoFilterOut = editPage.PageItem.VideoFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (videoFilterOut.Length > 0)
            {
                for (int i = 0; i < videoFilterOut.Length; i++)
                {
#pragma warning disable CA1862
                    if (await movieContext.MovieFiles.Where(p => p.SearchFilter.ToLower().Contains(videoFilterOut[i])).AnyAsync())
                    {
                        var movies = await movieContext.MovieFiles.Where(p => p.SearchFilter.ToLower().Contains(videoFilterOut[i]) & p.MovieInMainList == true).ToListAsync();
                        movies.Sort((movies1, movies2) => movies1.MovieDatePublished.CompareTo(movies2.MovieDatePublished));

                        VideoLinksViewModel videoLinksViewModel = new()
                        {
                            HeadTitleForVideoLinks = videoFilterOut[i],
                            IsImage = false,
                            IconType = "webicon300",
                            SearchFilter = videoFilterOut[i],
                            MovieInMainList = true,
                            IsPartsMoreOne = true
                        };

                        listsOfVideoFilterOut.Add(videoLinksViewModel);

                        moviesFileModel.Add(movies);
                    }
#pragma warning restore CA1862
                }
                _ = listsOfVideoFilterOut.Distinct();
                _ = moviesFileModel.Distinct();
            }

            editPage.LinksToVideosByFilterOut = listsOfVideoFilterOut;
            editPage.ListsMoviesFileModel = moviesFileModel;

            #endregion

            #region Ссылки на страницы сайта по GUID (RefPages)

            string[] pageIdOut = editPage.PageItem.RefPages.Split(',', StringSplitOptions.RemoveEmptyEntries);

            List<PageInfoModel> linksToPagesByGuid = [];

            if (pageIdOut.Length > 0)
            {
                foreach (string refPageId in pageIdOut)
                {
                    if (Guid.TryParse(refPageId, out Guid pageGuid))
                    {
                        if (await pageInfoContext.PagesInfo.Where(p => p.PageInfoModelId == pageGuid).AnyAsync())
                        {
                            linksToPagesByGuid.Add(await pageInfoContext.PagesInfo.FirstAsync(p => p.PageInfoModelId == pageGuid));

                            _ = linksToPagesByGuid.Distinct().OrderBy(p => p.PageCardText);
                        }
                    }
                }
            }

            editPage.LinksToPagesByGuid = linksToPagesByGuid;

            #endregion

            #region Ссылки на страницы сайта по GUID (RefPages2)

            string[] pageIdOut2 = editPage.PageItem.RefPages2.Split(',', StringSplitOptions.RemoveEmptyEntries);

            List<PageInfoModel> linksToPagesByGuid2 = [];

            if (pageIdOut2.Length > 0)
            {
                foreach (string refPageId2 in pageIdOut2)
                {
                    if (Guid.TryParse(refPageId2, out Guid pageGuid2))
                    {
                        if (await pageInfoContext.PagesInfo.Where(p => p.PageInfoModelId == pageGuid2).AnyAsync())
                        {
                            linksToPagesByGuid2.Add(await pageInfoContext.PagesInfo.FirstAsync(p => p.PageInfoModelId == pageGuid2));

                            _ = linksToPagesByGuid2.Distinct().OrderBy(p => p.PageCardText);
                        }
                    }
                }
            }

            editPage.LinksToPagesByGuid2 = linksToPagesByGuid2;

            #endregion

            return View(editPage);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPage(EditPageViewModel editPage)
    {
        if (ModelState.IsValid)
        {
            PageInfoModel pageUpdate = await pageInfoContext.PagesInfo.FirstAsync(i => i.PageInfoModelId == editPage.PageItem.PageInfoModelId);

            IconFileModel editPageiconItem;

            if (await iconContext.IconFiles
                .Where(icon => icon.IconPath == pageUpdate.PageIconPath && icon.IconFileName == DataConfig.IconItem).AnyAsync())
            {
                editPageiconItem = await iconContext.IconFiles
                    .FirstAsync(icon => icon.IconPath == pageUpdate.PageIconPath && icon.IconFileName == DataConfig.IconItem);
            }
            else
            {
                editPageiconItem = await iconContext.IconFiles
                    .FirstAsync(icon => icon.IconPath == "main/" && icon.IconFileName == DataConfig.IconItem);
            }

            #region Изменить картинку для страницы

            if (editPage.ImageFileFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == editPage.ImageFileFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == editPage.ImageFileFormFile.FileName);

                    pageUpdate.ImageFileModelId = newImage.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImageFileFormFile", $"Добавьте картинку «{editPage.ImageFileFormFile.FileName}» в базу данных");

                    return View(nameof(EditPage), new EditPageViewModel
                    {
                        PageItem = pageUpdate,
                        IconItem = editPageiconItem
                    });
                }
            }

            #endregion

            #region Изменить текст карточки страницы

            pageUpdate.PageCardText = editPage.PageItem.PageCardText.Trim().ToUpper();

            #endregion

            #region Изменить фон страницы

            if (editPage.BackgroundFormFile != null)
            {
                if (await backgroundContext.BackgroundFiles.Where(bk => bk.LeftBackground == editPage.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.LeftBackground == editPage.BackgroundFormFile.FileName);

                    pageUpdate.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else if (await backgroundContext.BackgroundFiles.Where(bk => bk.RightBackground == editPage.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.RightBackground == editPage.BackgroundFormFile.FileName);

                    pageUpdate.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else if (await backgroundContext.BackgroundFiles.Where(bk => bk.WebLeftBackground == editPage.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebLeftBackground == editPage.BackgroundFormFile.FileName);

                    pageUpdate.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else if (await backgroundContext.BackgroundFiles.Where(bk => bk.WebRightBackground == editPage.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebRightBackground == editPage.BackgroundFormFile.FileName);

                    pageUpdate.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.RightBackground == "FotoPlenka.png");

                    pageUpdate.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
            }

            #endregion

            #region Изменить заголовок страницы (тег <head>)

            pageUpdate.PageTitle = editPage.PageItem.PageTitle.Trim();
            pageUpdate.PageDescription = editPage.PageItem.PageDescription.Trim();
            pageUpdate.PageKeyWords = editPage.PageItem.PageKeyWords.Trim();

            if (editPage.PageItem.OgType == "website")
            {
                pageUpdate.OgType = "website";
                pageUpdate.PageIconPath = "main/";
                pageUpdate.BrowserConfig = "main.xml";
                pageUpdate.BrowserConfigFolder = "/main";
                pageUpdate.Manifest = "main.json";
            }
            else if (editPage.PageItem.OgType == "movie")
            {
                pageUpdate.OgType = "movie";
                pageUpdate.PageIconPath = "movie/";
                pageUpdate.BrowserConfig = "movie.xml";
                pageUpdate.BrowserConfigFolder = "/movie";
                pageUpdate.Manifest = "movie.json";
            }
            else
            {
                pageUpdate.PageIconPath = "main/";
                pageUpdate.BrowserConfig = "main.xml";
                pageUpdate.BrowserConfigFolder = "/main";
                pageUpdate.Manifest = "main.json";
            }

            if (pageUpdate.PageArea == "/admin")
            {
                pageUpdate.PageIconPath = "admin/";
                pageUpdate.BrowserConfig = "admin.xml";
                pageUpdate.BrowserConfigFolder = "/admin";
                pageUpdate.Manifest = "admin.json";
            }

            #endregion

            #region Изменить оформление заголовка страницы

            pageUpdate.PageHeading = editPage.PageItem.PageHeading.Trim();

            #endregion

            #region Изменить текст страницы

            pageUpdate.TextOfPage = editPage.PageItem.TextOfPage;

            #endregion

            #region Изменить картинку для  заголовка страницы

            if (editPage.ImagePageHeadingFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == editPage.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var newImage = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == editPage.ImagePageHeadingFormFile.FileName);

                    pageUpdate.ImagePageHeadingId = newImage.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImagePageHeadingFormFile", $"Добавьте картинку «{editPage.ImagePageHeadingFormFile.FileName}» в базу данных");

                    return View(nameof(EditPage), new EditPageViewModel
                    {
                        PageItem = pageUpdate,
                        IconItem = editPageiconItem
                    });
                }
            }

            #endregion

            #region Изменить адрес страницы (MVC или RazorPage)

            pageUpdate.PageAsRazorPage = editPage.PageItem.PageAsRazorPage;

            if (string.IsNullOrEmpty(editPage.PageItem.PageArea.Trim()) || editPage.PageItem.PageArea == "Root")
            {
                pageUpdate.PageArea = string.Empty;
            }
            else
            {
                pageUpdate.PageArea = "/" + editPage.PageItem.PageArea.Trim().Trim('/').ToLower();
            }

            if (string.IsNullOrWhiteSpace(editPage.PageItem.Controller) || string.IsNullOrEmpty(editPage.PageItem.Controller))
            {
                pageUpdate.Controller = string.Empty;
            }
            else
            {
                pageUpdate.Controller = "/" + editPage.PageItem.Controller.Trim().Trim('/').ToLower();
            }

            if (string.IsNullOrWhiteSpace(editPage.PageItem.Action) || string.IsNullOrEmpty(editPage.PageItem.Action))
            {
                pageUpdate.Action = string.Empty;
            }
            else
            {
                pageUpdate.Action = "/" + editPage.PageItem.Action.Trim().Trim('/').ToLower();
            }

            if (string.IsNullOrWhiteSpace(editPage.PageItem.RoutData) || string.IsNullOrEmpty(editPage.PageItem.RoutData))
            {
                pageUpdate.RoutData = string.Empty;
            }
            else
            {
                pageUpdate.RoutData = "?" + editPage.PageItem.RoutData.Trim().Trim('/').TrimStart('?').ToLower();
            }

            if (string.IsNullOrWhiteSpace(editPage.PageItem.PageLoc) || string.IsNullOrEmpty(editPage.PageItem.PageLoc))
            {
                pageUpdate.PageLoc = string.Empty;
            }
            else if (editPage.PageItem.PageLoc == "/")
            {
                pageUpdate.PageLoc = "/";
            }
            else
            {
                pageUpdate.PageLoc = "/" + editPage.PageItem.PageLoc.Trim().Trim('/').ToLower();
            }

            if (string.IsNullOrWhiteSpace(editPage.PageItem.PagePathNickName) || string.IsNullOrEmpty(editPage.PageItem.PagePathNickName))
            {
                pageUpdate.PagePathNickName = string.Empty;
            }
            else if (editPage.PageItem.PagePathNickName == "/")
            {
                pageUpdate.PagePathNickName = "/";
            }
            else
            {
                pageUpdate.PagePathNickName = "/" + editPage.PageItem.PagePathNickName.Trim().Trim('/').ToLower();
            }

            #endregion

            #region Изменить фильтр поиска текущей страницы

            pageUpdate.PageFilter = editPage.PageItem.PageFilter.Trim();

            #endregion

            #region Изменить данные для Sitemap

            pageUpdate.PageLastmod = DateTime.Now;
            pageUpdate.Changefreq = editPage.PageItem.Changefreq.Trim();
            pageUpdate.Priority = editPage.PageItem.Priority.Trim();
            pageUpdate.OgType = editPage.PageItem.OgType.Trim();

            #endregion

            #region Изменить группы связанных ссылок

            // поиск связанных страниц поGUID (1)
            pageUpdate.PageLinks = editPage.PageItem.PageLinks;
            pageUpdate.RefPages = editPage.PageItem.RefPages.Trim().ToLower();

            // поиск связанных страниц поGUID (2)
            pageUpdate.PageLinks2 = editPage.PageItem.PageLinks2;
            pageUpdate.RefPages2 = editPage.PageItem.RefPages2.Trim().ToLower();

            // поиск связанных страниц по фильтру
            pageUpdate.PageLinksByFilters = editPage.PageItem.PageLinksByFilters;
            pageUpdate.PageFilterOut = editPage.PageItem.PageFilterOut.Trim();

            // поиск связанных видео
            pageUpdate.VideoLinks = editPage.PageItem.VideoLinks;
            pageUpdate.VideoFilterOut = editPage.PageItem.VideoFilterOut.Trim();

            #endregion

            await pageInfoContext.SaveChangesInPageAsync();

            return RedirectToAction("DetailsPage", new { pageId = pageUpdate.PageInfoModelId, Area = "Admin" });
        }
        else
        {
            return View();
        }
    }

    #endregion

    #region Удалить страницу из базы данных

    public async Task<IActionResult> DeletePage(Guid? pageId)
    {
        PageInfoModel deletePage = new();

        if (pageId.HasValue)
        {
            if (await pageInfoContext.PagesInfo.Where(i => i.PageInfoModelId == pageId).AnyAsync())
            {
                deletePage = await pageInfoContext.PagesInfo.FirstAsync(i => i.PageInfoModelId == pageId);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(deletePage);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePage(PageInfoModel deletePage)
    {
        if (deletePage != null)
        {
            await pageInfoContext.DeletePageAsync(deletePage.PageInfoModelId);

            return RedirectToAction(nameof(Index));
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion
}