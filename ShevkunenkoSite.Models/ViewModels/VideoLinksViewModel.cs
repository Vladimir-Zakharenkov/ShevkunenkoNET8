namespace ShevkunenkoSite.Models.ViewModels;

public class VideoLinksViewModel
{
    #region Заголовок над ссылками на видео

    public string HeadTitleForVideoLinks { get; set; } = string.Empty;

    #endregion

    #region Вид изображения ссылки (true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео)

    public bool? IsImage { get; set; }

    #endregion

    #region Параметры картинки ("hd", "image" "icon300", "icon200", "icon100", "webhd", "webimage" "webicon300", "webicon200", "webicon100")

    public string IconType { get; set; } = string.Empty;

    #endregion

    #region Текстовый фильтр поиска фильмов

    public string SearchFilter { get; set; } = string.Empty;

    #endregion

    #region Отображать все фильмы ( InMainList=false) или с параметром (InMainList=true)

    public bool MovieInMainList { get; set; } = true;

    #endregion

    #region Ссылка на страницу серий фильма (true) если фильм многосерийный

    public bool IsPartsMoreOne { get; set; } = true;

    #endregion
}