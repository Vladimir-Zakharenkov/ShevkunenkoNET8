namespace ShevkunenkoSite.Models.ViewModels;

public class VideoLinksViewModel
{
    public string HeadTitleForVideoLinks { get; set; } = string.Empty;

    public bool? IsImage { get; set; } = null; // true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео

    public string IconType { get; set; } = string.Empty; // "hd", "image" "icon300", "icon200", "icon100", "webhd", "webimage" "webicon300", "webicon200", "webicon100"

    public string SearchFilter { get; set; } = string.Empty; // текстовый фильтр поиска фильмов

    public bool MovieInMainList { get; set; } = true; // true - отображать фильмы с параметром InMainList=true, false - все найденные фильмы

    public bool IsPartsMoreOne { get; set; } = true; // true - формировать ссылку на страницу серий фильма (если фильм многосерийный)
}