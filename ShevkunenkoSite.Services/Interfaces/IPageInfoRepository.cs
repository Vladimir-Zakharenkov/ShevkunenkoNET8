using Microsoft.AspNetCore.Http;
using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Services.Interfaces;

public interface IPageInfoRepository
{
    IQueryable<PageInfoModel> PagesInfo { get; }

    //Task<PageInfoModel> GetPageInfoByPathAsync(string pagePath, IQueryCollection? pageQuery);

    Task<PageInfoModel> GetPageInfoByPathAsync(HttpContext httpContext);

    Task AddNewPageAsync(PageInfoModel page);

    Task SaveChangesInPageAsync();

    Task DeletePageAsync(Guid pageId);
}