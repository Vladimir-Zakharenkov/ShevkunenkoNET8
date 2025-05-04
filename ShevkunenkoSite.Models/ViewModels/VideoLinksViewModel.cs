namespace ShevkunenkoSite.Models.ViewModels;

public class VideoLinksViewModel
{
    // Заголовок над ссылками на видео
    public string HeadTitleForVideoLinks { get; set; } = string.Empty;

    // true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео
    public bool? IsImage { get; set; }

    // "hd", "image" "icon300", "icon200", "icon100", "webhd", "webimage" "webicon300", "webicon200", "webicon100"
    public string IconType { get; set; } = string.Empty;

    // Текстовый фильтр поиска фильмов
    public string SearchFilter { get; set; } = string.Empty;

    // true -> отображать фильмы с параметром InMainList=true, false -> все найденные фильмы
    public bool MovieInMainList { get; set; } = true;

    // true - ссылка на страницу серий фильма (если фильм многосерийный)
    public bool IsPartsMoreOne { get; set; } = true;
}