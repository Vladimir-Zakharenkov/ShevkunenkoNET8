// Ignore Spelling: Org

namespace ShevkunenkoSite.Models.ViewModels;

public class MoviesListViewModel
{
    public IEnumerable<MovieFileModel> Movies { get; set; } = [];

    public IEnumerable<PageInfoModel> MoviePages { get; set; } = [];

    public PagingInfoViewModel PagingInfo { get; set; } = new();

    // true - выбираем главную страницу многосерийного фильма
    public bool IsPartsMoreOne { get; set; } = true;

    public string MovieCaptionSearchString { get; set; } = string.Empty;

    public string MovieDescriptionForSchemaOrgSearchString { get; set; } = string.Empty;

    public string MovieGenreSearchString { get; set; } = string.Empty;

    public string MovieDirectorSearchString { get; set; } = string.Empty;

    public string MovieMusicBySearchString { get; set; } = string.Empty;

    public string MovieActorSearchString { get; set; } = string.Empty;

    public string PageHeadTitle { get; set; } = string.Empty;

    public string ActionForPagination { get; set; } = string.Empty;

    // true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео
    public bool? IsImage { get; set; } = false;

    // "hd", "image", "icon300", "icon200", "icon100", "webhd", "webimage", "webicon300", "webicon200", "webicon100" 
    public string IconType { get; set; } = string.Empty;

    // true -> все найденные фильмы, false - фильмы с параметром InMainList (true)
    public bool AllMoviesFromDB { get; set; } = true;

    // картинка страницы серий для многосерийного фильма
    public Guid? HeadImageSeries { get; set; }
}