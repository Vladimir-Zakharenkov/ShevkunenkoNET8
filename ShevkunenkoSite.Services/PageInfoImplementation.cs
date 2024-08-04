using Microsoft.AspNetCore.Http;

namespace ShevkunenkoSite.Services;

public class PageInfoImplementation : IPageInfoRepository
{
    private readonly SiteDbContext _siteContext;
    public PageInfoImplementation(SiteDbContext siteContext) => _siteContext = siteContext;

    public IQueryable<PageInfoModel> PagesInfo => _siteContext.PageInfo
        .Include(i => i.ImageFileModel)
        .Include(b => b.BackgroundFileModel)
        .Include(m => m.MovieFile).ThenInclude(mi => mi!.ImageFileModel);

    #region Определить страницу по адрессу

    public async Task<PageInfoModel> GetPageInfoByPathAsync(HttpContext httpContext)

    {
        string pagePath = httpContext.Request.Path.ToString().ToLower().TrimEnd('/');

        string routData = string.Empty;

        if (!string.IsNullOrEmpty(httpContext.Request.QueryString.ToString()))
        {
            foreach (var item in httpContext.Request.Query)
            {
                if (routData == string.Empty)
                {
                    routData = "?" + item.Key + "=" + item.Value;
                }
                else
                {
                    routData = routData + "&" + item.Key + "=" + item.Value;
                }
            }

            for (int i = 0; i < httpContext.Request.Query.Count; i++)
            {
                if (await PagesInfo.Where(p => p.PageFullPath == pagePath & p.RoutData == routData).AnyAsync())
                {
                    return await PagesInfo.FirstAsync(p => p.PageFullPath == pagePath & p.RoutData == routData);
                }
                else if (await PagesInfo.Where(p => p.PagePathNickName == pagePath & p.RoutData == routData).AnyAsync())
                {
                    return await PagesInfo.FirstAsync(p => p.PagePathNickName == pagePath & p.RoutData == routData);
                }
                else
                {
                    if (i == httpContext.Request.Query.Count - 1)
                    {
                        routData = string.Empty;
                    }
                    else
                    {
                        if (routData.Contains("&videohosting=https://vk.com"))
                        {
                            routData = routData[..routData.LastIndexOf("&videohosting=https://vk.com")];
                        }
                        else
                        {
                            routData = routData[..routData.LastIndexOf("&")];
                        }
                    }
                }
            }
        }

        if (routData == string.Empty)
        {
            if (string.IsNullOrEmpty(pagePath))
            {
                return await PagesInfo.FirstAsync(p => p.PageFullPath == "/shevkunenko/index");
            }
            else if (await PagesInfo.Where(p => p.PageFullPath == pagePath).AnyAsync())
            {
                return await PagesInfo.FirstAsync(p => p.PageFullPath == pagePath); ;
            }
            else if (await PagesInfo.Where(p => p.PagePathNickName == pagePath).AnyAsync())
            {
                return await PagesInfo.FirstAsync(p => p.PagePathNickName == pagePath);
            }
            else if (await PagesInfo.Where(p => p.PageFullPath == pagePath + "/index").AnyAsync())
            {
                return await PagesInfo.FirstAsync(p => p.PageFullPath == pagePath + "/index");
            }
            else if (await PagesInfo.Where(p => p.PagePathNickName == pagePath + "/index").AnyAsync())
            {
                return await PagesInfo.FirstAsync(p => p.PagePathNickName == pagePath + "/index");
            }
            else
            {
                return await PagesInfo.FirstAsync(p => p.PageFullPath == "/error404");
            }
        }
        else
        {
            return await PagesInfo.FirstAsync(p => p.PageFullPath == "/error404");
        }
    }

    #endregion

    #region Сохранить изменения

    public async Task SaveChangesInPageAsync()
    {
        _ = await _siteContext.SaveChangesAsync();
    }

    #endregion

    #region Добавить страницу

    public async Task AddNewPageAsync(PageInfoModel page)
    {
        _ = await _siteContext.PageInfo.AddAsync(page);
        await SaveChangesInPageAsync();
    }

    #endregion

    #region Удалить страницу

    public async Task DeletePageAsync(Guid pageId)
    {
        if (await _siteContext.PageInfo.Where(i => i.PageInfoModelId == pageId).AnyAsync())
        {
            PageInfoModel pageToDelete = await _siteContext.PageInfo.FirstAsync(i => i.PageInfoModelId == pageId);

            _ = _siteContext.PageInfo.Remove(pageToDelete);
            _ = await _siteContext.SaveChangesAsync();
        }
    }

    #endregion
}