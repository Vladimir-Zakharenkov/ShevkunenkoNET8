namespace ShevkunenkoSite.Services.Interfaces;

public interface IPageInfoRepository
{
    IQueryable<PageInfoModel> PagesInfo { get; }

    Task<PageInfoModel> GetPageInfoByPathAsync(HttpContext httpContext);

    Task AddNewPageAsync(PageInfoModel page);

    Task SaveChangesInPageAsync();

    Task DeletePageAsync(Guid pageId);
}