using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Services.Interfaces;

public class IconFileImplementation : IIconFileRepository
{
    private readonly SiteDbContext _siteContext;
    public IconFileImplementation(SiteDbContext siteContext) => _siteContext = siteContext;

    public IQueryable<IconFileModel> IconFiles => _siteContext.IconFile;

    public async Task SaveChangesInIconAsync()
    {
        await _siteContext.SaveChangesAsync();
    }

    public async Task AddNewIconAsync(IconFileModel icon)
    {
        await _siteContext.IconFile.AddAsync(icon);
        await SaveChangesInIconAsync();
    }
}