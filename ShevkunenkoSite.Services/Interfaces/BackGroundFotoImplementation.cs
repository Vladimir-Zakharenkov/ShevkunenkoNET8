using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Services.Interfaces;

public class BackGroundFotoImplementation : IBackgroundFotoRepository
{
    private readonly SiteDbContext _siteContext;
    public BackGroundFotoImplementation(SiteDbContext siteContext) => _siteContext = siteContext;

    public IQueryable<BackgroundFileModel> BackgroundFiles => _siteContext.BackgroundFile;
}