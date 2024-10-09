namespace ShevkunenkoSite.Views.Shared.Components.Code;

public class RefPages(IPageInfoRepository pageInfoContext, IMovieFileRepository movieContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        PageInfoModel pageInfoModel = await pageInfoContext.GetPageInfoByPathAsync(HttpContext);

        List<List<PageInfoModel>> listsOfFilterOut = [];

        List<PageInfoModel> linksToPagesByGuid = [];

        List<PageInfoModel> linksToPagesByGuid2 = [];

        List<VideoLinksViewModel> listOfVideoLinksViewModel = [];

        VideoLinksViewModel videoLinksViewModel = new();

        if (pageInfoModel.PageLinks == false & pageInfoModel.PageLinks2 == false & pageInfoModel.PageLinksByFilters == false & pageInfoModel.VideoLinks == false)
        {
            return View("Empty");
        }
        else if (string.IsNullOrEmpty(pageInfoModel.PageFilterOut) & string.IsNullOrEmpty(pageInfoModel.RefPages) & string.IsNullOrEmpty(pageInfoModel.RefPages2) & string.IsNullOrEmpty(pageInfoModel.VideoFilterOut))
        {
            return View("Empty");
        }
        else
        {
            // ссылки на связанные страницы по GUID (1)
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

            // ссылки на связанные страницы по GUID (2)
            if (!string.IsNullOrEmpty(pageInfoModel.RefPages2) & pageInfoModel.PageLinks2 == true)
            {
                string[] pageIdOut2 = pageInfoModel.RefPages2.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (pageIdOut2.Length > 0)
                {
                    foreach (string pageId2 in pageIdOut2)
                    {
                        if (Guid.TryParse(pageId2, out Guid pageGuid2))
                        {
                            if (await pageInfoContext.PagesInfo.Where(p => p.PageInfoModelId == pageGuid2).AnyAsync())
                            {
                                linksToPagesByGuid2.Add(await pageInfoContext.PagesInfo.FirstAsync(p => p.PageInfoModelId == pageGuid2));
                            }
                        }
                    }

                    _ = linksToPagesByGuid2.Distinct().OrderBy(p => p.PageCardText);
                }
            }

            // ссылки на связанные страницы по фильтру
            if (!string.IsNullOrEmpty(pageInfoModel.PageFilterOut) & pageInfoModel.PageLinksByFilters == true)
            {
                string[] pageFilterOut = pageInfoModel.PageFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (pageFilterOut.Length > 0)
                {
                    for (int i = 0; i < pageFilterOut.Length; i++)
                    {
#pragma warning disable CA1862
                        if (await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFilterOut[i])).AnyAsync())
                        {
                            listsOfFilterOut.Add(await pageInfoContext.PagesInfo.Where(p => p.PageFilter.Contains(pageFilterOut[i])).OrderBy(od => od.PageCardText).ToListAsync());
                        }
#pragma warning restore CA1862
                    }
                }
                _ = listsOfFilterOut.Distinct();
            }

            // ссылки на связанные видео по текстовому фильтру VideoFilterOut
            if (!string.IsNullOrEmpty(pageInfoModel.VideoFilterOut) & pageInfoModel.VideoLinks == true)
            {
                string[] videoFilterOut = pageInfoModel.VideoFilterOut.Split(',', StringSplitOptions.RemoveEmptyEntries);

                if (videoFilterOut.Length > 0)
                {
                    for (int i = 0; i < videoFilterOut.Length; i++)
                    {
                        if (await movieContext.MovieFiles.Where(p => p.SearchFilter.Contains(videoFilterOut[i])).AnyAsync())
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

                            listOfVideoLinksViewModel.Add(videoLinksViewModel);
                        }
                    }
                    _ = listOfVideoLinksViewModel.Distinct();
                }
            }
        }

        if (listsOfFilterOut.Count < 1 & linksToPagesByGuid.Count < 1 & listOfVideoLinksViewModel.Count < 1 & linksToPagesByGuid2.Count < 1)
        {
            return View("Empty");
        }
        else
        {
            return View(new RefPagesViewModel
            {
                LinksToPagesByGuid = linksToPagesByGuid,
                LinksToPagesByGuid2 = linksToPagesByGuid2,
                ListsOfFilterOut = listsOfFilterOut,
                ListOfVideoLinksViewModel = listOfVideoLinksViewModel
            });
        }
    }
}