namespace ShevkunenkoSite.Models.DataModels;

public class AudioInfoModel
{
    #region Идентификатор аудиофайла

    [Key]
    [Display(Name = "AudioInfoId:")]
    [Column("AudioInfoId")]
    public Guid AudioInfoModelId { get; set; }

    #endregion

    #region Автор книги

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
    [Range(0, 300)]
    public int? SequenceNumber { get; set; }

    #endregion

    #region Значение для сортировки

    [Required(ErrorMessage = "Выберите значение")]
    [Display(Name = "Индекс сортировки:")]
    [Range(1, 100000)]
    public int SortOfAudioFile { get; set; } = 1;

    #endregion

    #region AutoFill

    [Column(TypeName = "bigint")]
    [Required(ErrorMessage = "Введите продолжительность аудиофайла")]
    public TimeSpan AudioFileDuration { get; set; }

    [Required(ErrorMessage = "Введите битрейт аудиофайла")]
    [Display(Name = "Битрейт аудиофайла :")]
    [Range(0, 1000000)]
    public int AudioFileBitRate { get; set; }

    [Required(ErrorMessage = "Введите частоту аудиофайла")]
    [Display(Name = "Частоту аудиофайла :")]
    [Range(0, 1000000)]
    public int AudioFileFrequency { get; set; }

    [Required(ErrorMessage = "Введите размер аудиофайла")]
    [Display(Name = "Размер аудиофайла :")]
    [Range(0, 10000000000)]
    public int AudioFileSize { get; set; }

    [Required(ErrorMessage = "Добавьте MIME Type аудиофайла")]
    [DataType(DataType.Text)]
    [Display(Name = "MIME Type аудиофайла :")]
    public string AudioFileMimeType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Добавьте расширение аудиофайла")]
    [DataType(DataType.Text)]
    [Display(Name = "Расширение аудиофайла :")]
    public string AudioFileType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Добавьте имя аудиофайла")]
    [DataType(DataType.Text)]
    [Display(Name = "Имя аудиофайла :")]
    public string AudioFileName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Добавьте тип воспроизведения аудиофайла", AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Тип воспроизведения :")]
    public string AudioFilePlaybackType { get; set; } = string.Empty;

    #endregion

    #region Ссылки на аудиофайл в интернете

    [DataType(DataType.Url)]
    [Display(Name = "Ссылка на аудиофайл: ")]
    public Uri? InternetRefToAudioFile { get; set; }

    #endregion

    #region Дата загрузки

    [Required(ErrorMessage = "Введите дату загрузки на сервер")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата публикации на сайте:")]
    public DateTime AudioFileUploadDate { get; set; } = DateTime.Today;

    #endregion

    #region Связанные страница сайта

    // идентификатор страницы сайта
    [Display(Name = "Страница аудиофайла :")]
    public Guid? PageInfoModelId { get; set; }
    public PageInfoModel? PageInfoModel { get; set; }

    #endregion
}