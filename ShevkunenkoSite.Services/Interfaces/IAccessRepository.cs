namespace ShevkunenkoSite.Services.Interfaces;

public interface IAccessRepository
{
    IQueryable<AccessModel> Accesses { get; }
}
