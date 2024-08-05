namespace ShevkunenkoSite.Models.ViewModels;

public class SeriesViewModel
{
    public Guid? HeadImageSeries {  get; set; }

    public MovieFileModel[] AllSeriesMovies { get; set; } = Array.Empty<MovieFileModel>();

    public string HeadTitleForVideoLinks { get; set; } = string.Empty;

    public bool? IsImage { get; set; } = null; // true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео

    public string IconType { get; set; } = string.Empty; // "hd", "image" "icon300", "icon200", "icon100", "webhd", "webimage" "webicon300", "webicon200", "webicon100"

    public bool MovieInMainList { get; set; } = false; // true - отображать фильмы с параметром InMainList=true, false - все найденные фильмы

    public bool IsPartsMoreOne { get; set; } = false; // true - ссылка на главную страницу фильма (если фильм многосерийный)
}