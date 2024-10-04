//Ignore Spelling: Org
namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class MovieInfoController(
    IMovieFileRepository movieInfoContext,
    IPageInfoRepository pageInfoContext,
    IImageFileRepository imageContext,
    ITopicMovieRepository topicMovieContext
    ) : Controller
{
    PageInfoModel? pageForSeries = new();

    //PageInfoModel? pageForMovie = new();

    MovieFileModel? fullMovie = new();

    ImageFileModel? posterForMovie = new();

    #region Список фильмов на сайте

    public int moviesPerPage = 12;

    public async Task<ViewResult> Index(
        string? movieCaptionSearchString,
        string? movieDescriptionForSchemaOrgSearchString,
        string? movieGenreSearchString,
        string? movieDirectorSearchString,
        string? movieMusicBySearchString,
        string? movieActorSearchString,
        int pageNumber = 1)
    {
        var allMovies = from m in movieInfoContext.MovieFiles
                        .Where(s => s.MovieCaption.Contains((movieCaptionSearchString ?? string.Empty).Trim()))
                        .Where(s => s.MovieDescriptionForSchemaOrg.Contains((movieDescriptionForSchemaOrgSearchString ?? string.Empty).Trim()))
                        .Where(s => s.MovieGenre.Contains((movieGenreSearchString ?? string.Empty).Trim()))
                        .Where(s => s.MovieDirector1.Contains((movieDirectorSearchString ?? string.Empty).Trim()))
                        .Where(s => s.MovieMusicBy.Contains((movieMusicBySearchString ?? string.Empty).Trim()))
                        .Where(s => s.MovieActor01.Contains((movieActorSearchString ?? string.Empty).Trim())
                            || s.MovieActor02.Contains((movieActorSearchString ?? string.Empty).Trim())
                            || s.MovieActor03.Contains((movieActorSearchString ?? string.Empty).Trim())
                            || s.MovieActor04.Contains((movieActorSearchString ?? string.Empty).Trim())
                            || s.MovieActor05.Contains((movieActorSearchString ?? string.Empty).Trim())
                            || s.MovieActor06.Contains((movieActorSearchString ?? string.Empty).Trim())
                            || s.MovieActor07.Contains((movieActorSearchString ?? string.Empty).Trim())
                            || s.MovieActor08.Contains((movieActorSearchString ?? string.Empty).Trim())
                            || s.MovieActor09.Contains((movieActorSearchString ?? string.Empty).Trim())
                            || s.MovieActor10.Contains((movieActorSearchString ?? string.Empty).Trim()))
                        select m;


        return View(new MoviesListViewModel
        {
            Movies = await allMovies
                .Skip((pageNumber - 1) * moviesPerPage)
                .Take(moviesPerPage)
                .ToArrayAsync(),

            IsPartsMoreOne = false,

            PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = pageNumber,
                ItemsPerPage = moviesPerPage,
                TotalItems = allMovies.Count(),
            },

            MovieCaptionSearchString = movieCaptionSearchString ?? string.Empty,
            MovieDescriptionForSchemaOrgSearchString = movieDescriptionForSchemaOrgSearchString ?? string.Empty,
            MovieGenreSearchString = movieGenreSearchString ?? string.Empty,
            MovieDirectorSearchString = movieDirectorSearchString ?? string.Empty,
            MovieMusicBySearchString = movieMusicBySearchString ?? string.Empty,
            MovieActorSearchString = movieActorSearchString ?? string.Empty
        });
    }

    #endregion

    #region Информация о фильме

    public async Task<IActionResult> DetailsMovie(Guid? movieId)
    {
        if (movieId.HasValue && await movieInfoContext.MovieFiles.Where(p => p.MovieFileModelId == movieId).AnyAsync())
        {
            MovieFileModel movieItem = await movieInfoContext.MovieFiles
                .Include(img => img.ImageFileModel)
                .Include(img => img.ImageForHeadSeries)
                .AsNoTracking()
                .FirstAsync(p => p.MovieFileModelId == movieId);

            #region Страница серий многосерийного фильма

            if (movieItem.PageInfoModelIdForSeries != Guid.Empty && movieItem.PageInfoModelIdForSeries != null && await pageInfoContext.PagesInfo.Where(p => p.PageInfoModelId == movieItem.PageInfoModelIdForSeries).AnyAsync())
            {
                pageForSeries = await pageInfoContext.PagesInfo.FirstAsync(p => p.PageInfoModelId == movieItem.PageInfoModelIdForSeries);
            }

            #endregion

            #region Полная версия фильма

            if (movieItem.FullMovieID != null && await movieInfoContext.MovieFiles.Where(p => p.MovieFileModelId == movieItem.FullMovieID).AnyAsync())
            {
                fullMovie = await movieInfoContext.MovieFiles
                    .AsNoTracking()
                    .FirstAsync(p => p.MovieFileModelId == movieItem.FullMovieID);
            }
            else
            {
                fullMovie = null;
            }

            #endregion

            #region Постер для фильма

            if (movieItem.MoviePosterGuid != null && await imageContext.ImageFiles.Where(img => img.ImageFileModelId == movieItem.MoviePosterGuid).AnyAsync())
            {
                posterForMovie = await imageContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == movieItem.MoviePosterGuid);
            }
            else if (!string.IsNullOrEmpty(movieItem.MoviePoster) && await imageContext.ImageFiles.Where(img => img.WebImageFileName == movieItem.MoviePoster || img.ImageFileName == movieItem.MoviePoster).AnyAsync())
            {
                posterForMovie = await imageContext.ImageFiles.FirstAsync(img => img.WebImageFileName == movieItem.MoviePoster || img.ImageFileName == movieItem.MoviePoster);
            }
            else
            {
                posterForMovie = null;
            }

            #endregion

            #region Фильтры поиска

            var searchFilters = movieItem.SearchFilter.Split(',').Where(val => val != string.Empty).ToArray();

            #endregion

            #region Список тем

            var topicFilters = movieItem.TopicGuidList.Split(',').Where(val => val != string.Empty).ToArray();

            if (topicFilters.Length > 0)
            {
                for (int i = 0; i < topicFilters.Length; i++)
                {
                    if (Guid.TryParse(topicFilters[i], out var guidOutput))
                    {
                        var topic = await topicMovieContext.TopicMovies.Where(t => t.TopicMovieModelId == guidOutput).FirstAsync();

                        topicFilters[i] = topicFilters[i] + " - " + topic.TopicDescription;
                    }
                }
            }

            #endregion

            return View(new DetailsMovieViewModel
            {
                MovieItem = movieItem,
                PageForSeries = pageForSeries,
                FullMovie = fullMovie,
                PosterForMovie = posterForMovie,
                SearchFilters = searchFilters ?? [],
                TopicFilters = topicFilters ?? []
            });
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion

    #region Добавить фильм в базу данных

    [HttpGet]
    public ViewResult AddMovie() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [DisableRequestSizeLimit]
    [RequestSizeLimit(5_268_435_456)]
    [RequestFormLimits(MultipartBodyLengthLimit = 5268435456)]
    public async Task<IActionResult> AddMovie(AddMovieViewModel movieItem)
    {
        if (ModelState.IsValid)
        {
            #region Добавить картинку для фильма

            if (movieItem.ImageForMovieFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == movieItem.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == movieItem.ImageForMovieFormFile.FileName);

                    movieItem.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImageForMovieFormFile", $"Добавьте картинку «{movieItem.ImageForMovieFormFile.FileName}» в базу данных");

                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("ImageForMovieFormFile", $"Выберите картинку");

                return View();
            }

            #endregion

            #region Добавить постер для фильма

            if (movieItem.PosterForMovieFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == movieItem.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == movieItem.PosterForMovieFormFile.FileName);

                    movieItem.MoviePoster = posterFile.ImageFileName;
                    movieItem.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("PosterForMovieFormFile", $"Добавьте картинку постера «{movieItem.PosterForMovieFormFile.FileName}» в базу данных");

                    return View();
                }
            }
            else
            {
                var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileModelId == movieItem.ImageFileModelId);

                movieItem.MoviePoster = posterFile.ImageFileName;
                movieItem.MoviePosterGuid = movieItem.ImageFileModelId;
            }

            #endregion

            #region Добавить ссылку на страницу фильма

            if (!string.IsNullOrEmpty(movieItem.PageForMovie))
            {
                movieItem.PageForMovie = "/" + movieItem.PageForMovie.Trim().Trim('/');

                if (!await pageInfoContext.PagesInfo.Where(p => p.PageFullPath == movieItem.PageForMovie).AnyAsync())
                {
                    ModelState.AddModelError("PageForMovie", "Указанной страницы нет в базе данных");

                    return View();
                }

                PageInfoModel pageInfo = await pageInfoContext.PagesInfo.FirstAsync(p => p.PageFullPath == movieItem.PageForMovie);

                if (await movieInfoContext.MovieFiles.Where(m => m.PageInfoModelId == pageInfo.PageInfoModelId).AnyAsync())
                {
                    MovieFileModel movieSet = await movieInfoContext.MovieFiles.FirstAsync(m => m.PageInfoModelId == pageInfo.PageInfoModelId);

                    ModelState.AddModelError("PageForMovie", $"Указанная страница связана с фильмом {movieSet.MovieCaption}");

                    return View();
                }

                movieItem.PageInfoModelId = pageInfo.PageInfoModelId;
            }
            else
            {
                movieItem.PageInfoModelId = null;
            }

            #endregion

            #region Добавить информацию о сериях

            #region Добавить картинку заголовка серий

            if (movieItem.ImageHeadForSeriesFormFile != null && movieItem.MovieTotalParts > 1 && movieItem.MoviePart == 1)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == movieItem.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == movieItem.ImageHeadForSeriesFormFile.FileName);

                    movieItem.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImageHeadForSeriesFormFile", $"Добавьте картинку серий «{movieItem.ImageHeadForSeriesFormFile.FileName}» в базу данных");

                    return View();
                }
            }
            else
            {
                movieItem.ImageForHeadSeriesId = null;
            }

            #endregion

            #region Добавить ссылку на страницу серий фильма

            if (movieItem.MovieTotalParts > 1 && !string.IsNullOrEmpty(movieItem.PageForSeries))
            {
                movieItem.PageForSeries = "/" + movieItem.PageForSeries.Trim().Trim('/');

                if (!await pageInfoContext.PagesInfo.Where(p => p.PageFullPath == movieItem.PageForSeries).AnyAsync())
                {
                    ModelState.AddModelError("PageForSeries", "Указанной страницы нет в базе данных");

                    return View();
                }

                pageForSeries = await pageInfoContext.PagesInfo.FirstAsync(p => p.PageFullPath == movieItem.PageForSeries);

                movieItem.PageInfoModelIdForSeries = pageForSeries.PageInfoModelId;
            }
            else
            {
                movieItem.PageInfoModelIdForSeries = Guid.Empty;
            }

            #endregion

            movieItem.SeriesSearchFilter = movieItem.SeriesSearchFilter.Trim();

            #endregion

            #region Добавить файл фильма

            if (movieItem.FileForMovieFormFile != null)
            {
                if (!movieItem.FileForMovieFormFile.FileName.EndsWith(".mp4"))
                {
                    ModelState.AddModelError("FileForMovieFormFile", "Формат фильмов на сайте «mp4»");

                    return View();
                }

                if (await movieInfoContext.MovieFiles.Where(c => c.MovieFileName == movieItem.FileForMovieFormFile.FileName).AnyAsync())
                {
                    ModelState.AddModelError("FileForMovieFormFile", $"Файл {movieItem.FileForMovieFormFile.FileName} уже есть в базе данных"); ;

                    return View();
                }

                string path = Path.Combine(DataConfig.MovieFoldersPath, movieItem.FileForMovieFormFile.FileName);

                if (!System.IO.File.Exists(path))
                {
                    using var stream = new FileStream(path, FileMode.Create);
                    await movieItem.FileForMovieFormFile.CopyToAsync(stream);
                }

                IReadOnlyList<MetadataExtractor.Directory> movieDirectories = ImageMetadataReader.ReadMetadata(path);

                foreach (var movieDirectory in movieDirectories)
                {
                    foreach (var tag in movieDirectory.Tags)
                    {
                        if (movieDirectory.Name == "QuickTime Movie Header" && tag.Name == "Duration")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("movieItem.MovieDuration", "Продолжительность фильма равна 0");

                                return View();
                            }
                            else
                            {
                                movieItem.MovieDuration = TimeSpan.Parse(tag.Description);
                            }
                        }

                        if (movieDirectory.Name == "QuickTime Track Header" && tag.Name == "Width" && Convert.ToUInt32(tag.Description) > 0)
                        {
                            if (string.IsNullOrEmpty(tag.Description) && !(Convert.ToUInt32(tag.Description) > 0))
                            {
                                ModelState.AddModelError("movieItem.MovieWidth", "Ширина кадра равна 0");

                                return View();
                            }
                            else
                            {
                                movieItem.MovieWidth = Convert.ToUInt32(tag.Description);
                            }
                        }

                        if (movieDirectory.Name == "QuickTime Track Header" && tag.Name == "Height" && Convert.ToUInt32(tag.Description) > 0)
                        {
                            movieItem.MovieHeight = Convert.ToUInt32(tag.Description);

                            if (movieItem.MovieHeight == 0)
                            {
                                ModelState.AddModelError("movieItem.MovieHeight", "Высота кадра равна 0");

                                return View();
                            }
                        }

                        if (movieDirectory.Name == "File" && tag.Name == "File Name")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("movieItem.MovieFileName", "Название файла не определено");

                                return View();
                            }
                            else
                            {
                                movieItem.MovieFileName = tag.Description;
                            }
                        }

                        if (movieDirectory.Name == "File Type" && tag.Name == "Expected File Name Extension")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("movieItem.MovieFileExtension", "Расширение файла не определено");

                                return View();
                            }
                            else
                            {
                                movieItem.MovieFileExtension = tag.Description;
                            }
                        }

                        if (movieDirectory.Name == "File Type" && tag.Name == "Detected MIME Type")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("movieItem.MovieMimeType", "MIME/TYPE файла не определен");

                                return View();
                            }
                            else
                            {
                                movieItem.MovieMimeType = tag.Description;
                            }
                        }

                        if (movieDirectory.Name == "File" && tag.Name == "File Size")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("movieItem.MovieFileSize", "Размер файла равен 0");

                                return View();
                            }
                            else
                            {
                                movieItem.MovieFileSize = Convert.ToUInt64(tag.Description[..tag.Description.ToString().IndexOf(' ')]);
                            }
                        }
                    }
                }
            }
            else
            {
                movieItem.MovieDuration = TimeSpan.Zero;
                movieItem.MovieWidth = 0;
                movieItem.MovieHeight = 0;
                movieItem.MovieFileName = string.Empty;
                movieItem.MovieFileExtension = string.Empty;
                movieItem.MovieMimeType = string.Empty;
                movieItem.MovieFileSize = 0;
                movieItem.MovieContentUrl = null;
            }

            #endregion

            #region Добавить ссылку на полный вариант фильма

            if (movieItem.FileForMovieFormFile?.FileName == movieItem.FullMovieFormFile?.FileName)
            {
                ModelState.AddModelError("FullMovieFormFile", $"Выбран {movieItem.FileForMovieFormFile?.FileName} один файл для отрывка и полного фильма."); ;

                return View();

            }

            if (movieItem.FullMovieFormFile != null)
            {
                if (await movieInfoContext.MovieFiles.Where(mov => mov.MovieFileName == movieItem.FullMovieFormFile.FileName).AnyAsync())
                {
                    fullMovie = await movieInfoContext.MovieFiles.FirstAsync(mov => mov.MovieFileName == movieItem.FullMovieFormFile.FileName);

                    movieItem.FullMovieID = fullMovie.MovieFileModelId;
                }
                else
                {
                    ModelState.AddModelError("FullMovieFormFile", $"Файл фильма {movieItem.FullMovieFormFile.FileName} не найден в базе данных."); ;

                    return View();
                }
            }
            else
            {
                movieItem.FullMovieID = null;
                fullMovie = null;
            }

            #endregion

            #region Название фильма

            movieItem.MovieCaption = movieItem.MovieCaption.Trim();

            if (await movieInfoContext.MovieFiles.Where(c => c.MovieCaption == movieItem.MovieCaption & c.MovieTotalParts == movieItem.MovieTotalParts & c.MoviePart == movieItem.MoviePart).AnyAsync())
            {
                ModelState.AddModelError("movieItem.MovieCaption", $"Фильм «{movieItem.MovieCaption}» количество серий - {movieItem.MovieTotalParts} серия {movieItem.MoviePart} уже существует");

                return View();
            }

            #endregion

            #region Добавить ссылки на видео хостинги

            movieItem.MovieContentUrl = new Uri("https://sergeyshef.ru/video/" + movieItem.MovieFileName);
            movieItem.MovieYouTube = movieItem.MovieYouTube;
            movieItem.MovieVkVideo = movieItem.MovieVkVideo;
            movieItem.MovieMailRuVideo = movieItem.MovieMailRuVideo;
            movieItem.MovieOkVideo = movieItem.MovieOkVideo;
            movieItem.MovieYandexDiskVideo = movieItem.MovieYandexDiskVideo;

            #endregion

            #region Добавить блок ссылок под фильмом

            #region Блок1

            movieItem.HeadTitleForVideoLinks1 = movieItem.HeadTitleForVideoLinks1.Trim();
            movieItem.SearchFilter1 = movieItem.SearchFilter1.Trim();
            movieItem.IconType1 = movieItem.IconType1.Trim();

            #endregion

            #region Блок2

            movieItem.HeadTitleForVideoLinks2 = movieItem.HeadTitleForVideoLinks2.Trim();
            movieItem.SearchFilter2 = movieItem.SearchFilter2.Trim();
            //movieItem.IsImage2 = movieItem.IsImage2;
            movieItem.IconType2 = movieItem.IconType2.Trim();
            //movieItem.isPartsMoreOne2 = movieItem.isPartsMoreOne2;
            //movieItem.AllMoviesFromDB2 = movieItem.AllMoviesFromDB2;

            #endregion

            #region Блок3

            movieItem.HeadTitleForVideoLinks3 = movieItem.HeadTitleForVideoLinks3.Trim();
            movieItem.SearchFilter3 = movieItem.SearchFilter3.Trim();
            //movieItem.IsImage3 = movieItem.IsImage3;
            movieItem.IconType3 = movieItem.IconType3.Trim();
            //movieItem.isPartsMoreOne3 = movieItem.isPartsMoreOne3;
            //movieItem.AllMoviesFromDB3 = movieItem.AllMoviesFromDB3;

            #endregion

            #endregion

            #region Остальные данные фильма

            movieItem.MovieDescriptionForSchemaOrg = movieItem.MovieDescriptionForSchemaOrg.Trim();
            movieItem.MovieDescriptionHtml = movieItem.MovieDescriptionHtml.Trim();
            movieItem.MovieCaptionForOnline = movieItem.MovieCaptionForOnline.Trim();
            movieItem.MovieNote = movieItem.MovieNote.Trim();
            movieItem.MovieGenre = movieItem.MovieGenre.Trim();
            movieItem.MovieРroductionCompany = movieItem.MovieРroductionCompany.Trim();
            movieItem.MovieDirector1 = movieItem.MovieDirector1.Trim();
            movieItem.MovieDirector2 = movieItem.MovieDirector2.Trim();
            movieItem.MovieMusicBy = movieItem.MovieMusicBy.Trim();
            movieItem.MovieActor01 = movieItem.MovieActor01.Trim();
            movieItem.MovieActor02 = movieItem.MovieActor02.Trim();
            movieItem.MovieActor03 = movieItem.MovieActor03.Trim();
            movieItem.MovieActor04 = movieItem.MovieActor04.Trim();
            movieItem.MovieActor05 = movieItem.MovieActor05.Trim();
            movieItem.MovieActor06 = movieItem.MovieActor06.Trim();
            movieItem.MovieActor07 = movieItem.MovieActor07.Trim();
            movieItem.MovieActor08 = movieItem.MovieActor08.Trim();
            movieItem.MovieActor09 = movieItem.MovieActor09.Trim();
            movieItem.MovieActor10 = movieItem.MovieActor10.Trim();

            if (movieItem.SearchFilter != null)
            {
                movieItem.SearchFilter = movieItem.SearchFilter.Trim();
            }
            else
            {
                movieItem.SearchFilter = string.Empty;
            }

            if (movieItem.FramesAroundMovie != string.Empty)
            {
                if (!await movieInfoContext.MovieFiles.Where(p => p.MovieCaption == movieItem.FramesAroundMovie.Trim()).AnyAsync())
                {
                    ModelState.AddModelError("FramesAroundMovie", $"Фильма «{movieItem.FramesAroundMovie}»  нет в базе данных");

                    return View();
                }

                movieItem.FramesAroundMovie = movieItem.FramesAroundMovie.Trim();
            }
            else
            {
                movieItem.FramesAroundMovie = string.Empty;
            }

            if (movieItem.MovieAdult == true)
            {
                movieItem.MovieIsFamilyFriendly = false;
            }
            else
            {
                movieItem.MovieIsFamilyFriendly = true;
            }

            #endregion

            await movieInfoContext.AddNewMovieAsync(movieItem);

            var newMovie = await movieInfoContext.MovieFiles.FirstAsync(mov => mov.MovieCaption == movieItem.MovieCaption);

            return RedirectToAction("DetailsMovie", new { movieId = newMovie.MovieFileModelId, Area = "Admin" });
        }
        else
        {
            return View();
        }
    }

    #endregion

    #region Изменить данные фильма

    [HttpGet]
    public async Task<IActionResult> EditMovie(Guid? movieId)
    {
        if (movieId.HasValue && await movieInfoContext.MovieFiles.Where(i => i.MovieFileModelId == movieId).AnyAsync())
        {
            var movieItem = await movieInfoContext.MovieFiles.Include(img => img.ImageFileModel).Include(img => img.ImageForHeadSeries).FirstAsync(i => i.MovieFileModelId == movieId);

            #region Текущие темы для фильма

            var topicFilters = movieItem.TopicGuidList.Split(',').Where(val => val != string.Empty).ToArray();

            if (topicFilters.Length > 0)
            {
                for (int i = 0; i < topicFilters.Length; i++)
                {
                    if (Guid.TryParse(topicFilters[i], out var guidOutput))
                    {
                        var topic = await topicMovieContext.TopicMovies.Where(t => t.TopicMovieModelId == guidOutput).FirstAsync();

                        topicFilters[i] = topic.TopicDescription;
                    }
                }
            }

            #endregion

            EditMovieViewModel editMovie = new()
            {
                // редактируемый фильм
                MovieItem = movieItem,

                // все темы фильмов
                TopicsForMovie = [.. topicMovieContext.TopicMovies],

                // текущие темы для фильма (string[])
                TopicFilters = topicFilters
            };

            // адрес страницы фильма
            if (editMovie.MovieItem.PageInfoModelId != null
                    && editMovie.MovieItem.PageInfoModelId != Guid.Empty
                    && await pageInfoContext.PagesInfo.Where(i => i.PageInfoModelId == editMovie.MovieItem.PageInfoModelId).AnyAsync())
            {
                var pageForMovie = await pageInfoContext.PagesInfo.FirstAsync(i => i.PageInfoModelId == editMovie.MovieItem.PageInfoModelId);

                editMovie.PageForMovie = pageForMovie.PageFullPathWithData;
            }
            else
            {
                editMovie.PageForMovie = string.Empty;
            }

            // адрес страницы серий фильма
            if (editMovie.MovieItem.PageInfoModelIdForSeries != null
                    && editMovie.MovieItem.PageInfoModelIdForSeries != Guid.Empty
                    && await pageInfoContext.PagesInfo.Where(i => i.PageInfoModelId == editMovie.MovieItem.PageInfoModelIdForSeries).AnyAsync())
            {
                editMovie.PageForSeries = await pageInfoContext.PagesInfo.FirstAsync(i => i.PageInfoModelId == editMovie.MovieItem.PageInfoModelIdForSeries);

                editMovie.PageForSeriesString = editMovie.PageForSeries.PageFullPathWithData;
            }
            else
            {
                editMovie.PageForSeries = null;
                editMovie.PageForSeriesString = string.Empty;
            }

            // полный вариант фильма
            if (editMovie.MovieItem?.FullMovieID != null
                    && await movieInfoContext.MovieFiles.Where(mov => mov.MovieFileModelId == editMovie.MovieItem.FullMovieID).AnyAsync())
            {
                editMovie.FullMovie = await movieInfoContext.MovieFiles.FirstAsync(mov => mov.MovieFileModelId == editMovie.MovieItem.FullMovieID);
            }
            else
            {
                editMovie.FullMovie = null;
            }

            // постер для фильма
            if (editMovie.MovieItem!.MoviePosterGuid != null
                    && await imageContext.ImageFiles.Where(img => img.ImageFileModelId == editMovie.MovieItem.MoviePosterGuid).AnyAsync())
            {
                editMovie.PosterForMovie = await imageContext.ImageFiles.FirstAsync(img => img.ImageFileModelId == editMovie.MovieItem.MoviePosterGuid);
            }
            else if (!string.IsNullOrEmpty(editMovie.MovieItem!.MoviePoster)
                    && await imageContext.ImageFiles.Where(img => img.WebImageFileName == editMovie.MovieItem.MoviePoster || img.ImageFileName == editMovie.MovieItem.MoviePoster).AnyAsync())
            {
                editMovie.PosterForMovie = await imageContext.ImageFiles.FirstAsync(img => img.WebImageFileName == editMovie.MovieItem.MoviePoster || img.ImageFileName == editMovie.MovieItem.MoviePoster);
            }
            else
            {
                editMovie.PosterForMovie = null;
            }

            return View(editMovie);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [DisableRequestSizeLimit]
    [RequestSizeLimit(5_268_435_456)]
    [RequestFormLimits(MultipartBodyLengthLimit = 5268435456)]
    public async Task<IActionResult> EditMovie(EditMovieViewModel editMovie)
    {
        if (ModelState.IsValid)
        {
            MovieFileModel movieUpdate = await movieInfoContext.MovieFiles.FirstAsync(i => i.MovieFileModelId == editMovie.MovieItem.MovieFileModelId);

            #region Файл фильма

            if (editMovie.FileForMovieFormFile != null)
            {
                if (!editMovie.FileForMovieFormFile.FileName.EndsWith(".mp4"))
                {
                    ModelState.AddModelError("FileForMovieFormFile", "Формат фильмов на сайте «mp4»");

                    return View(editMovie);
                }

                if (await movieInfoContext.MovieFiles.Where(c => c.MovieFileName == editMovie.FileForMovieFormFile.FileName).AnyAsync())
                {
                    ModelState.AddModelError("FileForMovieFormFile", $"Файл {editMovie.FileForMovieFormFile.FileName} уже есть в базе данных"); ;

                    return View(editMovie);
                }

                string path = Path.Combine(DataConfig.MovieFoldersPath, editMovie.FileForMovieFormFile.FileName);

                if (!System.IO.File.Exists(path))
                {
                    using var stream = new FileStream(path, FileMode.Create);
                    await editMovie.FileForMovieFormFile.CopyToAsync(stream);
                }

                #region Извлечь параметры видеофайла

                IReadOnlyList<MetadataExtractor.Directory> movieDirectories = ImageMetadataReader.ReadMetadata(path);

                foreach (var movieDirectory in movieDirectories)
                {
                    foreach (var tag in movieDirectory.Tags)
                    {
                        if (movieDirectory.Name == "QuickTime Movie Header" && tag.Name == "Duration")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("editMovie.MovieItem.MovieDuration", "Продолжительность фильма равна 0");

                                return View();
                            }
                            else
                            {
                                movieUpdate.MovieDuration = TimeSpan.Parse(tag.Description);
                            }
                        }

                        if (movieDirectory.Name == "QuickTime Track Header" && tag.Name == "Width" && Convert.ToUInt32(tag.Description) > 0)
                        {
                            if (string.IsNullOrEmpty(tag.Description) && !(Convert.ToUInt32(tag.Description) > 0))
                            {
                                ModelState.AddModelError("editMovie.MovieItem.MovieWidth", "Ширина кадра равна 0");

                                return View();
                            }
                            else
                            {
                                movieUpdate.MovieWidth = Convert.ToUInt32(tag.Description);
                            }
                        }

                        if (movieDirectory.Name == "QuickTime Track Header" && tag.Name == "Height" && Convert.ToUInt32(tag.Description) > 0)
                        {
                            movieUpdate.MovieHeight = Convert.ToUInt32(tag.Description);

                            if (movieUpdate.MovieHeight == 0)
                            {
                                ModelState.AddModelError("editMovie.MovieItem.MovieHeight", "Высота кадра равна 0");

                                return View();
                            }
                        }

                        if (movieDirectory.Name == "File" && tag.Name == "File Name")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("editMovie.MovieItem.MovieFileName", "Название файла не определено");

                                return View();
                            }
                            else
                            {
                                movieUpdate.MovieFileName = tag.Description;
                            }
                        }

                        if (movieDirectory.Name == "File Type" && tag.Name == "Expected File Name Extension")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("editMovie.MovieItem.MovieFileExtension", "Расширение файла не определено");

                                return View();
                            }
                            else
                            {
                                movieUpdate.MovieFileExtension = tag.Description;
                            }
                        }

                        if (movieDirectory.Name == "File Type" && tag.Name == "Detected MIME Type")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("editMovie.MovieItem.MovieMimeType", "MIME/TYPE файла не определен");

                                return View();
                            }
                            else
                            {
                                movieUpdate.MovieMimeType = tag.Description;
                            }
                        }

                        if (movieDirectory.Name == "File" && tag.Name == "File Size")
                        {
                            if (string.IsNullOrEmpty(tag.Description))
                            {
                                ModelState.AddModelError("editMovie.MovieItem.MovieFileSize", "Размер файла равен 0");

                                return View();
                            }
                            else
                            {
                                movieUpdate.MovieFileSize = Convert.ToUInt64(tag.Description[..tag.Description.ToString().IndexOf(' ')]);
                            }
                        }
                    }
                }

                #endregion
            }
            else
            {
                movieUpdate.MovieDuration = editMovie.MovieItem.MovieDuration;
                movieUpdate.MovieWidth = editMovie.MovieItem.MovieWidth;
                movieUpdate.MovieHeight = editMovie.MovieItem.MovieHeight;
                movieUpdate.MovieFileName = editMovie.MovieItem.MovieFileName;
                movieUpdate.MovieFileExtension = editMovie.MovieItem.MovieFileExtension;
                movieUpdate.MovieMimeType = editMovie.MovieItem.MovieMimeType;
                movieUpdate.MovieFileSize = editMovie.MovieItem.MovieFileSize;
            }



            #endregion

            #region Полная версия фильма

            if (editMovie.FullMovieFormFile != null)
            {
                if (editMovie.FileForMovieFormFile != null)
                {
                    if (editMovie.FullMovieFormFile.FileName == editMovie.FileForMovieFormFile.FileName | editMovie.FullMovieFormFile.FileName == editMovie.MovieItem.MovieFileName)
                    {
                        ModelState.AddModelError("FullMovieFormFile", $"Выбран {editMovie.FileForMovieFormFile?.FileName} одинаковый файл для отрывка и полного фильма."); ;

                        return View(editMovie);
                    }
                }
                else if (await movieInfoContext.MovieFiles.Where(mov => mov.MovieFileName == editMovie.FullMovieFormFile!.FileName).AnyAsync())
                {
                    var fullMovie = await movieInfoContext.MovieFiles.FirstAsync(mov => mov.MovieFileName == editMovie.FullMovieFormFile!.FileName);

                    movieUpdate.FullMovieID = fullMovie.MovieFileModelId;
                }
                else
                {
                    ModelState.AddModelError("FullMovieFormFile", $"Файл фильма {editMovie.FullMovieFormFile!.FileName} не найден в базе данных."); ;

                    return View(editMovie);
                }
            }
            else
            {
                movieUpdate.FullMovieID = movieUpdate.FullMovieID;
            }

            #endregion

            #region Страница фильма

            if (!string.IsNullOrEmpty(editMovie.PageForMovie.Trim()) && editMovie.PageForMovie.Trim() != "0")
            {
                editMovie.PageForMovie = "/" + editMovie.PageForMovie.Trim().Trim('/');

                if (!await pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == editMovie.PageForMovie.Trim()).AnyAsync()
                        & !await pageInfoContext.PagesInfo.Where(p => p.PagePathNickNameWithData == editMovie.PageForMovie.Trim()).AnyAsync())
                {
                    ModelState.AddModelError("PageForMovie", "Указанной страницы нет в базе данных");

                    return View(editMovie);
                }
                else if (await pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == editMovie.PageForMovie.Trim()).AnyAsync())
                {
                    var pageForMovie = await pageInfoContext.PagesInfo.FirstAsync(p => p.PageFullPathWithData == editMovie.PageForMovie.Trim());
                    movieUpdate.PageInfoModelId = pageForMovie.PageInfoModelId;
                }
                else if (await pageInfoContext.PagesInfo.Where(p => p.PagePathNickNameWithData == editMovie.PageForMovie.Trim()).AnyAsync())
                {
                    var pageForMovie = await pageInfoContext.PagesInfo.FirstAsync(p => p.PagePathNickNameWithData == editMovie.PageForMovie.Trim());
                    movieUpdate.PageInfoModelId = pageForMovie.PageInfoModelId;
                }
                else
                {
                    movieUpdate.PageInfoModelId = editMovie.MovieItem.PageInfoModelId;
                }
            }
            else if (editMovie.PageForMovie.Trim() == "0")
            {
                movieUpdate.PageInfoModelId = null;
            }
            else
            {
                movieUpdate.PageInfoModelId = editMovie.MovieItem.PageInfoModelId;
            }

            #endregion

            #region Изменить картинку для фильма

            if (editMovie.ImageForMovieFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageFileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == editMovie.ImageForMovieFormFile.FileName).AnyAsync())
                {
                    var imageFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == editMovie.ImageForMovieFormFile.FileName);

                    movieUpdate.ImageFileModelId = imageFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImageForMovieFormFile", $"Добавьте картинку «{editMovie.ImageForMovieFormFile.FileName}» в базу данных");

                    return View(editMovie);
                }
            }
            else
            {
                movieUpdate.ImageFileModelId = editMovie.MovieItem.ImageFileModelId;
            }

            #endregion

            #region Изменить постер для фильма

            if (editMovie.PosterForMovieFormFile != null)
            {
                if (await imageContext.ImageFiles.Where(i => i.ImageFileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == editMovie.PosterForMovieFormFile.FileName).AnyAsync())
                {
                    var posterFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == editMovie.PosterForMovieFormFile.FileName);

                    movieUpdate.MoviePoster = string.Empty;
                    movieUpdate.MoviePosterGuid = posterFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("PosterForMovieFormFile", $"Добавьте картинку постера «{editMovie.PosterForMovieFormFile.FileName}» в базу данных");

                    return View(editMovie);
                }
            }
            else
            {
                movieUpdate.MoviePoster = editMovie.MovieItem.MoviePoster;
                movieUpdate.MoviePosterGuid = editMovie.MovieItem.MoviePosterGuid;
            }

            #endregion

            #region Изменить информацию о сериях

            movieUpdate.MovieTotalParts = editMovie.MovieItem.MovieTotalParts;
            movieUpdate.MoviePart = editMovie.MovieItem.MoviePart;

            #region Изменить ссылку на страницу серий фильма

            if (!string.IsNullOrEmpty(editMovie.PageForSeriesString.Trim()) & editMovie.PageForSeriesString.Trim() != "0")
            {
                editMovie.PageForSeriesString = "/" + editMovie.PageForSeriesString.Trim().Trim('/');

                if (!await pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == editMovie.PageForSeriesString).AnyAsync()
                        & !await pageInfoContext.PagesInfo.Where(p => p.PagePathNickNameWithData == editMovie.PageForSeriesString).AnyAsync())
                {
                    ModelState.AddModelError("PageOfSeries", $"{editMovie.PageForSeriesString} - страницы нет в базе данных");

                    return View(editMovie);
                }
                else if (await pageInfoContext.PagesInfo.Where(p => p.PageFullPathWithData == editMovie.PageForSeriesString).AnyAsync())
                {
                    var pageForSeries = await pageInfoContext.PagesInfo.FirstAsync(p => p.PageFullPathWithData == editMovie.PageForSeriesString);
                    movieUpdate.PageInfoModelIdForSeries = pageForSeries.PageInfoModelId;
                }
                else if (await pageInfoContext.PagesInfo.Where(p => p.PagePathNickNameWithData == editMovie.PageForSeriesString).AnyAsync())
                {
                    var pageForSeries = await pageInfoContext.PagesInfo.FirstAsync(p => p.PagePathNickNameWithData == editMovie.PageForSeriesString);
                    movieUpdate.PageInfoModelIdForSeries = pageForSeries.PageInfoModelId;
                }
                else
                {
                    movieUpdate.PageInfoModelIdForSeries = editMovie.MovieItem.PageInfoModelIdForSeries;
                }
            }
            else if (editMovie.PageForSeriesString?.Trim() == "0")
            {
                movieUpdate.PageInfoModelIdForSeries = null;
            }
            else
            {
                movieUpdate.PageInfoModelIdForSeries = editMovie.MovieItem.PageInfoModelIdForSeries;
            }

            #endregion

            #region Изменить картинку заголовка серий

            if (editMovie.ImageHeadForSeriesFormFile != null && editMovie.MovieItem.MovieTotalParts > 1 && editMovie.MovieItem.MoviePart == 1)
            {
                if (await imageContext.ImageFiles.Where(i => i.WebImageFileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageFileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIconFileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIconFileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebImageHDFileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebImageHDFileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon200FileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon200FileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.WebIcon100FileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.WebIcon100FileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageFileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageFileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.IconFileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.IconFileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.ImageHDFileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.ImageHDFileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon200FileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon200FileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else if (await imageContext.ImageFiles.Where(i => i.Icon100FileName == editMovie.ImageHeadForSeriesFormFile.FileName).AnyAsync())
                {
                    var imageHeadSeriesFile = await imageContext.ImageFiles.FirstAsync(i => i.Icon100FileName == editMovie.ImageHeadForSeriesFormFile.FileName);

                    movieUpdate.ImageForHeadSeriesId = imageHeadSeriesFile.ImageFileModelId;
                }
                else
                {
                    ModelState.AddModelError("ImageHeadForSeriesFormFile", $"Добавьте картинку серий «{editMovie.ImageHeadForSeriesFormFile.FileName}» в базу данных");

                    return View();
                }
            }
            else
            {
                movieUpdate.ImageForHeadSeriesId = editMovie.MovieItem.ImageForHeadSeriesId;
            }

            #endregion

            #region Фильтр поиска серий

            movieUpdate.SeriesSearchFilter = editMovie.MovieItem.SeriesSearchFilter.Trim();

            #endregion

            #endregion

            #region Заголовок, описание, примечание, лента кадров

            movieUpdate.MovieCaption = editMovie.MovieItem!.MovieCaption.Trim();
            movieUpdate.MovieCaptionForOnline = editMovie.MovieItem.MovieCaptionForOnline.Trim();
            movieUpdate.MovieDescriptionForSchemaOrg = editMovie.MovieItem.MovieDescriptionForSchemaOrg.Trim();
            movieUpdate.MovieDescriptionHtml = editMovie.MovieItem.MovieDescriptionHtml.Trim();
            movieUpdate.MovieNote = editMovie.MovieItem.MovieNote.Trim();

            if (editMovie.MovieItem.FramesAroundMovie.Trim() != string.Empty)
            {
                if (!await movieInfoContext.MovieFiles.Where(p => p.MovieCaption == editMovie.MovieItem.FramesAroundMovie.Trim()).AnyAsync())
                {
                    ModelState.AddModelError("FramesAroundMovie", $"Фильма «{editMovie.MovieItem.FramesAroundMovie}»  нет в базе данных");

                    return View(editMovie);
                }
                else
                {
                    movieUpdate.FramesAroundMovie = editMovie.MovieItem.FramesAroundMovie.Trim();
                }
            }
            else
            {
                movieUpdate.FramesAroundMovie = string.Empty;
            }

            #endregion

            #region Формат изображения

            movieUpdate.MovieScreenFormat = editMovie.MovieItem.MovieScreenFormat.Trim();

            #endregion

            #region Даты

            movieUpdate.MovieDatePublished = editMovie.MovieItem.MovieDatePublished;
            movieUpdate.MovieDateCreated = editMovie.MovieItem.MovieDateCreated;
            movieUpdate.MovieUploadDate = editMovie.MovieItem.MovieUploadDate;

            #endregion

            #region Жанр фильм

            movieUpdate.MovieGenre = editMovie.MovieItem.MovieGenre.Trim();

            #endregion

            #region Фильтр поиска фильма

            if (editMovie.MovieItem.SearchFilter != null)
            {
                movieUpdate.SearchFilter = editMovie.MovieItem.SearchFilter.Trim();
            }
            else
            {
                movieUpdate.SearchFilter = string.Empty;
            }

            #endregion

            #region Темы видео

            var topicFilters = Request.Form["topicOptions"].ToString().Split(',');

            string topics = string.Empty;

            if (Request.Form["topicOptions"].Count > 0)
            {
                for (int i = 0; i < topicFilters.Length; i++)
                {
                    if (Guid.TryParse(topicFilters[i], out var guidOutput))
                    {
                        if (!await topicMovieContext.TopicMovies.Where(t => t.TopicMovieModelId == guidOutput).AnyAsync())
                        {
                            Array.Clear(topicFilters, i, 1);
                        }
                    }
                    else
                    {
                        Array.Clear(topicFilters, i, 1);
                    }
                }

                for (int i = 0; i < topicFilters.Length; i++)
                {
                    if (topicFilters[i] != null)
                    {
                        topics += topicFilters[i] + ',';
                    }
                }

                movieUpdate.TopicGuidList = topics.TrimEnd(',');
            }
            else
            {
                movieUpdate.TopicGuidList = string.Empty;
            }

            #endregion

            #region Ограничения для просмотра

            movieUpdate.MovieAdult = editMovie.MovieItem.MovieAdult;
            movieUpdate.MovieIsFamilyFriendly = editMovie.MovieItem.MovieIsFamilyFriendly;
            movieUpdate.MovieInMainList = editMovie.MovieItem.MovieInMainList;

            #endregion

            #region Язык, субтитры

            movieUpdate.MovieInLanguage1 = editMovie.MovieItem.MovieInLanguage1;
            movieUpdate.MovieInLanguage2 = editMovie.MovieItem.MovieInLanguage2;
            movieUpdate.MovieSubtitles1 = editMovie.MovieItem.MovieSubtitles1;
            movieUpdate.MovieSubtitles2 = editMovie.MovieItem.MovieSubtitles2;

            #endregion

            #region Съемочная группа

            movieUpdate.MovieРroductionCompany = editMovie.MovieItem.MovieРroductionCompany.Trim();
            movieUpdate.MovieDirector1 = editMovie.MovieItem.MovieDirector1.Trim();
            movieUpdate.MovieDirector2 = editMovie.MovieItem.MovieDirector2.Trim();
            movieUpdate.MovieMusicBy = editMovie.MovieItem.MovieMusicBy.Trim();
            movieUpdate.MovieActor01 = editMovie.MovieItem.MovieActor01.Trim();
            movieUpdate.MovieActor02 = editMovie.MovieItem.MovieActor02.Trim();
            movieUpdate.MovieActor03 = editMovie.MovieItem.MovieActor03.Trim();
            movieUpdate.MovieActor04 = editMovie.MovieItem.MovieActor04.Trim();
            movieUpdate.MovieActor05 = editMovie.MovieItem.MovieActor05.Trim();
            movieUpdate.MovieActor06 = editMovie.MovieItem.MovieActor06.Trim();
            movieUpdate.MovieActor07 = editMovie.MovieItem.MovieActor07.Trim();
            movieUpdate.MovieActor08 = editMovie.MovieItem.MovieActor08.Trim();
            movieUpdate.MovieActor09 = editMovie.MovieItem.MovieActor09.Trim();
            movieUpdate.MovieActor10 = editMovie.MovieItem.MovieActor10.Trim();

            #endregion

            #region Ссылки на видео хостинги

            movieUpdate.MovieContentUrl = editMovie.MovieItem.MovieContentUrl;
            movieUpdate.MovieYouTube = editMovie.MovieItem.MovieYouTube;
            movieUpdate.MovieVkVideo = editMovie.MovieItem.MovieVkVideo;
            movieUpdate.MovieMailRuVideo = editMovie.MovieItem.MovieMailRuVideo;
            movieUpdate.MovieOkVideo = editMovie.MovieItem.MovieOkVideo;
            movieUpdate.MovieYandexDiskVideo = editMovie.MovieItem.MovieYandexDiskVideo;

            #endregion

            #region Ссылки на информацию

            movieUpdate.MovieKinoTeatrRu = editMovie.MovieItem.MovieKinoTeatrRu;
            movieUpdate.MovieKinoPoisk = editMovie.MovieItem.MovieKinoPoisk;
            movieUpdate.MovieImbd = editMovie.MovieItem.MovieImbd;

            #endregion

            #region Изменить блок ссылок под фильмом

            #region Фотокарусель

            movieUpdate.Carousel = editMovie.MovieItem.Carousel;

            #endregion

            #region Блок1

            movieUpdate.HeadTitleForVideoLinks1 = editMovie.MovieItem.HeadTitleForVideoLinks1.Trim();
            movieUpdate.SearchFilter1 = editMovie.MovieItem.SearchFilter1.Trim();
            movieUpdate.IsImage1 = editMovie.MovieItem.IsImage1;
            movieUpdate.IconType1 = editMovie.MovieItem.IconType1;
            movieUpdate.IsPartsMoreOne1 = editMovie.MovieItem.IsPartsMoreOne1;
            movieUpdate.AllMoviesFromDB1 = editMovie.MovieItem.AllMoviesFromDB1;

            #endregion

            #region Блок2

            movieUpdate.HeadTitleForVideoLinks2 = editMovie.MovieItem.HeadTitleForVideoLinks2.Trim();
            movieUpdate.SearchFilter2 = editMovie.MovieItem.SearchFilter2.Trim();
            movieUpdate.IsImage2 = editMovie.MovieItem.IsImage2;
            movieUpdate.IconType2 = editMovie.MovieItem.IconType2;
            movieUpdate.IsPartsMoreOne2 = editMovie.MovieItem.IsPartsMoreOne2;
            movieUpdate.AllMoviesFromDB2 = editMovie.MovieItem.AllMoviesFromDB2;

            #endregion

            #region Блок3

            movieUpdate.HeadTitleForVideoLinks3 = editMovie.MovieItem.HeadTitleForVideoLinks3.Trim();
            movieUpdate.SearchFilter3 = editMovie.MovieItem.SearchFilter3.Trim();
            movieUpdate.IsImage3 = editMovie.MovieItem.IsImage3;
            movieUpdate.IconType3 = editMovie.MovieItem.IconType3;
            movieUpdate.IsPartsMoreOne3 = editMovie.MovieItem.IsPartsMoreOne3;
            movieUpdate.AllMoviesFromDB3 = editMovie.MovieItem.AllMoviesFromDB3;

            #endregion

            #endregion

            await movieInfoContext.SaveChangesInMovieAsync();

            return RedirectToAction("DetailsMovie", new { movieId = movieUpdate.MovieFileModelId, Area = "Admin" });
        }
        else
        {
            return View(editMovie);
        }
    }

    #endregion

    #region Удалить фильм из базы данных

    [HttpGet]
    public async Task<IActionResult> DeleteMovie(Guid? movieId)
    {
        MovieFileModel deleteMovie = new();

        if (movieId.HasValue)
        {
            if (await movieInfoContext.MovieFiles.Where(i => i.MovieFileModelId == movieId).AnyAsync())
            {
                deleteMovie = await movieInfoContext.MovieFiles.FirstAsync(i => i.MovieFileModelId == movieId);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            return View(deleteMovie);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMovie(MovieFileModel deleteMovie)
    {
        if (deleteMovie != null)
        {
            await movieInfoContext.DeleteMovieAsync(deleteMovie.MovieFileModelId);

            return RedirectToAction(nameof(Index));
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion
}