namespace ShevkunenkoSite.Services.Interfaces;

public class IconFileImplementation(SiteDbContext siteContext) : IIconFileRepository
{
    public IQueryable<IconFileModel> IconFiles => siteContext.IconFile;

    public async Task SaveChangesInIconAsync()
    {
        await siteContext.SaveChangesAsync();
    }

    public async Task AddNewIconAsync(IconFileModel icon)
    {
        await siteContext.IconFile.AddAsync(icon);
        await SaveChangesInIconAsync();
    }

    public Task DeleteIconAsync(Guid iconId)
    {
        throw new NotImplementedException();
    }
}