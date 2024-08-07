namespace ShevkunenkoSite.Services.Interfaces;

public class IconFileImplementation(SiteDbContext siteContext) : IIconFileRepository
{
    private readonly SiteDbContext _siteContext = siteContext;

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

    public Task DeleteIconAsync(Guid iconId)
    {
        throw new NotImplementedException();
    }
}