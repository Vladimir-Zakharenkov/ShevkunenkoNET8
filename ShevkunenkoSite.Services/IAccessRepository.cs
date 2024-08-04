namespace ShevkunenkoSite.Services;

public interface IAccessRepository
{
    IQueryable<AccessModel> Accesses { get; }
}
