using NuGet.Configuration;

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
            PageInfoModel pageItem;
            IconFileModel iconItem;

            #region Инициализация pageItem и  iconItem

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
            linksFromPagesByGuid.AddRange(await pageInfoContext.PagesInfo.Where(p => p.RefPages.ToLower().Contains(pageItem.PageInfoModelId.ToString().ToLower())).ToArrayAsync());
#pragma warning restore CA1862

            _ = linksFromPagesByGuid.Distinct().OrderBy(p => p.PageCardText);

            #endregion

            #region Ссылки на текущую страницу по GUID (2)

            List<PageInfoModel> linksFromPagesByGuid2 = [];

#pragma warning disable CA1862
            linksFromPagesByGuid2.AddRange(await pageInfoContext.PagesInfo.Where(p => p.RefPages2.ToLower().Contains(pageItem.PageInfoModelId.ToString().ToLower())).ToArrayAsync());
#pragma warning restore CA1862

            _ = linksFromPagesByGuid2.Distinct().OrderBy(p => p.PageCardText);

            #endregion

            #region Ссылки на текущую страницу по фильтру PageFilter

            string[] pageFilters = pageItem.PageFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);

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
                LinksFromPagesByPageFilter = linksFromPagesByPageFilter
            });
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
        EditPageViewModel newText = new();

        return View(newText);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPage(EditPageViewModel addItem)
    {
        if (ModelState.IsValid)
        {
            #region Добавить картинку для страницы

            if (addItem.ImageFileFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == addItem.ImageFileFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == addItem.ImageFileFormFile.FileName);

                    addItem.PageItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImageFileFormFile", $"Добавьте картинку «{addItem.ImageFileFormFile.FileName}» в базу данных");

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

            addItem.PageItem.PageCardText = addItem.PageItem.PageCardText.Trim().ToUpper();

            #endregion

            #region Добавить фон для страницы

            if (addItem.BackgroundFormFile != null)
            {
                if (await backgroundContext.BackgroundFiles.Where(bk => bk.LeftBackground == addItem.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.LeftBackground == addItem.BackgroundFormFile.FileName);

                    addItem.PageItem.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else if (await backgroundContext.BackgroundFiles.Where(bk => bk.RightBackground == addItem.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.RightBackground == addItem.BackgroundFormFile.FileName);

                    addItem.PageItem.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else if (await backgroundContext.BackgroundFiles.Where(bk => bk.WebLeftBackground == addItem.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebLeftBackground == addItem.BackgroundFormFile.FileName);

                    addItem.PageItem.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else if (await backgroundContext.BackgroundFiles.Where(bk => bk.WebRightBackground == addItem.BackgroundFormFile.FileName).AnyAsync())
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebRightBackground == addItem.BackgroundFormFile.FileName);

                    addItem.PageItem.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
                else
                {
                    var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebRightBackground == "FotoPlenka.webp");

                    addItem.PageItem.BackgroundFileModelId = newBackground.BackgroundFileModelId;
                }
            }
            else
            {
                var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebRightBackground == "FotoPlenka-right.webp");

                addItem.PageItem.BackgroundFileModelId = newBackground.BackgroundFileModelId;
            }

            #endregion

            #region Заголовок страницы (тег <head>)

            addItem.PageItem.PageTitle = addItem.PageItem.PageTitle.Trim();
            addItem.PageItem.PageDescription = addItem.PageItem.PageDescription.Trim();
            addItem.PageItem.PageKeyWords = addItem.PageItem.PageKeyWords.Trim();

            if (addItem.PageItem.OgType == "website")
            {
                addItem.PageItem.PageIconPath = "main/";
                addItem.PageItem.BrowserConfig = "main.xml";
                addItem.PageItem.BrowserConfigFolder = "/main";
                addItem.PageItem.Manifest = "main.json";
            }
            else if (addItem.PageItem.OgType == "movie")
            {
                addItem.PageItem.PageIconPath = "movie/";
                addItem.PageItem.BrowserConfig = "movie.xml";
                addItem.PageItem.BrowserConfigFolder = "/movie";
                addItem.PageItem.Manifest = "movie.json";
            }
            else
            {
                addItem.PageItem.PageIconPath = "main/";
                addItem.PageItem.BrowserConfig = "main.xml";
                addItem.PageItem.BrowserConfigFolder = "/main";
                addItem.PageItem.Manifest = "main.json";
            }

            if (addItem.PageItem.PageArea == "admin")
            {
                addItem.PageItem.PageIconPath = "admin/";
                addItem.PageItem.BrowserConfig = "admin.xml";
                addItem.PageItem.BrowserConfigFolder = "/admin";
                addItem.PageItem.Manifest = "admin.json";
            }

            #endregion

            #region Оформление заголовка страницы

            addItem.PageItem.PageHeading = addItem.PageItem.PageHeading.Trim();

            #endregion

            #region Текст страницы

            addItem.PageItem.TextOfPage = addItem.PageItem.TextOfPage;

            #endregion

            #region Добавить картинку для  заголовка страницы

            if (addItem.ImagePageHeadingFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == addItem.ImagePageHeadingFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == addItem.ImagePageHeadingFormFile.FileName);

                    addItem.PageItem.ImagePageHeadingId = imageFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImagePageHeadingFormFile", $"Добавьте картинку «{addItem.ImagePageHeadingFormFile.FileName}» в базу данных");

                    return View();
                }
            }
            else
            {
                addItem.PageItem.ImagePageHeading = null;
            }

            #endregion

            #region MVC или RazorPage

            #region Область

            if (string.IsNullOrWhiteSpace(addItem.PageItem.PageArea) || string.IsNullOrEmpty(addItem.PageItem.PageArea))
            {
                addItem.PageItem.PageArea = string.Empty;
            }
            else
            {
                addItem.PageItem.PageArea = "/" + addItem.PageItem.PageArea.Trim().Trim('/').ToLower();
            }

            #endregion

            #region MVC или RazorPage

            addItem.PageItem.PageAsRazorPage = addItem.PageItem.PageAsRazorPage;

            #endregion

            #region Контроллер

            if (addItem.PageItem.PageAsRazorPage)
            {
                addItem.PageItem.Controller = string.Empty;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(addItem.PageItem.Controller) || string.IsNullOrEmpty(addItem.PageItem.Controller))
                {
                    ModelState.AddModelError("PageItem.Controller", "Введите название контроллера");

                    return View();
                }
                else
                {
                    addItem.PageItem.Controller = "/" + addItem.PageItem.Controller.Trim().Trim('/').ToLower();
                }
            }

            #endregion

            #region Действие

            if (addItem.PageItem.PageAsRazorPage)
            {
                addItem.PageItem.Action = string.Empty;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(addItem.PageItem.Action) || string.IsNullOrEmpty(addItem.PageItem.Action))
                {
                    ModelState.AddModelError("PageItem.Action", "Введите название метода");

                    return View();
                }
                else
                {
                    addItem.PageItem.Action = "/" + addItem.PageItem.Action.Trim().Trim('/').ToLower();
                }
            }

            #endregion

            #region Адрес без Области (для RazorPage)

            if (addItem.PageItem.PageAsRazorPage)
            {
                if (string.IsNullOrWhiteSpace(addItem.PageItem.PageLoc) || string.IsNullOrEmpty(addItem.PageItem.PageLoc))
                {
                    ModelState.AddModelError("PageItem.PageLoc", "Введите адрес страницы без области");

                    return View();
                }
                else if (addItem.PageItem.PageLoc == "/")
                {
                    addItem.PageItem.PageLoc = "/";
                }
                else
                {
                    addItem.PageItem.PageLoc = "/" + addItem.PageItem.PageLoc.Trim().Trim('/').TrimStart('?').ToLower();
                }
            }
            else
            {
                addItem.PageItem.PageLoc = addItem.PageItem.Action;
            }

            #endregion

            #region Данные (RoutData)

            if (string.IsNullOrWhiteSpace(addItem.PageItem.RoutData) || string.IsNullOrEmpty(addItem.PageItem.RoutData))
            {
                addItem.PageItem.RoutData = string.Empty;
            }
            else
            {
                addItem.PageItem.RoutData = "?" + addItem.PageItem.RoutData.Trim().Trim('/').TrimStart('?').ToLower();
            }

            #endregion

            #region Псевдоним страницы

            if (string.IsNullOrWhiteSpace(addItem.PageItem.PagePathNickName) || string.IsNullOrEmpty(addItem.PageItem.PagePathNickName))
            {
                addItem.PageItem.PagePathNickName = string.Empty;
            }
            else if (addItem.PageItem.PagePathNickName == "/")
            {
                addItem.PageItem.PagePathNickName = "/";
            }
            else
            {
                addItem.PageItem.PagePathNickName = "/" + addItem.PageItem.PagePathNickName.Trim().Trim('/').ToLower();
            }

            #endregion

            #region Проверка существующих страниц

            string checkPageFullPathWithData = string.Empty;

            if (addItem.PageItem.PageAsRazorPage)
            {
                checkPageFullPathWithData = addItem.PageItem.PageArea + addItem.PageItem.PageLoc + addItem.PageItem.RoutData;

                if (await pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == checkPageFullPathWithData).AnyAsync())
                {
                    ModelState.AddModelError("pageItem.PageLoc", $"Страница «{addItem.PageItem.PageArea + addItem.PageItem.PageLoc + addItem.PageItem.RoutData}» уже существует");

                    return View();
                }
            }
            else
            {
                checkPageFullPathWithData = addItem.PageItem.PageArea + addItem.PageItem.Controller + addItem.PageItem.Action + addItem.PageItem.RoutData;

                if (await pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == checkPageFullPathWithData).AnyAsync())
                {
                    ModelState.AddModelError("pageItem.PageLoc", $"Страница «{addItem.PageItem.PageArea + addItem.PageItem.Controller + addItem.PageItem.Action + addItem.PageItem.RoutData}» уже существует");

                    return View();
                }
            }

            if (!string.IsNullOrEmpty(addItem.PageItem.PagePathNickName) && await pageInfoContext.PagesInfo.Where(p => p.PagePathNickName == addItem.PageItem.PagePathNickName).AnyAsync())
            {
                ModelState.AddModelError("pageItem.PagePathNickName", $"Страница с псевдонимом «{addItem.PageItem.PagePathNickName}» уже существует");

                return View();
            }

            #endregion

            #endregion

            #region Фильтр поиска текущей страницы

            addItem.PageItem.PageFilter = addItem.PageItem.PageFilter.Trim();

            #endregion

            #region Данные для Sitemap

            addItem.PageItem.PageLastmod = DateTime.Now;
            addItem.PageItem.Changefreq = addItem.PageItem.Changefreq.Trim();
            addItem.PageItem.Priority = addItem.PageItem.Priority.Trim();
            addItem.PageItem.OgType = addItem.PageItem.OgType.Trim();

            #endregion

            #region Группы связанных ссылок

            // связанные видео
            addItem.PageItem.VideoLinks = addItem.PageItem.VideoLinks;
            addItem.PageItem.VideoFilterOut = addItem.PageItem.VideoFilterOut.Trim();

            // связанные страницы по фильтрам
            addItem.PageItem.PageLinksByFilters = addItem.PageItem.PageLinksByFilters;
            addItem.PageItem.PageFilterOut = addItem.PageItem.PageFilterOut.Trim();

            // связанные страницы по GUID (1)
            addItem.PageItem.PageLinks = addItem.PageItem.PageLinks;
            addItem.PageItem.RefPages = addItem.PageItem.RefPages.ToLower().Trim();

            // связанные страницы по GUID (2)
            addItem.PageItem.PageLinks2 = addItem.PageItem.PageLinks2;
            addItem.PageItem.RefPages2 = addItem.PageItem.RefPages2.ToLower().Trim();

            #endregion

            #region Сохранить в базе данных

            await pageInfoContext.AddNewPageAsync(addItem.PageItem);

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

            if (string.IsNullOrEmpty(editPage.PageItem.PageArea))
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

    #region Секция для тестов

    //public async Task<IActionResult> ChangeIconPath()
    //{
    //    foreach (var item in pageInfoContext.PagesInfo)
    //    {

    //    }
    //}

    #endregion
}