using System.Drawing;

namespace ShevkunenkoSite.Controllers;

public class BooksController(
    IBooksAndArticlesRepository articleContext,
    ITextInfoRepository textContext,
    IPageInfoRepository pageContext,
    IImageFileRepository imageFileContext,
    IWebHostEnvironment hostEnvironment) : Controller
{
    private readonly string rootPath = hostEnvironment.WebRootPath;

    public IActionResult Index() => View();

    public async Task<IActionResult> Book(Guid? bookId, int? pageNumber, bool? scan)
    {
        if (bookId != null
            && await articleContext.BooksAndArticles.Where(article => article.BooksAndArticlesModelId == bookId).AnyAsync()
            && pageNumber != null
            && pageNumber > -1
            && pageNumber <= articleContext.BooksAndArticles.First(article => article.BooksAndArticlesModelId == bookId).NumberOfPages)
        {
            #region Если есть только скан

            if (!await textContext.Texts.Where(text => text.BooksAndArticlesModelId == bookId).AnyAsync() & scan == true)
            {
                ArticleViewModel scanForArticle = new()
                {
                    BookOrArticle = await articleContext.BooksAndArticles
                       .Include(logo => logo.LogoOfArticle)
                       .Include(scan => scan.ScanOfArticle)
                       .Include(movie => movie.VideoForBookOrArticle)
                       .FirstAsync(article => article.BooksAndArticlesModelId == bookId),

                    PageInfo = await pageContext.GetPageInfoByPathAsync(HttpContext),

                    HtmlText = null,

                    PageNumber = null,

                    Scan = true
                };

                return View("Book", scanForArticle);
            }

            #endregion

            #region Если для статьи или книги отсутствует текст

            if (!await textContext.Texts.Where(text => text.BooksAndArticlesModelId == bookId & text.SequenceNumber == pageNumber).AnyAsync())
            {
                return RedirectToAction(nameof(Index));
            }

            var textForBookOrArticle = await textContext.Texts.FirstAsync(text => text.BooksAndArticlesModelId == bookId & text.SequenceNumber == pageNumber);

            if (!System.IO.File.Exists(rootPath + DataConfig.TextsFolderPath + textForBookOrArticle.FolderForText + textForBookOrArticle.HtmlFileName))
            {
                return RedirectToAction(nameof(Index));
            }

            #endregion

            #region Экземпляр  книги или статьи

            var bookOrArticleItem = await articleContext.BooksAndArticles
                        .Include(logo => logo.LogoOfArticle)
                        .Include(scan => scan.ScanOfArticle)
                        .Include(movie => movie.VideoForBookOrArticle)
                        .FirstAsync(article => article.BooksAndArticlesModelId == bookId);

            #endregion

            #region Html текст

            using StreamReader htmlText = new(rootPath + DataConfig.TextsFolderPath + textForBookOrArticle.FolderForText + textForBookOrArticle.HtmlFileName);

            var textWithMarkUp = htmlText.ReadToEnd();

            #endregion

            #region Кадры слева и справа

            FramesAroundMainContentModel framesAroundMainContent = new();

            var imageItems = await imageFileContext.ImageFiles
                .Where(img => img.SearchFilter.ToLower().Contains(bookOrArticleItem.CaptionOfText.ToLower()))
                .ToArrayAsync();

            imageItems = [.. imageItems.Shuffle()];

            if (imageItems.Length > 1)
            {
                framesAroundMainContent.FramesOnTheLeft = [.. imageItems.Take(imageItems.Length / 2)];
                framesAroundMainContent.FramesOnTheRight = [.. imageItems.Skip(imageItems.Length / 2)];
            }

            #endregion

            #region Фото из книги

            List<ImageFileModel>? listOfPictures = null;

            if (await imageFileContext.ImageFiles.Where(img => img.SearchFilter.Contains(bookOrArticleItem.CaptionOfText)).AnyAsync())
            {
                //listOfPictures = await imageFileContext.ImageFiles.Where(img => img.SearchFilter.Contains(bookOrArticleItem.CaptionOfText)).ToListAsync();

                var listOfPictures2 = from m in imageFileContext.ImageFiles
                   .Where(p => p.SearchFilter.Contains(bookOrArticleItem.CaptionOfText + "#album#"))
                   .OrderBy(p => p.SortOfPicture)
                                      select m;

                listOfPictures = [.. listOfPictures2.AsEnumerable()];
            }

            #endregion

            #region ViewModel

            ArticleViewModel bookOrArticle = new()
            {
                BookOrArticle = bookOrArticleItem,
                PageInfo = await pageContext.GetPageInfoByPathAsync(HttpContext),
                HtmlText = textWithMarkUp,
                PageNumber = pageNumber,
                Scan = scan,
                FramesAroundMainContent = framesAroundMainContent,
                ListOfPictures = listOfPictures
            };

            #endregion

            return View("Book", bookOrArticle);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> PhotoAlbum(Guid? imageId, int pageNumber = 1)
    {
        if (imageId == null
            || await imageFileContext.ImageFiles.Where(img => img.ImageFileModelId == imageId).AnyAsync() == false)
        {
            return RedirectToAction(nameof(Index));
        }

        var imageItem = await imageFileContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == imageId);

        PhotoAlbumViewModel photoAlbumView = new();

        if (imageItem.SearchFilter.Contains("#album#"))
        {
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
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }

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
        photoAlbumView.PagingInfo.TotalItems = allItems.Count();
        photoAlbumView.PagingInfo.CurrentPage = pageNumber;

        return View(photoAlbumView);
    }
}
