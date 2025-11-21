namespace ShevkunenkoSite.Services.Interfaces;

public class AccessImplementation : IAccessRepository
{
    private readonly SiteDbContext _siteContext;
    public AccessImplementation(SiteDbContext siteContext) => _siteContext = siteContext;

    public IQueryable<AccessModel> Accesses => _siteContext.Access;
}