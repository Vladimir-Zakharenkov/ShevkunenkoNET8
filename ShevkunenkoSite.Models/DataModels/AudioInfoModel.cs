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
    [Display(Name = "Продолжительность :")]
    public TimeSpan AudioFileDuration { get; set; }

    [Display(Name = "Битрейт :")]
    public int AudioFileBitRate { get; set; }

    [Display(Name = "Частота дискредитации :")]
    public int AudioFileFrequency { get; set; }

    [Display(Name = "Размер :")]
    public int AudioFileSize { get; set; }

    [Display(Name = "Тип MIME :")]
    public string AudioFileMimeType { get; set; } = string.Empty;

    [Display(Name = "Тип файла :")]
    public string AudioFileType { get; set; } = string.Empty;

    [Display(Name = "Имя файла :")]
    public string AudioFileName { get; set; } = string.Empty;

    #endregion

    #region Ссылки на аудиофайл в интернете

    [DataType(DataType.Url)]
    [Display(Name = "Сайт sergeyshef.ru : ")]
    public Uri? InternetRefToAudioFile { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Сайт podster.fm : ")]
    public Uri? PodsterFmRefToAudioFile { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Сайт disk.yandex.ru : ")]
    public Uri? YandexDiskRefToAudioFile { get; set; }

    #endregion

    #region Код плейера podster.fm

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Код плейера :")]
    public string? PlayerPodsterFm { get; set; }

    #endregion

    #region Дата загрузки

    [Required(ErrorMessage = "Введите дату загрузки на сервер")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата публикации :")]
    public DateTime AudioFileUploadDate { get; set; } = DateTime.Today;

    #endregion

    #region Transcript - GUID текста аудиофайла

    [Display(Name = "Transcript :")]
    public Guid? TranscriptId { get; set; }

    [NotMapped]
    public TextInfoModel? Transcript { get; set; }
    #endregion

    #region Текст аудиофайла (NotMapped)

    [NotMapped]
    public string? ClearText { get; set; }

    #endregion

    #region Выбрать аудиофайл (NotMapped)

    [NotMapped]
    [DataType(DataType.Upload)]
    [Display(Name = "Выбрать аудиофайл:")]
    public IFormFile? ChooseAudioFile { get; set; }

    #endregion
}