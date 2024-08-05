namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class VideoCard : ViewComponent
{
    private readonly IPageInfoRepository _pageInfoContext;
    private readonly IImageFileRepository _imageFileContext;
    public VideoCard(IPageInfoRepository pageInfoContext, IImageFileRepository imageFileContext)
    {
        _pageInfoContext = pageInfoContext;
        _imageFileContext = imageFileContext;
    }

    string pathForSeries = string.Empty;

    string imageForVideo = string.Empty;

    Guid? imageForVideoGuid;

    PageInfoModel pageForMoreOneParts = new();

    ImageFileModel imageVideo = new();

    public async Task<IViewComponentResult> InvokeAsync(MovieFileModel movieFileModel, bool IsPartsMoreOne, bool? isImage, string? iconType = "webicon300")
    {
        if (IsPartsMoreOne == true & movieFileModel.PageInfoModelIdForSeries != Guid.Empty & movieFileModel.PageInfoModelIdForSeries != null)
        {
            if (await _pageInfoContext.PagesInfo.Where(p => p.PageInfoModelId == movieFileModel.PageInfoModelIdForSeries).AnyAsync())
            {
                pageForMoreOneParts = await _pageInfoContext.PagesInfo.FirstAsync(p => p.PageInfoModelId == movieFileModel.PageInfoModelIdForSeries);

                pathForSeries = pageForMoreOneParts.PageFullPathWithData;
            }
            else
            {
                pathForSeries = string.Empty;
            }
        }

        if (isImage == true & await _imageFileContext.ImageFiles.Where(i => i.ImageFileModelId == movieFileModel.ImageFileModelId).AnyAsync())
        {
            imageVideo = await _imageFileContext.ImageFiles.FirstAsync(i => i.ImageFileModelId == movieFileModel.ImageFileModelId);

            imageForVideo = imageVideo.ImageFileName;

            imageForVideoGuid = imageVideo.ImageFileModelId;
        }
        else if (isImage == false & movieFileModel.MoviePosterGuid != null)
        {
            imageForVideoGuid = movieFileModel.MoviePosterGuid!;
        }
        else
        {
            imageForVideo = "no-image.png";
        }

        return View(new VideoCardViewModel()
        {
            MovieInfo = movieFileModel,
            PathForSeries = pathForSeries,
            ImageForVideo = imageForVideo,
            ImageForVideoGuid = imageForVideoGuid,
            IsImageIcon = isImage,
            IconType = iconType ?? "webicon300"
        });
    }
}