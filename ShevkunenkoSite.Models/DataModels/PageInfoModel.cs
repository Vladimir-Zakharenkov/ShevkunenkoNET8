// Ignore Spelling: Loc Lastmod Changefreq Og

namespace ShevkunenkoSite.Models.DataModels;

public class PageInfoModel
{
    #region Список областей (Area)

    [NotMapped]
    public string[] AreaItems =
    [
        "Root",
        "Admin",
        "Movies",
        "Rybakov"
    ];

    #endregion

    #region Идентификатор страницы в базе данных

    [Display(Name = "PageInfoId:")]
    [Column("PageInfoId")]
    public Guid PageInfoModelId { get; set; }

    #endregion

    #region Адрес страницы

    [Required(ErrorMessage = "Выберите тип MVC или RazorPage")]
    [Display(Name = "Тип MVC или RazorPage :")]
    public bool PageAsRazorPage { get; set; } = false;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Область :")]
    [DataType(DataType.Text)]
    public string PageArea { get; set; } = "Root";

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Контроллер :")]
    [DataType(DataType.Text)]
    public string Controller { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Действие :")]
    [DataType(DataType.Text)]
    public string Action { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Данные :")]
    [DataType(DataType.Text)]
    public string RoutData { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    public string PageFullPath { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    public string PageFullPathWithData { get; set; } = string.Empty;

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Псевдоним адреса (1) :")]
    [DataType(DataType.Text)]
    public string PagePathNickName { get; set; } = string.Empty;

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Псевдоним адреса (2) :")]
    [DataType(DataType.Text)]
    public string PagePathNickName2 { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Псевдоним адреса :")]
    [DataType(DataType.Text)]
    public string PagePathNickNameWithData { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Адрес страницы :")]
    [DataType(DataType.Text)]
    public string PageLoc { get; set; } = string.Empty;

    #endregion

    #region Теги title, description, keywords

    [Required(ErrorMessage = "Добавьте заголовок страницы")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Заголовок страницы :")]
    public string PageTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Добавте описание")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание страницы :")]
    public string PageDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Добавте ключевые слова")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Ключевые слова :")]
    public string PageKeyWords { get; set; } = string.Empty;

    #endregion

    #region SiteMap

    [Required(ErrorMessage = "Укажите дату изменения")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата изменения :")]
    public DateTime PageLastmod { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Частота изменения :")]
    [DataType(DataType.Text)]
    [MaxLength(7)]
    public string Changefreq { get; set; } = "monthly";

    [Required(ErrorMessage = "Выберите значения от 0.1 до 1.0")]
    [Display(Name = "Приоритет страницы :")]
    [DataType(DataType.Text)]
    [MaxLength(3)]
    public string Priority { get; set; } = "0.5";

    #endregion

    #region BrowserConfig и т.п.

    [Required(ErrorMessage = "Необходимо указать файл browserconfig")]
    [Display(Name = "Файл browserconfig :")]
    [DataType(DataType.Text)]
    public string BrowserConfig { get; set; } = "main.xml";

    [Required(ErrorMessage = "Укажите каталог иконок browserconfig")]
    [Display(Name = "Каталог иконок browserconfig :")]
    [DataType(DataType.Text)]
    public string BrowserConfigFolder { get; set; } = "main";

    [Required(ErrorMessage = "Необходимо указать файл manifest")]
    [Display(Name = "Файл manifest :")]
    [DataType(DataType.Text)]
    public string Manifest { get; set; } = "main.json";

    [Required(ErrorMessage = "Выберите папку с иконками")]
    [Display(Name = "Папка с иконками :")]
    [DataType(DataType.Text)]
    public string PageIconPath { get; set; } = "0.5";

    #endregion

    #region OpenGraph

    [Required(ErrorMessage = "Необходимо указать og:type")]
    [Display(Name = "Open Graph :")]
    [DataType(DataType.Text)]
    public string OgType { get; set; } = "website";

    #endregion

    #region Фон страницы (фотопленка)

    public Guid BackgroundFileModelId { get; set; }
    public BackgroundFileModel? BackgroundFileModel { get; set; }

    #endregion

    #region Карточка страницы

    // Картинка страницы
    public Guid ImageFileModelId { get; set; }
    public ImageFileModel? ImageFileModel { get; set; }

    // Текст карточки страницы
    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Текст карточки :")]
    [DataType(DataType.Text)]
    public string PageCardText { get; set; } = string.Empty;

    #endregion

    #region Заголовок, картинка и текст страницы

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Заголовок страницы (<h1>...</h1>) :")]
    [DataType(DataType.Text)]
    public string PageHeading { get; set; } = string.Empty;

    [Display(Name = "Картинка страницы :")]
    public Guid? ImagePageHeadingId { get; set; }
    public ImageFileModel? ImagePageHeading { get; set; }

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Текст (HTML) :")]
    [DataType(DataType.Text)]
    public string TextOfPage { get; set; } = string.Empty;

    #endregion

    #region Фильтры поиска текущей страницы

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Фильтр поиска :")]
    [DataType(DataType.Text)]
    public string PageFilter { get; set; } = string.Empty;

    #endregion

    #region Значение для сортировки страницы

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Индекс сортировки:")]
    public int SortOfPage { get; set; } = 1;

    #endregion

    #region Связанные фотографии, страницы и видео

    #region Ссылки на связанные фото

    #region Включить ссылки на фото

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Ссылки на фото :")]
    public bool PhotoLinks { get; set; } = false;

    #endregion

    #region Список фильтров фото, для формирования ссылок на них

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Фильтры поиска фото :")]
    [DataType(DataType.Text)]
    public string PhotoFilterOut { get; set; } = string.Empty;

    #endregion

    #endregion

    #region Ссылки по списку GUID1

    #region Включить ссылки по GUID1

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Ссылки по GUID (1) :")]
    public bool PageLinks { get; set; } = false;

    #endregion

    #region Список GUID1 страниц

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Ссылки по GUID (1) :")]
    public string RefPages { get; set; } = string.Empty;

    #endregion

    #endregion

    #region Ссылки по GUID2

    #region Включить ссылки по GUID2

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Ссылки по GUID (2) :")]
    public bool PageLinks2 { get; set; } = false;

    #endregion

    #region Список GUID2 страниц

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Ссылки по GUID (2) :")]
    public string RefPages2 { get; set; } = string.Empty;

    #endregion

    #endregion

    #region Ссылки на видео

    #region Включить ссылки на видео

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Ссылки на видео :")]
    public bool VideoLinks { get; set; } = false;

    #endregion

    #region Список фильтров видео, для формирования ссылок на них

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Фильтры поиска видео :")]
    [DataType(DataType.Text)]
    public string VideoFilterOut { get; set; } = string.Empty;

    #endregion

    #endregion

    #region Ссылки на страницы

    #region Включить ссылки на страницы по текстовому фильтру

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Ссылки на страницы :")]
    public bool PageLinksByFilters { get; set; } = false;

    #endregion

    #region Список фильтров страниц сайта, для формирования ссылок на них

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Фильтры поиска страниц :")]
    [DataType(DataType.Text)]
    public string PageFilterOut { get; set; } = string.Empty;

    #endregion

    #endregion

    #endregion

    #region Навигационное свойство MovieFileModel

    public MovieFileModel? MovieFile { get; set; }

    #endregion

    #region Навигационное свойство книги или статьи

    public BooksAndArticlesModel? BooksAndArticles { get; set; }

    #endregion

    #region Навигационное свойство аудиокниги

    public AudioBookModel? AudioBook { get; set; }

    #endregion

}