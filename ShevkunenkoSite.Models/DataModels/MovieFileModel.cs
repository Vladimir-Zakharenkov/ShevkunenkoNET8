// Ignore Spelling: Org Рroduction Imbd Poisk

namespace ShevkunenkoSite.Models.DataModels;

public class MovieFileModel
{
    [Display(Name = "Идентификатор фильма :")]
    [Column("MovieFileId")]
    public Guid MovieFileModelId { get; set; } = Guid.Empty;

    #region Description

    [Required(ErrorMessage = "Введите название фильма")]
    [DataType(DataType.Text)]
    [Display(Name = "Название фильма:")]
    public string MovieCaption { get; set; } = string.Empty;

    [Display(Name = "Включать фильм в список видео:")]
    public bool MovieInMainList { get; set; } = true;

    [Required(ErrorMessage = "Введите заголовок страницы")]
    [DataType(DataType.Text)]
    [Display(Name = "Заголовок страницы фильма: ")]
    public string MovieCaptionForOnline { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите содержание фильма")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Краткое содержание:")]
    public string MovieDescriptionForSchemaOrg { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите содержание фильма (HTML)")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Краткое содержание (HTML):")]
    public string MovieDescriptionHtml { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Примечания: ")]
    public string MovieNote { get; set; } = string.Empty;

    #endregion

    #region Автозаполнение

    //[Required(ErrorMessage = "Введите продолжительность фильма")]
    [DataType(DataType.Duration)]
    public TimeSpan MovieDuration { get; set; }

    //[Required(ErrorMessage = "Введите ширину кадра")]
    [DataType(DataType.Text)]
    public uint MovieWidth { get; set; }

    //[Required(ErrorMessage = "Введите высоту кадра")]
    [DataType(DataType.Text)]
    public uint MovieHeight { get; set; }

    //[Required(ErrorMessage = "Введите имя файла", AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    public string MovieFileName { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Введите расширение файла", AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    public string MovieFileExtension { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Введите MIME Type файла", AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    public string MovieMimeType { get; set; } = string.Empty;

    //[Required(ErrorMessage = "Введите размер файла")]
    [DataType(DataType.Text)]
    public ulong MovieFileSize { get; set; }

    #endregion

    #region Ограничения и поиск

    [DataType(DataType.Text)]
    [Display(Name = "Формат изображения:")]
    public string MovieScreenFormat { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите жанр фильма")]
    [DataType(DataType.Text)]
    [Display(Name = "Жанр фильма: ")]
    public string MovieGenre { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Фильтр поиска: ")]
    public string SearchFilter { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Список тем: ")]
    public string TopicGuidList { get; set; } = string.Empty;

    [Required(ErrorMessage = "Выберите ограничения по возрасту")]
    [Display(Name = "Нет ограничений по возрасту:")]
    public bool MovieIsFamilyFriendly { get; set; } = true;

    [Display(Name = "Фильм для взрослых (18+):")]
    public bool MovieAdult { get; set; } = false;

    [Display(Name = "Полная версия фильма:")]
    public Guid? FullMovieID { get; set; } // идентификатор полной версии фильма

    #endregion

    #region Dates

    [Required(ErrorMessage = "Введите дату создания")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата создания фильма:")]
    public DateTime MovieDateCreated { get; set; }

    [Required(ErrorMessage = "Введите дату примьеры")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата примьеры фильма:")]
    public DateTime MovieDatePublished { get; set; }

    [Required(ErrorMessage = "Введите дату загрузки на сервер")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата публикации на сайте:")]
    public DateTime MovieUploadDate { get; set; }

    #endregion

    #region MovieLanguage

    [Required(ErrorMessage = "Введите язык звуковой дорожки")]
    [DataType(DataType.Text)]
    [Display(Name = "Звуковая дорожка (1):")]
    public string MovieInLanguage1 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Звуковая дорожка (2):")]
    public string MovieInLanguage2 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Язык субтитров (1):")]
    public string MovieSubtitles1 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Язык субтитров (2):")]
    public string MovieSubtitles2 { get; set; } = string.Empty;

    #endregion

    #region FilmCrew

    [Required(ErrorMessage = "Введите название кинокомпании")]
    [DataType(DataType.Text)]
    [Display(Name = "Кинокомпания:")]
    public string MovieРroductionCompany { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Режиссер: ")]
    public string MovieDirector1 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Режиссер: ")]
    public string MovieDirector2 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Композитор: ")]
    public string MovieMusicBy { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor01 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor02 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor03 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor04 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor05 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor06 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor07 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor08 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor09 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Актёр (актриса): ")]
    public string MovieActor10 { get; set; } = string.Empty;

    #endregion

    #region RefsForMovie

    [DataType(DataType.Url)]
    [Display(Name = "Адрес файла в интернете: ")]
    public Uri? MovieContentUrl { get; set; } = null!;

    [DataType(DataType.Url)]
    [Display(Name = "Фильм на YouTube: ")]
    public Uri? MovieYouTube { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Фильм на VK: ")]
    public Uri? MovieVkVideo { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Фильм на MailRu: ")]
    public Uri? MovieMailRuVideo { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Фильм на OK: ")]
    public Uri? MovieOkVideo { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Фильм на Яндекс Диск: ")]
    public Uri? MovieYandexDiskVideo { get; set; }

    #endregion

    #region InforForMovie

    [DataType(DataType.Url)]
    [Display(Name = "Kino-Teatr: ")]
    public Uri? MovieKinoTeatrRu { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Кино-Поиск: ")]
    public Uri? MovieKinoPoisk { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "IMBD: ")]
    public Uri? MovieImbd { get; set; }

    #endregion

    #region Многосерийный фильм

    [Required(ErrorMessage = "Введите количество серий")]
    [Range(1, 100)]
    [Display(Name = "Количество серий:")]
    public uint MovieTotalParts { get; set; } = 1;

    [Required(ErrorMessage = "Введите номер серии фильма")]
    [Range(1, 100)]
    [Display(Name = "Номер серии фильма:")]
    public uint MoviePart { get; set; } = 1;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Фильтр поиска серий: ")]
    public string SeriesSearchFilter { get; set; } = string.Empty;

    public Guid? PageInfoModelIdForSeries { get; set; } // страница фильма из нескольких серий

    public Guid? ImageForHeadSeriesImageFileModelId { get; set; } // картинка заголовка страницы серий
    public ImageFileModel? ImageForHeadSeries { get; set; }

    #endregion

    #region PageForMovie

    public Guid? PageInfoModelId { get; set; } // страница фильма
    public PageInfoModel? PageInfoModel { get; set; }

    #endregion

    #region PictureForMovie

    public Guid? ImageFileModelId { get; set; } // картинка для карточки фильма
    public ImageFileModel? ImageFileModel { get; set; }

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Постер к фильму FileName:")] // постер к фильму FileName
    public string? MoviePoster { get; set; }

    [Display(Name = "Постер к фильму Guid:")] // постер к фильм Guid
    public Guid? MoviePosterGuid { get; set; }

    [Display(Name = "Карусель кадров фильма:")] // карусель кадров фильма
    public bool Carousel { get; set; } = false;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Название фильма для ленты кадров:")]
    public string FramesAroundMovie { get; set; } = string.Empty;

    #endregion

    #region UnderMovie1

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Заголовок (1) связанных видео")]
    public string HeadTitleForVideoLinks1 { get; set; } = string.Empty; // заголовок 1 связанных видео

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Фильтр поиска (1)  связанных видео")]
    public string SearchFilter1 { get; set; } = string.Empty;

    [Display(Name = "Выбрать постер или картинку для видео")]
    // true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео
    public bool? IsImage1 { get; set; } = false;

    [Display(Name = "Выбрать параметры изображения")]
    // "hd", "icon300", "icon200", "icon100", "image", "webhd", "webicon300", "webicon200", "webicon100", "webimage" 
    public string IconType1 { get; set; } = string.Empty;

    [Display(Name = "Выбрать ссылку на страницу фильма")]
    // true -> определяем путь к главной странице многосерийного фильма
    public bool IsPartsMoreOne1 { get; set; } = true;

    [Display(Name = "Все найденные фильмы")]
    // true -> все найденные фильмы, false - фильмы с параметром InMainList (true)
    public bool AllMoviesFromDB1 { get; set; } = true;

    #endregion

    #region UnderMovie2

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Заголовок (2) связанных видео")]
    public string HeadTitleForVideoLinks2 { get; set; } = string.Empty; // заголовок 2 связанных видео

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Фильтр поиска (2)  связанных видео ")]
    public string SearchFilter2 { get; set; } = string.Empty;

    [Display(Name = "Выбрать постер или картинку для видео")]
    // true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео
    public bool? IsImage2 { get; set; } = false;

    [Display(Name = "Выбрать параметры изображения")]
    // "hd", "icon300", "icon200", "icon100", "image", "webhd", "webicon300", "webicon200", "webicon100", "webimage" 
    public string IconType2 { get; set; } = string.Empty;

    [Display(Name = "Выбрать ссылку на страницу фильма")]
    // true -> определяем путь к главной странице многосерийного фильма
    public bool IsPartsMoreOne2 { get; set; } = true;

    [Display(Name = "Все найденные фильмы")]
    // true -> все найденные фильмы, false - фильмы с параметром InMainList (true)
    public bool AllMoviesFromDB2 { get; set; } = true;

    #endregion

    #region UnderMovie3

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Заголовок (3) связанных видео")]
    public string HeadTitleForVideoLinks3 { get; set; } = string.Empty; // заголовок 2 связанных видео

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Фильтр поиска (3)  связанных видео ")]
    public string SearchFilter3 { get; set; } = string.Empty;

    [Display(Name = "Выбрать постер или картинку для видео")]
    // true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео
    public bool? IsImage3 { get; set; } = false;

    [Display(Name = "Выбрать параметры изображения")]
    // "hd", "icon300", "icon200", "icon100", "image", "webhd", "webicon300", "webicon200", "webicon100", "webimage" 
    public string IconType3 { get; set; } = string.Empty;

    [Display(Name = "Выбрать ссылку на страницу фильма")]
    // true -> определяем путь к главной странице многосерийного фильма
    public bool IsPartsMoreOne3 { get; set; } = true;

    [Display(Name = "Все найденные фильмы")]
    // true -> все найденные фильмы, false - фильмы с параметром InMainList (true)
    public bool AllMoviesFromDB3 { get; set; } = true;

    #endregion
}