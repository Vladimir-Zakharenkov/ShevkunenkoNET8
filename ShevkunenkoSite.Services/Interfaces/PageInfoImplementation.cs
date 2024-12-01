﻿namespace ShevkunenkoSite.Services.Interfaces;

public class PageInfoImplementation(SiteDbContext siteContext) : IPageInfoRepository
{
    public IQueryable<PageInfoModel> PagesInfo => siteContext.PageInfo
        .Include(i => i.ImageFileModel)
        .Include(b => b.BackgroundFileModel)
        .Include(m => m.MovieFile).ThenInclude(mi => mi!.ImageFileModel);

    #region Определить страницу в базе данных по запросу

    public async Task<PageInfoModel> GetPageInfoByPathAsync(HttpContext httpContext)
    {
        string pagePath = httpContext.Request.Path.ToString().ToLower().TrimEnd('/');

        IQueryCollection pageQuery = httpContext.Request.Query;

        string pageQueryString = httpContext.Request.QueryString.ToString();
        
        string routData = string.Empty;

        if (pageQuery.Count >0)
        {
            foreach (var item in pageQuery)
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
                else if (await PagesInfo.Where(p => p.PageFullPath == pagePath + "/index" & p.RoutData == routData).AnyAsync())
                {
                    return await PagesInfo.FirstAsync(p => p.PageFullPath == pagePath + "/index" & p.RoutData == routData);
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
                        else if (routData.Contains("&videohosting=https://vkvideo.ru"))
                        {
                            routData = routData[..routData.LastIndexOf("&videohosting=https://vkvideo.ru")];
                        }
                        else
                        {
                            routData = routData[..routData.LastIndexOf('&')];
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
                var pageInfo = await PagesInfo.FirstAsync(p => p.PageFullPath == pagePath);

                if (!string.IsNullOrEmpty(pageInfo.RoutData)) 
                {
                    return await PagesInfo.FirstAsync(p => p.PageFullPath == "/shevkunenko/error404");
                }

                return (pageInfo);
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
                return await PagesInfo.FirstAsync(p => p.PageFullPath == "/shevkunenko/error404");
            }
        }
        else
        {
            return await PagesInfo.FirstAsync(p => p.PageFullPath == "/shevkunenko/error404");
        }
    }

    #endregion

    #region Сохранить изменения

    public async Task SaveChangesInPageAsync()
    {
        _ = await siteContext.SaveChangesAsync();
    }

    #endregion

    #region Добавить страницу

    public async Task AddNewPageAsync(PageInfoModel page)
    {
        _ = await siteContext.PageInfo.AddAsync(page);
        await SaveChangesInPageAsync();
    }

    #endregion

    #region Удалить страницу

    public async Task DeletePageAsync(Guid pageId)
    {
        if (await siteContext.PageInfo.Where(i => i.PageInfoModelId == pageId).AnyAsync())
        {
            PageInfoModel pageToDelete = await siteContext.PageInfo.FirstAsync(i => i.PageInfoModelId == pageId);

            _ = siteContext.PageInfo.Remove(pageToDelete);
            _ = await siteContext.SaveChangesAsync();
        }
    }

    #endregion
}