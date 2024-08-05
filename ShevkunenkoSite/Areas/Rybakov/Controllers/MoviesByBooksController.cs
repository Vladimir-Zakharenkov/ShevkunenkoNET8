namespace ShevkunenkoSite.Areas.Rybakov.Controllers;

[Area("Rybakov")]
public class MoviesByBooksController : Controller
{
    private readonly IMovieFileRepository _movieContext;
    private readonly IImageFileRepository _imageContext;
    public MoviesByBooksController(IMovieFileRepository movieContext, IImageFileRepository imageContext)
    {
        _movieContext = movieContext;
        _imageContext = imageContext;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Kortik()
    {
        return View();
    }

    [ActionName("Bronzovaya-ptica")]
    public IActionResult Bronzovaya_ptica()
    {
        return View();
    }

    public async Task<ViewResult> Movie(Guid movieId, string? videoHosting, Guid? imageID)
    {
        if (await _movieContext.MovieFiles.Where(m => m.MovieFileModelId == movieId).AnyAsync() & imageID == null)
        {
            MovieFileModel movieItem = await _movieContext.MovieFiles.FirstAsync(m => m.MovieFileModelId == movieId);

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
                if (videoHosting.Contains("vk"))
                {
                    videoRef = movieItem.MovieVkVideo;

                    youtubeImageBorder = null;
                    vkImageBorder = "setimage_border";
                    okImageBorder = null;
                    mailruImageBorder = null;
                    sergeyshefImageBorder = null;
                }
                else if (videoHosting.Contains("mail"))
                {
                    videoRef = movieItem.MovieMailRuVideo;

                    youtubeImageBorder = null;
                    vkImageBorder = null;
                    okImageBorder = null;
                    mailruImageBorder = "setimage_border";
                    sergeyshefImageBorder = null;
                }
                else if (videoHosting.Contains("ok"))
                {
                    videoRef = movieItem.MovieOkVideo;

                    youtubeImageBorder = null;
                    vkImageBorder = null;
                    okImageBorder = "setimage_border";
                    mailruImageBorder = null;
                    sergeyshefImageBorder = null;
                }
                else if (videoHosting.Contains("youtube"))
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
        else if (await _movieContext.MovieFiles.Where(m => m.MovieFileModelId == movieId).AnyAsync() & imageID != null)
        {
            MovieFileModel movieItem = await _movieContext.MovieFiles.FirstAsync(m => m.MovieFileModelId == movieId);

            ImageFileModel imageItem = await _imageContext.ImageFiles.FirstAsync(p => p.ImageFileModelId == imageID);

            ImageFileModel[] imageArray = Array.Empty<ImageFileModel>();

            if (movieItem.MovieCaption == "Кортик - 1" || movieItem.MovieCaption == "Кортик - 2" || movieItem.MovieCaption == "Кортик - 3")
            {
                imageArray = await _imageContext.ImageFiles.Where(p => p.SearchFilter.Contains("Кортик-альбом,")).ToArrayAsync();
            }
            else if (movieItem.MovieCaption == "Бронзовая птица - 1" || movieItem.MovieCaption == "Бронзовая птица - 2" || movieItem.MovieCaption == "Бронзовая птица - 3")
            {
                imageArray = await _imageContext.ImageFiles.Where(p => p.SearchFilter.Contains("Бронзовая птица - альбом,")).ToArrayAsync();
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
}