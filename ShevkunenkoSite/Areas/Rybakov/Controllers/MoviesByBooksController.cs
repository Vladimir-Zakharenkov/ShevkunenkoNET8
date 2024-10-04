namespace ShevkunenkoSite.Areas.Rybakov.Controllers;

[Area("Rybakov")]
public class MoviesByBooksController(
    IMovieFileRepository movieContext, 
    IImageFileRepository imageContext
    ) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<ViewResult> Movie(Guid movieId, string? videoHosting, Guid? imageID)
    {
        if (await movieContext.MovieFiles.Where(m => m.MovieFileModelId == movieId).AnyAsync() & imageID == null)
        {
            MovieFileModel movieItem = await movieContext.MovieFiles.FirstAsync(m => m.MovieFileModelId == movieId);

            MovieFileModel? fullMovie = null;

            if (movieItem.FullMovieID != null)
            {
                fullMovie = await movieContext.MovieFiles.AsNoTracking().FirstAsync(m => m.MovieFileModelId == movieItem.FullMovieID);
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
        else if (await movieContext.MovieFiles.Where(m => m.MovieFileModelId == movieId).AnyAsync() & imageID != null)
        {
            MovieFileModel movieItem = await movieContext.MovieFiles.FirstAsync(m => m.MovieFileModelId == movieId);

            ImageFileModel imageItem = await imageContext.ImageFiles.FirstAsync(p => p.ImageFileModelId == imageID);

            ImageFileModel[] imageArray = [];

            if (movieItem.MovieCaption == "Кортик - 1" || movieItem.MovieCaption == "Кортик - 2" || movieItem.MovieCaption == "Кортик - 3")
            {
                imageArray = await imageContext.ImageFiles.Where(p => p.SearchFilter.Contains("Кортик-альбом,")).ToArrayAsync();
            }
            else if (movieItem.MovieCaption == "Бронзовая птица - 1" || movieItem.MovieCaption == "Бронзовая птица - 2" || movieItem.MovieCaption == "Бронзовая птица - 3")
            {
                imageArray = await imageContext.ImageFiles.Where(p => p.SearchFilter.Contains("Бронзовая птица - альбом,")).ToArrayAsync();
            }
            else
            {
                imageArray = await imageContext.ImageFiles.Where(p => p.SearchFilter.Contains(movieItem.MovieCaption ?? string.Empty)).ToArrayAsync();
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