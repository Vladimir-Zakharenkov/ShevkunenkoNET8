namespace ShevkunenkoSite.Areas.Admin.Controllers;

// Имена методов не начинать со слова Page
[Area("Admin")]
[Authorize]
public class PageInfoController(
    IPageInfoRepository pageInfoContext,
    IIconFileRepository iconContext,
    IImageFileRepository imageContext,
    IBackgroundFotoRepository backgroundContext) : Controller
{
    #region Список страниц сайта

    public int pagesPerPage = 10;

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
            PageInfoModel pageItem = await pageInfoContext.PagesInfo
                .AsNoTracking()
                .FirstAsync(p => p.PageInfoModelId == pageId);

            if (pageItem == null)
            {
                return RedirectToAction(nameof(Index));
            }

           var iconItem2 = await iconContext.IconFiles.FirstAsync(icon => icon.IconPath == pageItem.PageIconPath);

            IconFileModel iconItem = await iconContext.IconFiles
                .FirstAsync(icon => icon.IconPath == pageItem.PageIconPath && icon.IconFileName == @DataConfig.IconItem);

            iconItem ??= await iconContext.IconFiles.FirstAsync(icon => icon.IconPath == "/main" && icon.IconFileName == @DataConfig.IconItem);

            #region Ссылки на страницы

            string[] pageFiltersOut = Array.Empty<string>();

            List<PageInfoModel> inList = new();

            PageInfoModel[] linksOnPages = Array.Empty<PageInfoModel>();

            if (!string.IsNullOrEmpty(pageItem.PageFilterOut))
            {
                pageFiltersOut = pageItem.PageFilterOut.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < pageFiltersOut.Length; i++)
                {
                    linksOnPages = await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFiltersOut[i].Trim() + ",")).OrderBy(p => p.PageCardText).ToArrayAsync();

                    inList.AddRange(linksOnPages);
                }

                linksOnPages = inList.Distinct().ToArray();
            }

            #endregion

            #region Сылающиеся страницы

            string[] pageFilters = pageItem.PageFilter.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<PageInfoModel> outList = new();

            PageInfoModel[] linksFromPages = Array.Empty<PageInfoModel>();

            if (!string.IsNullOrEmpty(pageItem.PageFilter))
            {
                for (int i = 0; i < pageFilters.Length; i++)
                {
                    linksFromPages = await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFilters[i].Trim() + ",")).OrderBy(p => p.PageCardText).ToArrayAsync();

                    outList.AddRange(linksFromPages);
                }

                linksFromPages = outList.ToArray();
            }

            #endregion

            return View(new DetailsPageViewModel
            {
                PageItem = pageItem,
                IconItem = iconItem,
                LinksOnPages = linksOnPages,
                LinksFromPages = linksFromPages
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
    public ViewResult AddPage() => View();

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
                var newBackground = await backgroundContext.BackgroundFiles.FirstAsync(bk => bk.WebRightBackground == "FotoPlenka.webp");

                addItem.PageItem.BackgroundFileModelId = newBackground.BackgroundFileModelId;
            }

            #endregion

            if (addItem.PageItem.OgType == "website")
            {
                addItem.PageItem.PageIconPath = "/main";
                addItem.PageItem.BrowserConfig = "main.xml";
                addItem.PageItem.BrowserConfigFolder = "/main";
                addItem.PageItem.Manifest = "main.json";
            }
            else if (addItem.PageItem.OgType == "movie")
            {
                addItem.PageItem.PageIconPath = "/movie";
                addItem.PageItem.BrowserConfig = "movie.xml";
                addItem.PageItem.BrowserConfigFolder = "/movie";
                addItem.PageItem.Manifest = "movie.json";
            }
            else
            {
                addItem.PageItem.PageIconPath = "/main";
                addItem.PageItem.BrowserConfig = "main.xml";
                addItem.PageItem.BrowserConfigFolder = "/main";
                addItem.PageItem.Manifest = "main.json";
            }

            if (addItem.PageItem.PageArea == "admin")
            {
                addItem.PageItem.PageIconPath = "/admin";
                addItem.PageItem.BrowserConfig = "admin.xml";
                addItem.PageItem.BrowserConfigFolder = "/admin";
                addItem.PageItem.Manifest = "admin.json";
            }

            if (string.IsNullOrWhiteSpace(addItem.PageItem.PageArea) || string.IsNullOrEmpty(addItem.PageItem.PageArea))
            {
                addItem.PageItem.PageArea = string.Empty;
            }
            else
            {
                addItem.PageItem.PageArea = "/" + addItem.PageItem.PageArea.Trim().Trim('/').ToLower();
            }

            addItem.PageItem.PageAsRazorPage = addItem.PageItem.PageAsRazorPage;

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

            if (addItem.PageItem.PageAsRazorPage)
            {
                if (string.IsNullOrWhiteSpace(addItem.PageItem.PageLoc) || string.IsNullOrEmpty(addItem.PageItem.PageLoc))
                {
                    ModelState.AddModelError("PageItem.PageLoc", "Введите адрес старницы без области");

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

            if (string.IsNullOrWhiteSpace(addItem.PageItem.RoutData) || string.IsNullOrEmpty(addItem.PageItem.RoutData))
            {
                addItem.PageItem.RoutData = string.Empty;
            }
            else
            {
                addItem.PageItem.RoutData = "?" + addItem.PageItem.RoutData.Trim().Trim('/').TrimStart('?').ToLower();
            }

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

            addItem.PageItem.PageTitle = addItem.PageItem.PageTitle.Trim();
            addItem.PageItem.PageDescription = addItem.PageItem.PageDescription.Trim();
            addItem.PageItem.PageCardText = addItem.PageItem.PageCardText.Trim().ToUpper();
            addItem.PageItem.PageFilter = addItem.PageItem.PageFilter.Trim();
            addItem.PageItem.PageFilterOut = addItem.PageItem.PageFilterOut.Trim();
            addItem.PageItem.PageLastmod = DateTime.Now;
            addItem.PageItem.PageLinks = addItem.PageItem.PageLinks;
            addItem.PageItem.RefPages = addItem.PageItem.RefPages.Trim();

            if (!string.IsNullOrEmpty(addItem.PageItem.PagePathNickName) && await pageInfoContext.PagesInfo.Where(p => p.PagePathNickName == addItem.PageItem.PagePathNickName).AnyAsync())
            {
                ModelState.AddModelError("pageItem.PagePathNickName", $"Страница с псевдонимом «{addItem.PageItem.PagePathNickName}» уже существует");

                return View();
            }

            string checkPageFullPathWithData = string.Empty;

            if (addItem.PageItem.PageAsRazorPage)
            {
                checkPageFullPathWithData = addItem.PageItem.PageArea + addItem.PageItem.PageLoc + addItem.PageItem.RoutData;
            }
            else
            {
                checkPageFullPathWithData = addItem.PageItem.PageArea + addItem.PageItem.Controller + addItem.PageItem.Action + addItem.PageItem.RoutData;
            }

            if (await pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == checkPageFullPathWithData).AnyAsync())
            {
                ModelState.AddModelError("pageItem.PageLoc", $"Страница «{addItem.PageItem.PageArea + addItem.PageItem.PageLoc + addItem.PageItem.RoutData}» уже существует");

                return View();
            }

            await pageInfoContext.AddNewPageAsync(addItem.PageItem);

            PageInfoModel newPage = await pageInfoContext.PagesInfo.FirstAsync(p => p.PageFullPathWithData == checkPageFullPathWithData);

            return RedirectToAction("DetailsPage", new { pageId = newPage.PageInfoModelId, Area = "Admin" });
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

            editPage.IconItem = await iconContext.IconFiles.FirstAsync(icon => icon.IconPath == editPage.PageItem.PageIconPath && icon.IconFileName == @DataConfig.IconItem);

            editPage.IconItem ??= await iconContext.IconFiles.FirstAsync(icon => icon.IconPath == "/main" && icon.IconFileName == @DataConfig.IconItem);

            editPage.ImageFileFormFile = null;

            editPage.LinksOnPages = await pageInfoContext.PagesInfo.Where(p => p.RefPages.Contains(editPage.PageItem.PageInfoModelId.ToString())).ToArrayAsync();

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

            IconFileModel editPageiconItem = await iconContext.IconFiles
                .FirstAsync(icon => icon.IconPath == pageUpdate.PageIconPath && icon.IconFileName == @DataConfig.IconItem);

            editPageiconItem ??= await iconContext.IconFiles
                .FirstAsync(icon => icon.IconPath == "/main" && icon.IconFileName == @DataConfig.IconItem);

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

            pageUpdate.PageAsRazorPage = editPage.PageItem.PageAsRazorPage;
            pageUpdate.PageTitle = editPage.PageItem.PageTitle.Trim();
            pageUpdate.PageDescription = editPage.PageItem.PageDescription.Trim();
            pageUpdate.PageCardText = editPage.PageItem.PageCardText.Trim().ToUpper();
            pageUpdate.PageFilter = editPage.PageItem.PageFilter.Trim();
            pageUpdate.PageFilterOut = editPage.PageItem.PageFilterOut.Trim();
            pageUpdate.PageLastmod = DateTime.Now;
            pageUpdate.Changefreq = editPage.PageItem.Changefreq.Trim();
            pageUpdate.PageLinks = editPage.PageItem.PageLinks;
            pageUpdate.RefPages = editPage.PageItem.RefPages.Trim();
            pageUpdate.Priority = editPage.PageItem.Priority.Trim();

            if (editPage.PageItem.OgType == "website")
            {
                pageUpdate.OgType = "website";
                pageUpdate.PageIconPath = "/main";
                pageUpdate.BrowserConfig = "main.xml";
                pageUpdate.BrowserConfigFolder = "/main";
                pageUpdate.Manifest = "main.json";
            }
            else if (editPage.PageItem.OgType == "movie")
            {
                pageUpdate.OgType = "movie";
                pageUpdate.PageIconPath = "/movie";
                pageUpdate.BrowserConfig = "movie.xml";
                pageUpdate.BrowserConfigFolder = "/movie";
                pageUpdate.Manifest = "movie.json";
            }
            else
            {
                pageUpdate.PageIconPath = "/main";
                pageUpdate.BrowserConfig = "main.xml";
                pageUpdate.BrowserConfigFolder = "/main";
                pageUpdate.Manifest = "main.json";
            }

            if (pageUpdate.PageArea == "/admin")
            {
                pageUpdate.PageIconPath = "/admin";
                pageUpdate.BrowserConfig = "admin.xml";
                pageUpdate.BrowserConfigFolder = "/admin";
                pageUpdate.Manifest = "admin.json";
            }

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



    #endregion
}