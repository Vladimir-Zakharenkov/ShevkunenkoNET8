﻿namespace ShevkunenkoSite.Models.ViewModels;

public class RefPagesViewModel
{
    // Словарь страниц по текстовым фильтрам
    public Dictionary<string, List<PageInfoModel>> DictionaryOfPages { get; set; } = [];

    // Словарь картинок по текстовым фильтрам
    public Dictionary<string, List<ImageFileModel>> DictionaryOfPictures { get; set; } = [];

    // связанные страницы по GUID (1)
    public List<PageInfoModel> LinksToPagesByGuid { get; set; } = [];

    // связанные страницы по GUID (2)
    public List<PageInfoModel> LinksToPagesByGuid2 { get; set; } = [];

    // список VideoLinksViewModel связанных видео
    public List<VideoLinksViewModel> ListOfVideoLinksViewModel { get; set; } = [];
}