﻿namespace ShevkunenkoSite.Areas.Admin.Controllers;

// Имена методов не начинать со слова Page
[Area("Admin")]
[Authorize]
public class PageInfoController(
    IPageInfoRepository pageInfoContext,
    IMovieFileRepository movieContext,
    IIconFileRepository iconContext,
    IImageFileRepository imageContext,
    IBackgroundFotoRepository backgroundContext,
    IBooksAndArticlesRepository bookAndArticleContext
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

            #region Ссылки на страницы сайта по GUID1

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

            #region Ссылки на страницы сайта по фильтрам (PageFilterOut)

            Dictionary<string, List<PageInfoModel>> dictionaryOfLinksByPageFilterOut = [];

            string[] pageFiltersOut = pageItem.PageFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (pageFiltersOut.Length > 0)
            {
                for (int i = 0; i < pageFiltersOut.Length; i++)
                {
                    if (await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFiltersOut[i])).AnyAsync())
                    {
                        var pages = await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFiltersOut[i])).ToListAsync();

                        _ = pages.OrderBy(p => p.SortOfPage);

                        dictionaryOfLinksByPageFilterOut[pageFiltersOut[i]] = pages;
                    }
                }
            }

            #endregion

            #region  Ссылки на видео сайта по текстовому фильтру (VideoFilterOut)

            Dictionary<string, VideoLinksViewModel> dictionaryOfLinksByVideoFilterOut = [];

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

                        dictionaryOfLinksByVideoFilterOut[videoFilterOut[i]] = videoLinksViewModel;
                    }
                }
            }

            #endregion

            #region Группы картинок по фильтрам (PhotoFilterOut)

            Dictionary<string, ImageListViewModel> dictionaryOfLinksByFotoFilterOut = [];

            string[] pictureFiltersOut = pageItem.PhotoFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (pictureFiltersOut.Length > 0)
            {
                for (int i = 0; i < pictureFiltersOut.Length; i++)
                {
                    if (await imageContext.ImageFiles.Where(img => img.SearchFilter.Contains(pictureFiltersOut[i])).AnyAsync())
                    {
                        var pictures = await imageContext.ImageFiles.Where(img => img.SearchFilter.Contains(pictureFiltersOut[i])).ToListAsync();

                        _ = pictures.OrderBy(p => p.ImageCaption);

                        dictionaryOfLinksByFotoFilterOut[pictureFiltersOut[i]] = new();

                        // TODO Сформировать ImageListViewModel для передачи в DetailsPage
                    }
                }
            }

            #endregion

            #region Кадры слева и справа от текста

            FramesAroundMainContentModel framesAroundMainContent = new();

            if (pageItem.OgType == "book")
            {
                if (pageItem.RoutData.Contains("bookid", StringComparison.CurrentCultureIgnoreCase))
                {
                    string[] routData = pageItem.RoutData[1..].Split('&');

                    var dictionaryOfRoutdata = routData.Select(part => part.Split('=')).ToDictionary(split => split[0], split => split[1]);

                    if (Guid.TryParse(dictionaryOfRoutdata["bookid"], out var bookGuid))
                    {
                        if (await bookAndArticleContext.BooksAndArticles.Where(book => book.BooksAndArticlesModelId == bookGuid).AnyAsync())
                        {
                            var bookForPage = await bookAndArticleContext.BooksAndArticles.FirstAsync(book => book.BooksAndArticlesModelId == bookGuid);

                            var imageItems = await imageContext.ImageFiles
                                                            .Where(img => img.SearchFilter.ToLower().Contains(bookForPage.CaptionOfText.ToLower()))
                                                            .ToArrayAsync();

                            imageItems = [.. imageItems.Shuffle()];

                            if (imageItems.Length > 1)
                            {
                                framesAroundMainContent.FramesOnTheLeft = [.. imageItems.Take(imageItems.Length / 2)];
                                framesAroundMainContent.FramesOnTheRight = [.. imageItems.Skip(imageItems.Length / 2)];
                            }
                        }
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
                LinksFromPagesByGuid = linksFromPagesByGuid,
                LinksFromPagesByGuid2 = linksFromPagesByGuid2,
                DictionaryOfOutPages = dictionaryOfOutPages,
                FramesAroundMainContent = framesAroundMainContent,
                DictionaryOfLinksByVideoFilterOut = dictionaryOfLinksByVideoFilterOut,
                DictionaryOfLinksByPageFilterOut = dictionaryOfLinksByPageFilterOut,
                DictionaryOfLinksByFotoFilterOut = dictionaryOfLinksByFotoFilterOut
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

            #region Индекс сортировки страницы

            addPage.SortOfPage = addPage.SortOfPage;

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

            // альбом картинок
            addPage.PhotoLinks = addPage.PhotoLinks;
            addPage.PhotoFilterOut = addPage.PhotoFilterOut.Trim();

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
            #region Инициализация экземпляра страницы

            if (await pageInfoContext.PagesInfo.Where(i => i.PageInfoModelId == pageId).AnyAsync())
            {
                editPage.PageItem = await pageInfoContext.PagesInfo.FirstAsync(i => i.PageInfoModelId == pageId);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            #endregion

            #region Инициализация иконки страницы

            if (await iconContext.IconFiles.Where(icon => icon.IconPath == editPage.PageItem.PageIconPath && icon.IconFileName == DataConfig.IconItem).AnyAsync())
            {
                editPage.IconItem = await iconContext.IconFiles.FirstAsync(icon => icon.IconPath == editPage.PageItem.PageIconPath && icon.IconFileName == DataConfig.IconItem);
            }
            else
            {
                editPage.IconItem = await iconContext.IconFiles.FirstAsync(icon => icon.IconPath == "main/" && icon.IconFileName == DataConfig.IconItem);
            }

            #endregion

            #region Ссылки на текущую страницу по GUID (1)

            List<PageInfoModel> linksFromPagesByGuid = [];

#pragma warning disable CA1862
            linksFromPagesByGuid
                .AddRange(await pageInfoContext.PagesInfo
                    .Where(p => p.RefPages.ToLower().Contains(editPage.PageItem.PageInfoModelId.ToString().ToLower()))
                    .ToArrayAsync());
#pragma warning restore CA1862

            _ = linksFromPagesByGuid.Distinct().OrderBy(p => p.SortOfPage);

            #endregion

            #region Ссылки на текущую страницу по GUID (2)

            List<PageInfoModel> linksFromPagesByGuid2 = [];

#pragma warning disable CA1862
            linksFromPagesByGuid2
                .AddRange(await pageInfoContext.PagesInfo
                    .Where(p => p.RefPages2.ToLower().Contains(editPage.PageItem.PageInfoModelId.ToString().ToLower()))
                .ToArrayAsync());
#pragma warning restore CA1862

            _ = linksFromPagesByGuid2.Distinct().OrderBy(p => p.SortOfPage);

            #endregion

            #region Ссылки на текущую страницу по фильтру PageFilter

            string[] pageFilters = editPage.PageItem.PageFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);

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

                        editPage.DictionaryOfOutPages = dictionaryOfOutPages;
                    }
                }
            }

            #endregion

            #region Ссылки на страницы сайта по GUID1

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

                            editPage.LinksToPagesByGuid = linksToPagesByGuid;
                        }
                    }
                }
            }

            #endregion

            #region Ссылки на страницы сайта по GUID2

            string[] pageIdOut2 = editPage.PageItem.RefPages2.Split(',', StringSplitOptions.RemoveEmptyEntries);

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

                            editPage.LinksToPagesByGuid2 = linksToPagesByGuid2;
                        }
                    }
                }
            }

            #endregion

            #region Ссылки на страницы сайта по фильтрам (PageFilterOut)

            Dictionary<string, List<PageInfoModel>> dictionaryOfLinksByPageFilterOut = [];

            string[] pageFiltersOut = editPage.PageItem.PageFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (pageFiltersOut.Length > 0)
            {
                for (int i = 0; i < pageFiltersOut.Length; i++)
                {
                    if (await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFiltersOut[i])).AnyAsync())
                    {
                        var pages = await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFiltersOut[i])).ToListAsync();

                        _ = pages.OrderBy(p => p.SortOfPage);

                        dictionaryOfLinksByPageFilterOut[pageFiltersOut[i]] = pages;

                        editPage.DictionaryOfLinksByPageFilterOut = dictionaryOfLinksByPageFilterOut;
                    }
                }
            }

            #endregion

            #region  Ссылки на видео сайта по текстовому фильтру (VideoFilterOut)

            Dictionary<string, VideoLinksViewModel> dictionaryOfLinksByVideoFilterOut = [];

            string[] videoFilterOut = editPage.PageItem.VideoFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

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

                        dictionaryOfLinksByVideoFilterOut[videoFilterOut[i]] = videoLinksViewModel;

                        editPage.DictionaryOfLinksByVideoFilterOut = dictionaryOfLinksByVideoFilterOut;
                    }
                }
            }

            #endregion

            #region Группы картинок по фильтрам (PhotoFilterOut)

            Dictionary<string, ImageListViewModel> dictionaryOfLinksByFotoFilterOut = [];

            string[] pictureFiltersOut = editPage.PageItem.PhotoFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (pictureFiltersOut.Length > 0)
            {
                for (int i = 0; i < pictureFiltersOut.Length; i++)
                {
                    if (await imageContext.ImageFiles.Where(img => img.SearchFilter.Contains(pictureFiltersOut[i])).AnyAsync())
                    {
                        var pictures = await imageContext.ImageFiles.Where(img => img.SearchFilter.Contains(pictureFiltersOut[i])).ToListAsync();

                        _ = pictures.OrderBy(p => p.ImageCaption);

                        dictionaryOfLinksByFotoFilterOut[pictureFiltersOut[i]] = new();

                        // TODO Сформировать ImageListViewModel для передачи в EditPage
                    }
                }
            }

            #endregion

            #region Кадры слева и справа от текста

            FramesAroundMainContentModel framesAroundMainContent = new();

            if (editPage.PageItem.OgType == "book")
            {
                if (editPage.PageItem.RoutData.Contains("bookid", StringComparison.CurrentCultureIgnoreCase))
                {
                    string[] routData = editPage.PageItem.RoutData[1..].Split('&');

                    var dictionaryOfRoutdata = routData.Select(part => part.Split('=')).ToDictionary(split => split[0], split => split[1]);

                    if (Guid.TryParse(dictionaryOfRoutdata["bookid"], out var bookGuid))
                    {
                        if (await bookAndArticleContext.BooksAndArticles.Where(book => book.BooksAndArticlesModelId == bookGuid).AnyAsync())
                        {
                            var bookForPage = await bookAndArticleContext.BooksAndArticles.FirstAsync(book => book.BooksAndArticlesModelId == bookGuid);

                            var imageItems = await imageContext.ImageFiles
                                                            .Where(img => img.SearchFilter.ToLower().Contains(bookForPage.CaptionOfText.ToLower()))
                                                            .ToArrayAsync();

                            imageItems = [.. imageItems.Shuffle()];

                            if (imageItems.Length > 1)
                            {
                                framesAroundMainContent.FramesOnTheLeft = [.. imageItems.Take(imageItems.Length / 2)];
                                framesAroundMainContent.FramesOnTheRight = [.. imageItems.Skip(imageItems.Length / 2)];

                                editPage.FramesAroundMainContent = framesAroundMainContent;
                            }
                        }
                    }
                }
            }

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
            #region Инициализация экземпляра страницы

            PageInfoModel pageUpdate = await pageInfoContext.PagesInfo.FirstAsync(i => i.PageInfoModelId == editPage.PageItem.PageInfoModelId);

            #endregion

            #region Инициализация иконки страницы

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

            #endregion

            #region Изменить адрес страницы

            #region MVC или RazorPage

            pageUpdate.PageAsRazorPage = editPage.PageItem.PageAsRazorPage;

            #endregion

            #region Area

            if (string.IsNullOrEmpty(editPage.PageItem.PageArea.Trim()) || editPage.PageItem.PageArea == "Root")
            {
                pageUpdate.PageArea = string.Empty;
            }
            else
            {
                pageUpdate.PageArea = "/" + editPage.PageItem.PageArea.Trim().Trim('/').ToLower();
            }

            #endregion

            #region Controller

            if (string.IsNullOrWhiteSpace(editPage.PageItem.Controller) || string.IsNullOrEmpty(editPage.PageItem.Controller) || editPage.PageItem.PageAsRazorPage)
            {
                pageUpdate.Controller = string.Empty;
            }
            else
            {
                pageUpdate.Controller = "/" + editPage.PageItem.Controller.Trim().Trim('/').ToLower();
            }

            #endregion

            #region Action

            if (string.IsNullOrWhiteSpace(editPage.PageItem.Action) || string.IsNullOrEmpty(editPage.PageItem.Action) || editPage.PageItem.PageAsRazorPage)
            {
                pageUpdate.Action = string.Empty;
            }
            else
            {
                pageUpdate.Action = "/" + editPage.PageItem.Action.Trim().Trim('/').ToLower();
            }

            #endregion

            #region QueryString

            if (string.IsNullOrWhiteSpace(editPage.PageItem.RoutData) || string.IsNullOrEmpty(editPage.PageItem.RoutData))
            {
                pageUpdate.RoutData = string.Empty;
            }
            else
            {
                pageUpdate.RoutData = "?" + editPage.PageItem.RoutData.Trim().Trim('/').TrimStart('?').ToLower();
            }

            #endregion

            #region Адрес (для RazorPage) или Действие (для MVC)

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

            #endregion

            #region Псевдоним адреса

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

            #endregion

            #region Изменить индекс сортировки

            pageUpdate.SortOfPage = editPage.PageItem.SortOfPage;

            #endregion

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

            #region Изменить заголовок страницы теги title, description, keywords

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

            // поиск связанных страниц по GUID (1)
            pageUpdate.PageLinks = editPage.PageItem.PageLinks;
            pageUpdate.RefPages = editPage.PageItem.RefPages.Trim().ToLower();

            // поиск связанных страниц по GUID (2)
            pageUpdate.PageLinks2 = editPage.PageItem.PageLinks2;
            pageUpdate.RefPages2 = editPage.PageItem.RefPages2.Trim().ToLower();

            // поиск связанных страниц по фильтру
            pageUpdate.PageLinksByFilters = editPage.PageItem.PageLinksByFilters;
            pageUpdate.PageFilterOut = editPage.PageItem.PageFilterOut.Trim();

            // поиск связанных видео
            pageUpdate.VideoLinks = editPage.PageItem.VideoLinks;
            pageUpdate.VideoFilterOut = editPage.PageItem.VideoFilterOut.Trim();

            // альбом картинок
            pageUpdate.PhotoLinks = editPage.PageItem.PhotoLinks;
            pageUpdate.PhotoFilterOut = editPage.PageItem.PhotoFilterOut.Trim();

            #endregion

            #region Сохранить изменения

            await pageInfoContext.SaveChangesInPageAsync();

            #endregion

            #region Переадресация на страницу информации о странице

            return RedirectToAction("DetailsPage", new { pageId = pageUpdate.PageInfoModelId, Area = "Admin" });

            #endregion
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