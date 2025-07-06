// Ignore Spelling: Org

namespace ShevkunenkoSite.Models.ViewModels;

public class MoviesListViewModel
{
    #region Коллекция экземпляров видео

    public IEnumerable<MovieFileModel> Movies { get; set; } = [];

    #endregion

    #region Коллекция экземпляров страниц

    public IEnumerable<PageInfoModel> MoviePages { get; set; } = [];

    #endregion

    #region Информация для постраничной разбивки

    public PagingInfoViewModel PagingInfo { get; set; } = new();

    #endregion

    #region Действие в контроллере к которому применяется постраничная  разбивка

    public string ActionForPagination { get; set; } = string.Empty;

    #endregion

    #region Если значение true выбираем страницу серий (многосерийного фильма), если false выбираем страницу текущей серии

    public bool IsPartsMoreOne { get; set; } = true;

    #endregion

    #region Параметры поиска

    public string MovieCaptionSearchString { get; set; } = string.Empty;

    public string MovieDescriptionForSchemaOrgSearchString { get; set; } = string.Empty;

    public string MovieGenreSearchString { get; set; } = string.Empty;

    public string MovieDirectorSearchString { get; set; } = string.Empty;

    public string MovieMusicBySearchString { get; set; } = string.Empty;

    public string MovieActorSearchString { get; set; } = string.Empty;

    #endregion

    #region Заголовок над ссылками

    public string PageHeadTitle { get; set; } = string.Empty;

    #endregion

    #region Вид картинки для ссылки (true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео)

    public bool? IsImage { get; set; } = false;

    #endregion

    #region Параметры картинки для ссылки ("hd", "image", "icon300", "icon200", "icon100", "webhd", "webimage", "webicon300", "webicon200", "webicon100" )

    public string IconType { get; set; } = string.Empty;

    #endregion

    #region Выборка из найденных экземпляров видео (true -> все найденные фильмы, false - фильмы с параметром InMainList ==true)

    public bool AllMoviesFromDB { get; set; } = true;

    #endregion

    #region Id картинки страницы серий для многосерийного фильма

    public Guid? HeadImageSeries { get; set; }

    #endregion

    #region Ссылка на страницу информации о фильме (true) или ссылка на страницу фильма (false)

    public bool? LinkToInfoAboutMovie { get; set; }

    #endregion
}