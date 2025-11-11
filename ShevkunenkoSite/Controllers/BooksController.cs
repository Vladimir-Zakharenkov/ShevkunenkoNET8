namespace ShevkunenkoSite.Controllers;

public class BooksController(
    IBooksAndArticlesRepository articleContext,
    ITextInfoRepository textContext,
    IPageInfoRepository pageContext,
    IImageFileRepository imageFileContext,
    IAudioBookRepository audioBookContext,
    IAudioInfoRepository audioFileContext,
    IWebHostEnvironment hostEnvironment) : Controller
{
    private readonly string rootPath = hostEnvironment.WebRootPath;

    public IActionResult Index() => View();

    public async Task<IActionResult> Book(string? bookCaption, int? pageNumber)
    {
        #region Проверка наличия книги с таким названием

        if (bookCaption == null
            || bookCaption is not string
            || !await articleContext.BooksAndArticles
                        .Where(article => article.CaptionOfText == bookCaption.Replace('-', ' '))
                        .AnyAsync())
        {
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Инициализация ViewModel

        ArticleViewModel articleViewModel = new();

        #endregion

        #region Экземпляр книги (статьи)

        articleViewModel.BookOrArticle = await articleContext.BooksAndArticles
                .Include(logo => logo.LogoOfArticle)
                .Include(scan => scan.ScanOfArticle)
                .Include(movie => movie.VideoForBookOrArticle)
                .FirstAsync(article => article.CaptionOfText == bookCaption.Replace('-', ' '));

        #endregion

        #region Если страница указана не верно

        if (pageNumber == null
            || pageNumber is not int
            || pageNumber < 0
            || pageNumber > articleViewModel.BookOrArticle.NumberOfPages)
        {
            return RedirectToAction(nameof(Book), new { bookCaption, pageNumber = 0 });
        }

        #endregion

        #region Номер текущей страницы

        articleViewModel.PageNumber = pageNumber;

        #endregion

        #region Экземпляр текста для текущей страницы

        if (await textContext.Texts
            .Where(text => text.BooksAndArticlesModelId == articleViewModel.BookOrArticle.BooksAndArticlesModelId & text.SequenceNumber == pageNumber)
            .AnyAsync())
        {
            articleViewModel.TextForBookOrArticle = await textContext.Texts
                .Include(audio => audio.AudioInfoModel)
                .FirstAsync(text => text.BooksAndArticlesModelId == articleViewModel.BookOrArticle.BooksAndArticlesModelId & text.SequenceNumber == pageNumber);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region HTML текст

        if (System.IO.File.Exists(rootPath + DataConfig.TextsFolderPath + articleViewModel.TextForBookOrArticle.FolderForText + articleViewModel.TextForBookOrArticle.HtmlFileName))
        {
            using StreamReader htmlText = new(rootPath + DataConfig.TextsFolderPath + articleViewModel.TextForBookOrArticle.FolderForText + articleViewModel.TextForBookOrArticle.HtmlFileName);

            articleViewModel.HtmlText = htmlText.ReadToEnd();
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Аудиофайл (аудиокнига) связанная с экземпляром текста

        if (articleViewModel.TextForBookOrArticle.AudioInfoModelId != null)
        {
            if (await audioFileContext.AudioFiles
                .Where(audio => audio.AudioInfoModelId == articleViewModel.TextForBookOrArticle.AudioInfoModelId)
                .AnyAsync())
            {
                articleViewModel.AudioFileForText = await audioFileContext.AudioFiles
                    .Include(abook => abook.AudioBookModel)
                    .FirstAsync(audio => audio.AudioInfoModelId == articleViewModel.TextForBookOrArticle.AudioInfoModelId);
            }
        }

        #endregion

        #region Информация о старнице сайта

        articleViewModel.PageInfo = await pageContext.GetPageInfoByPathAsync(HttpContext);

        #endregion

        #region Фото из книги - Фото слева и справа от текста

        if (await imageFileContext.ImageFiles
            .Where(img => img.SearchFilter.Contains(articleViewModel.BookOrArticle.CaptionOfText.Replace(' ', '-') + "#album#"))
            .AnyAsync())
        {
            var listOfPictures = from m in imageFileContext.ImageFiles
               .Where(p => p.SearchFilter.Contains(articleViewModel.BookOrArticle.CaptionOfText.Replace(' ', '-') + "#album#"))
               .OrderBy(p => p.SortOfPicture)
                                 select m;

            articleViewModel.ListOfPictures = [.. listOfPictures.AsEnumerable()];

            if (articleViewModel.ListOfPictures.Count > 1)
            {
                articleViewModel.FramesAroundMainContent = new FramesAroundMainContentModel
                {
                    FramesOnTheLeft = [.. articleViewModel.ListOfPictures.Take(articleViewModel.ListOfPictures.Count / 2)],
                    FramesOnTheRight = [.. articleViewModel.ListOfPictures.Skip(articleViewModel.ListOfPictures.Count / 2)]
                };
            }
            else
            {
                articleViewModel.FramesAroundMainContent = new FramesAroundMainContentModel
                {
                    FramesOnTheLeft = [.. articleViewModel.ListOfPictures],
                    FramesOnTheRight = [.. articleViewModel.ListOfPictures]
                };
            }
        }

        #endregion

        return View("Book", articleViewModel);

    }

    public async Task<IActionResult> AudioBook(string? audioBookCaption, int? audioBookPart, string? audioActor)
    {
        AudioBookModel audioBook = await audioBookContext.AudioBooks
            .Include(include => include.BookForAudioBook)
            .FirstAsync(audioBook => audioBook.CaptionOfAudioBook == audioBookCaption);

        return View(audioBook);
    }

    public async Task<IActionResult> PhotoAlbum(Guid? imageId, string? albumCaption, int pageNumber = 1)
    {
        if (
            (imageId == null & string.IsNullOrEmpty(albumCaption))
            || (imageId != null & await imageFileContext.ImageFiles.Where(img => img.ImageFileModelId == imageId).AnyAsync() == false)
            || (albumCaption != null & await imageFileContext.ImageFiles.Where(img => img.SearchFilter.Contains(albumCaption + "#album#")).AnyAsync() == false)
            )
        {
            return RedirectToAction(nameof(Index));
        }

        PhotoAlbumViewModel photoAlbumView = new();

        #region Просмотр картинки

        if (imageId != null)
        {
            photoAlbumView.AlbumOrPhoto = false;

            var imageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == imageId);

            if (imageItem.SearchFilter.Contains("#album#"))
            {
                #region Определение заголовка и подзаголовка альбома

                string[] filters = imageItem.SearchFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);

                string? filterForCaption = Array.Find(filters, p => p.Contains("#album#"));

                if (filterForCaption != null)
                {
                    int foundForCaption = filterForCaption.IndexOf("#album#");

                    photoAlbumView.CaptionOfAlbum = filterForCaption[..foundForCaption];

                    if (filterForCaption.Contains("#note#"))
                    {
                        int foundForNote = filterForCaption.IndexOf("#note#");

                        photoAlbumView.NoteForCaptionOfAlbum = filterForCaption[(foundForCaption + 7)..foundForNote];
                    }
                }

                #endregion

                var allItems = from m in imageFileContext.ImageFiles
                   .Where(p => p.SearchFilter.Contains(photoAlbumView.CaptionOfAlbum + "#album#"))
                   .OrderBy(p => p.SortOfPicture)
                               select m;

                var arrayOfItems = await allItems.ToArrayAsync();

                var indexOfItem = Array.FindIndex(arrayOfItems, item => item.ImageFileModelId == imageId) + 1;

                if (indexOfItem < DataConfig.NumberOfItemsPerPage)
                {
                    pageNumber = 1;
                }
                else if (indexOfItem % (DataConfig.NumberOfItemsPerPage) == 0)
                {
                    pageNumber = indexOfItem / DataConfig.NumberOfItemsPerPage;
                }
                else
                {
                    pageNumber = indexOfItem / DataConfig.NumberOfItemsPerPage + 1;
                }

                var itemsOnPage = await allItems
                   .Skip((pageNumber - 1) * photoAlbumView.PagingInfo.ItemsPerPage)
                   .Take(photoAlbumView.PagingInfo.ItemsPerPage)
                   .ToArrayAsync();

                photoAlbumView.ItemsOnPage = itemsOnPage;
                photoAlbumView.CurrentImageId = imageId;
                photoAlbumView.PagingInfo.TotalItems = allItems.Count();
                photoAlbumView.PagingInfo.CurrentPage = pageNumber;
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region Просмотр страницы альбома

        else
        {
            #region Все картинки по фильтру albumCaption + "#album#

            var allItems = from m in imageFileContext.ImageFiles
               .Where(p => p.SearchFilter.ToLower().Contains(albumCaption + "#album#"))
               .OrderBy(p => p.SortOfPicture)
                           select m;

            var allItemsArray = await allItems.ToArrayAsync();

            #endregion

            #region Проверка параметра pageNumber

            if (pageNumber < 1
                || pageNumber > (allItemsArray.Length % photoAlbumView.PagingInfo.ItemsPerPage == 0 ? (allItemsArray.Length / photoAlbumView.PagingInfo.ItemsPerPage) : (allItemsArray.Length / photoAlbumView.PagingInfo.ItemsPerPage + 1)))
            {
                return RedirectToAction(nameof(PhotoAlbum), new { pageNumber = 1 });
            }

            #endregion

            #region Картинки на текущей странице

            var itemsOnPage = await allItems
                   .Skip((pageNumber - 1) * photoAlbumView.PagingInfo.ItemsPerPage)
                   .Take(photoAlbumView.PagingInfo.ItemsPerPage)
                   .ToArrayAsync();

            #endregion

            #region Определение заголовка и подзаголовка альбома

            string[] filters = itemsOnPage[0].SearchFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);

            string? filterForCaption = Array.Find(filters, p => p.Contains("#album#"));

            if (filterForCaption != null)
            {
                int foundForCaption = filterForCaption.IndexOf("#album#");

                photoAlbumView.CaptionOfAlbum = filterForCaption[..foundForCaption];

                if (filterForCaption.Contains("#note#"))
                {
                    int foundForNote = filterForCaption.IndexOf("#note#");

                    photoAlbumView.NoteForCaptionOfAlbum = filterForCaption[(foundForCaption + 7)..foundForNote];
                }
            }

            #endregion

            photoAlbumView.AlbumOrPhoto = true;
            photoAlbumView.ItemsOnPage = itemsOnPage;
            photoAlbumView.CurrentImageId = itemsOnPage[0].ImageFileModelId;
            photoAlbumView.PagingInfo.TotalItems = allItems.Count();
            photoAlbumView.PagingInfo.CurrentPage = pageNumber;
        }

        #endregion

        return View(photoAlbumView);
    }
}