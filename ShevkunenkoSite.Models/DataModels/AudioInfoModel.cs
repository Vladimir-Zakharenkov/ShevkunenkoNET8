namespace ShevkunenkoSite.Models.DataModels;

public class AudioInfoModel
{
    #region Идентификатор аудиофайла

    [Key]
    [Display(Name = "AudioInfoId:")]
    [Column("AudioInfoId")]
    public Guid AudioInfoModelId { get; set; }

    #endregion

    #region Автор текста

    [Required(ErrorMessage = "Введите автора текста", AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Автор текста :")]
    public string AuthorOfText { get; set; } = string.Empty;

    #endregion

    #region Название текста аудиофайла

    [Required(ErrorMessage = "Заголовок текста в аудиофайле", AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Заголовок :")]
    public string CaptionOfTextInAudioFile { get; set; } = string.Empty;

    #endregion

    #region Transcript - guid файла с текстом

    [Display(Name = "Текст :")]
    public Guid? TextInfoModelId { get; set; }
    public TextInfoModel? TextInfoModel { get; set; }

    #endregion

    #region Описание содержания аудиофайла

    [Required(ErrorMessage = "Добавьте описание аудиофайла")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание аудиофайла :")]
    public string AudioFileDescription { get; set; } = string.Empty;

    #endregion

    #region Аудиокнига - Номер файла по порядку

    // идентификатор аудиокниги
    [Display(Name = "Аудиокнига :")]
    public Guid? AudioBookModelId { get; set; }
    public AudioBookModel? AudioBookModel { get; set; }

    // номер страницы аудиокниги
    [Display(Name = "Номер по порядку :")]
    [Range(1, 300)]
    public int? SequenceNumber { get; set; }

    #endregion

    #region Значение для сортировки

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Индекс сортировки:")]
    [Range(1, 100000)]
    public int SortOfAudioFile { get; set; } = 1;

    #endregion

    #region Папка аудиофайла

    [DataType(DataType.Text)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Папка аудиофайла:")]
    public string FolderForAudioFile { get; set; } = string.Empty;

    #endregion

    #region Автоматически вычисляемые свойства аудиофайла

    [Column(TypeName = "bigint")]
    public TimeSpan AudioFileDuration { get; set; }

    public int AudioFileBitRate { get; set; }

    public int AudioFileFrequency { get; set; }

    public int AudioFileSize { get; set; }

    public string AudioFileMimeType { get; set; } = string.Empty;

    public string AudioFileType { get; set; } = string.Empty;

    [Display(Name = "Имя файла :")]
    public string AudioFileName { get; set; } = string.Empty;

    #endregion

    #region Ссылки на аудиофайл в интернете

    [DataType(DataType.Url)]
    [Display(Name = "Ссылка на аудиофайл: ")]
    public Uri? InternetRefToAudioFile { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Ссылка на аудиофайл: ")]
    public Uri? PodsterFmRefToAudioFile { get; set; }

    #endregion

    #region Дата загрузки

    [Required(ErrorMessage = "Введите дату загрузки на сервер")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата публикации на сайте:")]
    public DateTime AudioFileUploadDate { get; set; } = DateTime.Today;

    #endregion

    #region Связанная страница сайта

    // идентификатор страницы сайта
    [Display(Name = "Страница аудиофайла :")]
    public Guid? PageInfoModelId { get; set; }
    public PageInfoModel? PageInfoModel { get; set; }

    #endregion

    #region Текст аудиофайла (NotMapped)

    [NotMapped]
    public string? ClearText { get; set; }

    #endregion
}