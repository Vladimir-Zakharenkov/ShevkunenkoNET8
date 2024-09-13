// Ignore Spelling: Loc Lastmod Changefreq Og

namespace ShevkunenkoSite.Models.DataModels;

public class PageInfoModel
{
    [Display(Name = "PageInfoId:")]
    [Column("PageInfoId")]
    public Guid PageInfoModelId { get; set; }

    [NotMapped]
    public string[] AreaItems =
    [
        "",
        "Admin",
        "Movies",
        "Rybakov"
    ];

    #region Описание

    [Required(ErrorMessage = "Добавьте заголовок страницы")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Заголовок страницы :")]
    public string PageTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Добавте описание")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание страницы:")]
    public string PageDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Добавте ключевые слова")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Ключевые слова:")]
    public string PageKeyWords { get; set; } = string.Empty;

    #endregion

    #region Адрес страницы

    [Required(ErrorMessage = "Отметьте тип страницы")]
    [Display(Name = "RazorPage:")]
    public bool PageAsRazorPage { get; set; } = false;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Область:")]
    [DataType(DataType.Text)]
    public string PageArea { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Контроллер:")]
    [DataType(DataType.Text)]
    public string Controller { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Действие:")]
    [DataType(DataType.Text)]
    public string Action { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Данные:")]
    [DataType(DataType.Text)]
    public string RoutData { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    public string PageFullPath { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    public string PageFullPathWithData { get; set; } = string.Empty;

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Псевдоним адреса страницы:")]
    [DataType(DataType.Text)]
    public string PagePathNickName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Псевдоним адреса страницы:")]
    [DataType(DataType.Text)]
    public string PagePathNickNameWithData { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Адрес страницы:")]
    [DataType(DataType.Text)]
    public string PageLoc { get; set; } = string.Empty;

    #endregion

    #region SiteMap

    [Required(ErrorMessage = "Укажите дату изменения")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата изменения:")]
    public DateTime PageLastmod { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Частота изменения:")]
    [DataType(DataType.Text)]
    [MaxLength(7)]
    public string Changefreq { get; set; } = "monthly";

    [Required(ErrorMessage = "Выберите значения от 0.1 до 1.0")]
    [Display(Name = "Приоритет страницы:")]
    [DataType(DataType.Text)]
    [MaxLength(3)]
    public string Priority { get; set; } = "0.5";

    #endregion

    #region BrowserConfig и т.п.

    [Required(ErrorMessage = "Необходимо указать файл browserconfig")]
    [Display(Name = "Файл browserconfig: ")]
    [DataType(DataType.Text)]
    public string BrowserConfig { get; set; } = "main.xml";

    [Required(ErrorMessage = "Укажите каталог иконок browserconfig")]
    [Display(Name = "Каталог иконок browserconfig: ")]
    [DataType(DataType.Text)]
    public string BrowserConfigFolder { get; set; } = "main";

    [Required(ErrorMessage = "Необходимо указать файл manifest")]
    [Display(Name = "Файл manifest: ")]
    [DataType(DataType.Text)]
    public string Manifest { get; set; } = "main.json";

    [Required(ErrorMessage = "Выберите папку с иконками")]
    [Display(Name = "Папка с иконками:")]
    [DataType(DataType.Text)]
    public string PageIconPath { get; set; } = "0.5";

    #endregion

    #region OpenGraph

    [Required(ErrorMessage = "Необходимо указать og:type")]
    [Display(Name = "Open Graph")]
    [DataType(DataType.Text)]
    public string OgType { get; set; } = "website";

    #endregion

    #region Оформление

    // Текст карточки страницы
    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Текст карточки:")]
    [DataType(DataType.Text)]
    public string PageCardText { get; set; } = string.Empty;

    // Фон страницы (фотопленка)
    public Guid BackgroundFileModelId { get; set; }
    public BackgroundFileModel? BackgroundFileModel { get; set; }

    // Картинка страницы
    public Guid ImageFileModelId { get; set; }
    public ImageFileModel? ImageFileModel { get; set; }

    #endregion

    #region Содержание страницы

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Заголовок страницы (<h1>...</h1>):")]
    [DataType(DataType.Text)]
    public string PageHeading { get; set; } = string.Empty;

    [Display(Name = "Картинка заголовка страницы:")]
    public Guid? ImagePageHeadingId { get; set; } = null;
    public ImageFileModel? ImagePageHeading { get; set; }

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Текст страницы (<article>...</article>):")]
    [DataType(DataType.Text)]
    public string TextOfPage { get; set; } = string.Empty;

    #endregion

    #region Связанные страницы и видео

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Включить первые ссылки по GUID:")]
    public bool PageLinks { get; set; } = false;

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Включить вторые ссылки по GUID:")]
    public bool PageLinks2 { get; set; } = false;

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Включить ссылки на видео:")]
    public bool VideoLinks { get; set; } = false;

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Включить ссылки по фильтрам:")]
    public bool PageLinksByFilters { get; set; } = false;

    // строка по которой осуществляется поиск текущей страницы
    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Фильтр поиска текущей страницы:")]
    [DataType(DataType.Text)]
    public string PageFilter { get; set; } = string.Empty;

    // список фильтров (PageFilter) страниц сайта, для формирования ссылок на них
    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Фильтры поиска страниц сайта:")]
    [DataType(DataType.Text)]
    public string PageFilterOut { get; set; } = string.Empty;

    // список фильтров (SearchFilter) видео, для формирования ссылок на них
    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Фильтры поиска видео:")]
    [DataType(DataType.Text)]
    public string VideoFilterOut { get; set; } = string.Empty;

    // список GUID страниц, для формирования ссылок на них
    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Ссылки на страницы по GUID (1):")]
    public string RefPages { get; set; } = string.Empty;

    // второй список GUID страниц, для формирования ссылок на них
    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Ссылки на страницы по GUID (2):")]
    public string RefPages2 { get; set; } = string.Empty;

    #endregion

    // навигационное свойство
    public MovieFileModel? MovieFile { get; set; }
}