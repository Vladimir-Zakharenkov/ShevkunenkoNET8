namespace ShevkunenkoSite.Services.Interfaces;

public class ImageFileImplementation : IImageFileRepository
{
    private readonly SiteDbContext _siteContext;
    public ImageFileImplementation(SiteDbContext siteContext) => _siteContext = siteContext;

    public IQueryable<ImageFileModel> ImageFiles => _siteContext.ImageFile;

    public async Task SaveChangesInImageAsync()
    {
        await _siteContext.SaveChangesAsync();
    }

    public async Task AddNewImageAsync(ImageFileModel image)
    {
        await _siteContext.ImageFile.AddAsync(image);
        await SaveChangesInImageAsync();
    }

    public async Task DeleteImageAsync(Guid imageId)
    {
        if (await _siteContext.ImageFile.Where(i => i.ImageFileModelId == imageId).AnyAsync())
        {
            ImageFileModel imageToDelete = await _siteContext.ImageFile.FirstAsync(i => i.ImageFileModelId == imageId);

            _ = _siteContext.ImageFile.Remove(imageToDelete);
            _ = await _siteContext.SaveChangesAsync();
        }
    }
}
