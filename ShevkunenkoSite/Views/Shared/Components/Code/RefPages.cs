namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class RefPages(IPageInfoRepository pageInfoContext, IMovieFileRepository movieContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        PageInfoModel pageInfoModel = await pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        List<List<PageInfoModel>> listsOfFilterOut = [];

        List<PageInfoModel> linksToPagesByGuid = [];

        List<VideoLinksViewModel> listsOfVideoFilterOut = [];

        VideoLinksViewModel videoLinksViewModel = new();

        if (string.IsNullOrEmpty(pageInfoModel.PageFilterOut) & string.IsNullOrEmpty(pageInfoModel.RefPages) & string.IsNullOrEmpty(pageInfoModel.VideoFilterOut))
        {
            return View("Empty");
        }
        else if (pageInfoModel.PageLinks == false & pageInfoModel.PageLinksByFilters == false & pageInfoModel.VideoLinks == false)
        {
            return View("Empty");
        }
        else
        {
            if (!string.IsNullOrEmpty(pageInfoModel.VideoFilterOut) & pageInfoModel.VideoLinks == true)
            {
                string[] videoFilterOut = pageInfoModel.VideoFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (videoFilterOut.Length > 0)
                {
                    for (int i = 0; i < videoFilterOut.Length; i++)
                    {
#pragma warning disable CA1862
                        if (await movieContext.MovieFiles.Where(p => p.SearchFilter.ToLower().Contains(videoFilterOut[i])).AnyAsync())
                        {
                            videoLinksViewModel = new()
                            {
                                HeadTitleForVideoLinks = videoFilterOut[i],
                                IsImage = false,
                                IconType = "webicon300",
                                SearchFilter = videoFilterOut[i],
                                MovieInMainList = true,
                                IsPartsMoreOne = true
                            };

                            listsOfVideoFilterOut.Add(videoLinksViewModel);
                        }
#pragma warning restore CA1862
                    }
                    _ = listsOfVideoFilterOut.Distinct();
                }
            }

            if (!string.IsNullOrEmpty(pageInfoModel.PageFilterOut) & pageInfoModel.PageLinksByFilters == true)
            {
                string[] pageFilterOut = pageInfoModel.PageFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (pageFilterOut.Length > 0)
                {
                    for (int i = 0; i < pageFilterOut.Length; i++)
                    {
#pragma warning disable CA1862
                        if (await pageInfoContext.PagesInfo.Where(p => p.PageFilter.ToLower().Contains(pageFilterOut[i])).AnyAsync())
                        {
                            listsOfFilterOut.Add(await pageInfoContext.PagesInfo.Where(p => p.PageFilter.ToLower().Contains(pageFilterOut[i])).ToListAsync());
                        }
#pragma warning restore CA1862
                    }
                }
                _ = listsOfFilterOut.Distinct();
            }

            if (!string.IsNullOrEmpty(pageInfoModel.RefPages) & pageInfoModel.PageLinks == true)
            {
                string[] pageIdOut = pageInfoModel.RefPages.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (pageIdOut.Length > 0)
                {
                    foreach (string pageId in pageIdOut)
                    {
                        if (Guid.TryParse(pageId, out Guid pageGuid))
                        {
                            if (await pageInfoContext.PagesInfo.Where(p => p.PageInfoModelId == pageGuid).AnyAsync())
                            {
                                linksToPagesByGuid.Add(await pageInfoContext.PagesInfo.FirstAsync(p => p.PageInfoModelId == pageGuid));
                            }
                        }
                    }

                    _ = linksToPagesByGuid.Distinct().OrderBy(p => p.PageCardText);
                }
            }
        }

        if (listsOfFilterOut.Count < 1 & linksToPagesByGuid.Count < 1)
        {
            return View("Empty");
        }
        else
        {
            return View(new RefPagesViewModel
            {
                ListsOfVideoFilterOut = listsOfVideoFilterOut,
                ListsOfFilterOut = listsOfFilterOut,
                LinksToPagesByGuid = linksToPagesByGuid
            });
        }
    }
}