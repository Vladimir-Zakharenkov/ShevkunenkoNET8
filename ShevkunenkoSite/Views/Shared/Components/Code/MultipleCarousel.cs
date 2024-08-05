namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class MultipleCarousel : ViewComponent
{
    private readonly IImageFileRepository _imageContext;
    private readonly IPageInfoRepository _pageInfoContext;
    private readonly IMovieFileRepository _movieContext;
    public MultipleCarousel(IImageFileRepository imageContext, IPageInfoRepository pageInfoContext, IMovieFileRepository movieContext)
    {
        _imageContext = imageContext;
        _pageInfoContext = pageInfoContext;
        _movieContext = movieContext;
    }

    public ImageFileModel[] firstGroup = Array.Empty<ImageFileModel>();
    public ImageFileModel[] secondGroup = Array.Empty<ImageFileModel>();
    public ImageFileModel[] thirdGroup = Array.Empty<ImageFileModel>();

    public async Task<IViewComponentResult> InvokeAsync(MovieFileModel movieCarousel)
    {
        PageInfoModel pageInfoModel = await _pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        ImageFileModel[] pictures = Array.Empty<ImageFileModel>();

        MovieFileModel? fullMovie = null;

        if (await _movieContext.MovieFiles.Where(m => m.MovieFileModelId == movieCarousel.FullMovieID).AnyAsync())
        {
            fullMovie = await _movieContext.MovieFiles.FirstAsync(m => m.MovieFileModelId == movieCarousel.FullMovieID);
        }

        if (movieCarousel.MovieCaption.Contains("Интервью"))
        {
            pictures = await _imageContext.ImageFiles
                .Where(p => p.SearchFilter.Contains("Криминальная звезда," ?? string.Empty))
                .ToArrayAsync();
        }
        else
        {
            pictures = await _imageContext.ImageFiles
            .Where(p => p.SearchFilter.Contains($"{movieCarousel.MovieCaption}," ?? string.Empty))
            .ToArrayAsync();
        }

        int numberOfPictures = pictures.Length;

        if (numberOfPictures >= 18)
        {
            firstGroup = pictures.Take(6).ToArray();
            secondGroup = pictures.Skip(6).Take(6).ToArray();
            thirdGroup = pictures.Skip(12).Take(6).ToArray();
        }
        else if (numberOfPictures >= 12 & numberOfPictures < 18)
        {
            firstGroup = pictures.Take(4).ToArray();
            secondGroup = pictures.Skip(4).Take(4).ToArray();
            thirdGroup = pictures.Skip(8).Take(4).ToArray();
        }
        else
        {
            firstGroup = pictures;
            secondGroup = pictures;
            thirdGroup = pictures;
        }

        return View(new ThreeCarouselsViewModel
        {
            PageInfo = pageInfoModel,
            MovieCarousel = movieCarousel,
            FirstCarousel = firstGroup,
            SecondCarousel = secondGroup,
            ThirdCarousel = thirdGroup
        });
    }
}