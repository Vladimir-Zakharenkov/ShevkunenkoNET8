// Ignore Spelling: Programmy

namespace ShevkunenkoSite.Controllers;

//[Route("/All-Video/[action]")]
public class AllVideoController : Controller
{
    private readonly IMovieFileRepository _movieContext;
    private readonly IImageFileRepository _imageContext;
    private readonly ITopicMovieRepository _topicMovieContext;
    private readonly IPageInfoRepository _pageInfoContext;
    public AllVideoController(IMovieFileRepository movieContext,
                                        IImageFileRepository imageContext,
                                        ITopicMovieRepository topicMovieContext,
                                        IPageInfoRepository pageInfoContext)
    {
        _movieContext = movieContext;
        _imageContext = imageContext;
        _topicMovieContext = topicMovieContext;
        _pageInfoContext = pageInfoContext;
    }

    public int moviesPerPage = 12;

    #region AllVideoController for Tests

    //public ViewResult Index(int pageNumber = 1) =>
    //View(new MoviesListViewModel
    //{
    //    Movies = _movieContext.MovieFiles
    //        .Where(p => p.PageInfoModelId != null & p.MoviePart < 2)
    //        .OrderBy(p => p.MovieCaption)
    //        .Skip((pageNumber - 1) * moviesPerPage)
    //        .Take(moviesPerPage)
    //        .ToArray(),

    //    PagingInfo = new PagingInfoViewModel
    //    {
    //        CurrentPage = pageNumber,
    //        ItemsPerPage = moviesPerPage,
    //        TotalItems = _movieContext.MovieFiles.Count()
    //    }
    //});

    #endregion

    #region AllVideoController for Publish

    public async Task<ViewResult> Index(Guid? topicId, int pageNumber = 1)
    {
        MoviesListViewModel moviesListViewModel = new();

        List<PageInfoModel> pagesForMovies = Enumerable.Empty<PageInfoModel>().ToList();

        // выборка фильмов по теме
        if (topicId.HasValue & await _topicMovieContext.TopicMovies.Where(t => t.TopicMovieModelId == topicId).AnyAsync())
        {
            TopicMovieModel topicMovie = await _topicMovieContext.TopicMovies.FirstAsync(mt => mt.TopicMovieModelId == topicId);

            moviesPerPage = topicMovie.NumberOfLinksPerPage;

            moviesListViewModel.Movies = await _movieContext.MovieFiles
                .Where(p => p.MovieInMainList == true & p.TopicGuidList
                .Contains(topicMovie.TopicMovieModelId.ToString()) == true)
                .OrderBy(p => p.MovieDatePublished)
                .Skip((pageNumber - 1) * moviesPerPage)
                .Take(moviesPerPage)
                .ToArrayAsync();

            moviesListViewModel.PageHeadTitle = topicMovie.TopicHeadPage;

            moviesListViewModel.IsImage = topicMovie.ImageForRef;

            moviesListViewModel.IconType = topicMovie.IconTypeForRef;

            moviesListViewModel.IsPartsMoreOne = topicMovie.GeneralPageForMovieEpisodes;

            moviesListViewModel.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = pageNumber,
                ItemsPerPage = topicMovie.NumberOfLinksPerPage,
                TotalItems = _movieContext.MovieFiles
                    .Where(p => p.MovieInMainList == true & p.TopicGuidList
                        .Contains(topicMovie.TopicMovieModelId.ToString()) == true).Count()
            };
        }
        // выборка всех фильмов
        else
        {
            moviesListViewModel.Movies = await _movieContext.MovieFiles
                .Where(p => p.MovieInMainList == true)
                .OrderBy(p => p.MovieDatePublished)
                .Skip((pageNumber - 1) * moviesPerPage)
                .Take(moviesPerPage)
                .ToArrayAsync();

            moviesListViewModel.PageHeadTitle = "ВИДЕО НА САЙТЕ";

            moviesListViewModel.IsImage = false;

            moviesListViewModel.IconType = "webicon300";

            moviesListViewModel.IsPartsMoreOne = true;

            moviesListViewModel.PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = pageNumber,
                ItemsPerPage = moviesPerPage,
                TotalItems = _movieContext.MovieFiles
                    .Where(p => p.MovieInMainList == true & p.SearchFilter.Contains(string.Empty))
                    .Count()
            };
        }

        if (moviesListViewModel.IsImage == null)
        {
            var pagesOfMovies = from p in moviesListViewModel.Movies
                                select p.PageInfoModelId;

            foreach (var p in pagesOfMovies)
            {
                if (await _pageInfoContext.PagesInfo.Where(i => i.PageInfoModelId == p).AnyAsync())
                {
                    var pg = await _pageInfoContext.PagesInfo.FirstAsync(i => i.PageInfoModelId == p);

                    pagesForMovies.Add(pg);
                }
            }

            moviesListViewModel.MoviePages = pagesForMovies;
        }
        else
        {
            moviesListViewModel.MoviePages = Enumerable.Empty<PageInfoModel>();
        }

        return View(moviesListViewModel);
    }

    [Route("video-na-saite/shevkunenko-roli-v-kino")]
    [ActionName("Shevkunenko-roli-v-kino")]
    public async Task<ViewResult> Shevkunenko() =>
        View("Index", new MoviesListViewModel
        {
            Movies = await _movieContext.MovieFiles
                .AsNoTracking()
                .Where(p => p.MovieInMainList == true & p.SearchFilter.Contains("Шевкуненко,"))
                .OrderBy(p => p.MovieDatePublished)
                .ToArrayAsync(),

            IsImage = false,

            IsPartsMoreOne = true,

            PageHeadTitle = "<h1 class=\"text_shadow\">СЕРГЕЙ ШЕВКУНЕНКО - РОЛИ&nbsp;В&nbsp;КИНО</h1>"
        });

    [Route("video-na-saite/programmy-o-sergee-shevkunenko")]
    [ActionName("Programmy-o-Sergee-Shevkunenko")]
    public async Task<ViewResult> Programmy() =>
        View("Index", new MoviesListViewModel
        {
            Movies = await _movieContext.MovieFiles
                .AsNoTracking()
                .Where(p => p.SearchFilter.Contains("Программа,"))
                .OrderBy(p => p.MovieDatePublished)
                .ToArrayAsync(),

            IsImage = true,

            IsPartsMoreOne = false,

            PageHeadTitle = "<h1 class=\"fs-4 text_shadow\">ФИЛЬМЫ И ПРОГРАММЫ О<br /><span class=\"fs-2\">СЕРГЕЕ ШЕВКУНЕНКО</span></h1>"
        });

    [Route("video-na-saite/znakomye-o-sergee-shevkunenko")]
    [ActionName("Znakomye-o-Sergee-Shevkunenko")]
    public async Task<ViewResult> Znakomye() =>
    View("Index", new MoviesListViewModel
    {
        Movies = await _movieContext.MovieFiles
            .AsNoTracking()
            .Where(p => p.SearchFilter.Contains("Знакомые,"))
            .OrderBy(p => p.MovieDatePublished)
            .ToArrayAsync(),

        IsImage = true,

        IsPartsMoreOne = false,

        PageHeadTitle = "<h1 class=\"fs-4 text_shadow\">ЗНАКОМЫЕ О<br /><span class=\"fs-2\">СЕРГЕЕ ШЕВКУНЕНКО</span></h1>"
    });

    [Route("video-na-saite/lihie-90-e")]
    [ActionName("lihie-90-e")]
    public async Task<ViewResult> Lihie90() =>
        View("Index", new MoviesListViewModel
        {
            Movies = await _movieContext.MovieFiles
                .AsNoTracking()
                .Where(p => p.SearchFilter.Contains("Программа «Лихие 90-е»,"))
                .OrderBy(p => p.MovieDatePublished)
                .ToArrayAsync(),

            IsImage = true,

            IconType = "webicon300",

            IsPartsMoreOne = false,

            AllMoviesFromDB = true,

            PageHeadTitle = "<h1 class=\"fs-2 text_shadow\">ПРОГРАММА «ЛИХИЕ 90-е»</h1>"
        });

    [Route("video-na-saite/bbc-top-100-us-movies")]
    [ActionName("bbc-top-100-us-movies")]
    public async Task<ViewResult> BbcTop100() =>
        View("Index", new MoviesListViewModel
        {
            Movies = await _movieContext.MovieFiles
                .AsNoTracking()
                .Where(p => p.SearchFilter.Contains("BBC Top 100 US,"))
                .OrderBy(p => p.MovieUploadDate)
                .ToArrayAsync(),
            IsImage = false,
            IconType = "webicon300",
            IsPartsMoreOne = true,
            AllMoviesFromDB = false,
            PageHeadTitle = "<h1 class=\"fs-2 text_shadow\">The&nbsp;100&nbsp;greatest American&nbsp;films&nbsp;by&nbsp;BBC</h1>"
        });

    public async Task<ViewResult> Movie(Guid? movieId, string? videoHosting, Guid? imageID, bool? pageOfSeries = false, string? topic = null)
    {
        if ((movieId == null & topic == null) || (topic != null & !await _movieContext.MovieFiles.Where(p => p.SearchFilter.Contains(topic + ",")).AnyAsync()))
        {
            Redirect("~/AllVideo/Index");
        }

        if (topic != null)
        {
            return View("Topic", new MoviesListViewModel
            {
                Movies = await _movieContext.MovieFiles
                .AsNoTracking()
                .Where(p => p.SearchFilter.Contains(topic + ","))
                .OrderBy(p => p.MovieUploadDate)
                .ToArrayAsync(),
                IsImage = null,
                IconType = "webicon300",
                IsPartsMoreOne = true,
                AllMoviesFromDB = false,
                PageHeadTitle = topic
            });
        }
        else if (await _movieContext.MovieFiles.Where(m => m.MovieFileModelId == movieId).AnyAsync())
        {
            MovieFileModel movieItem = await _movieContext.MovieFiles.FirstAsync(m => m.MovieFileModelId == movieId);

            if (pageOfSeries == true && movieItem.MovieTotalParts > 1 && !string.IsNullOrEmpty(movieItem.SeriesSearchFilter))
            {
                MovieFileModel[] allSeries = await _movieContext.MovieFiles.Where(m => m.SeriesSearchFilter == movieItem.SeriesSearchFilter).ToArrayAsync();

                if (allSeries.Length > 0)
                {
                    allSeries = allSeries.OrderBy(allSeries => allSeries.MoviePart).ToArray();
                }

                return View("Series", new SeriesViewModel
                {
                    AllSeriesMovies = allSeries,
                    HeadImageSeries = allSeries[0].ImageForHeadSeriesImageFileModelId,
                    IsImage = true,
                    IconType = "webicon300",
                    IsPartsMoreOne = false,
                    MovieInMainList = false,
                    HeadTitleForVideoLinks = string.Empty
                });
            }
            else if (imageID == null)
            {
                MovieFileModel? fullMovie = null;

                if (movieItem.FullMovieID != null)
                {
                    fullMovie = await _movieContext.MovieFiles.AsNoTracking().FirstAsync(m => m.MovieFileModelId == movieItem.FullMovieID);
                }
                else
                {
                    fullMovie = null;
                }

                string queryString = HttpContext.Request.QueryString.ToString();

                bool sergeyshefRu = false;

                Uri? videoRef;

                string? youtubeImageBorder, vkImageBorder, okImageBorder, mailruImageBorder, sergeyshefImageBorder = null;

                if (!string.IsNullOrEmpty(videoHosting))
                {
                    if (videoHosting.Contains("vk.com"))
                    {
                        videoRef = movieItem.MovieVkVideo;

                        youtubeImageBorder = null;
                        vkImageBorder = "setimage_border";
                        okImageBorder = null;
                        mailruImageBorder = null;
                        sergeyshefImageBorder = null;
                    }
                    else if (videoHosting.Contains("mail.ru"))
                    {
                        videoRef = movieItem.MovieMailRuVideo;

                        youtubeImageBorder = null;
                        vkImageBorder = null;
                        okImageBorder = null;
                        mailruImageBorder = "setimage_border";
                        sergeyshefImageBorder = null;
                    }
                    else if (videoHosting.Contains("ok.ru"))
                    {
                        videoRef = movieItem.MovieOkVideo;

                        youtubeImageBorder = null;
                        vkImageBorder = null;
                        okImageBorder = "setimage_border";
                        mailruImageBorder = null;
                        sergeyshefImageBorder = null;
                    }
                    else if (videoHosting.Contains("youtube.com"))
                    {
                        videoRef = movieItem.MovieYouTube;

                        youtubeImageBorder = "setimage_border";
                        vkImageBorder = null;
                        okImageBorder = null;
                        mailruImageBorder = null;
                        sergeyshefImageBorder = null;
                    }
                    else
                    {
                        videoRef = movieItem.MovieContentUrl;

                        youtubeImageBorder = null;
                        vkImageBorder = null;
                        okImageBorder = null;
                        mailruImageBorder = null;
                        sergeyshefImageBorder = "setimage_border";

                        sergeyshefRu = true;
                    }
                }
                else
                {
                    videoRef = movieItem.MovieYouTube;

                    youtubeImageBorder = "setimage_border";
                    vkImageBorder = null;
                    okImageBorder = null;
                    mailruImageBorder = null;
                    sergeyshefImageBorder = null;
                }

                return View("Movie", new MoviePageViewModel
                {
                    MovieFile = movieItem,

                    FullMovie = fullMovie,

                    SergeyshefRu = sergeyshefRu,

                    VideoRef = videoRef!,

                    YoutubeImageBorder = youtubeImageBorder,

                    OkImageBorder = okImageBorder,

                    MailruImageBorder = mailruImageBorder,

                    VkImageBorder = vkImageBorder,

                    SergeyshefBorder = sergeyshefImageBorder
                });
            }
            else if (imageID != null)
            {
                ImageFileModel imageItem = await _imageContext.ImageFiles.FirstAsync(p => p.ImageFileModelId == imageID);

                ImageFileModel[] imageArray = Array.Empty<ImageFileModel>();

                if (movieItem.MovieCaption == "Пропавшая экспедиция - 1" || movieItem.MovieCaption == "Пропавшая экспедиция - 2")
                {
                    imageArray = await _imageContext.ImageFiles.Where(p => p.SearchFilter.Contains("Пропавшая экспедиция - альбом,")).ToArrayAsync();
                }
                else if (movieItem.MovieCaption.Contains("Интервью"))
                {
                    imageArray = await _imageContext.ImageFiles.Where(p => p.SearchFilter.Contains("Криминальная звезда,")).ToArrayAsync();
                }
                else
                {
                    imageArray = await _imageContext.ImageFiles.Where(p => p.SearchFilter.Contains(movieItem.MovieCaption ?? string.Empty)).ToArrayAsync();
                }

                return View("Frames", new FramesViewModel
                {
                    MovieFile = movieItem,
                    ImageArray = imageArray,
                    ImageFile = imageItem
                });
            }
            else
            {
                return View("NoMovie");
            }
        }
        else
        {
            return View("NoMovie");
        }
    }

    #endregion
}