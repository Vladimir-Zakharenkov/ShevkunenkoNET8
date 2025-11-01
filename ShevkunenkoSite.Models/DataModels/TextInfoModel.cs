namespace ShevkunenkoSite.Models.DataModels;

public class TextInfoModel
{
    #region Идентификатор текста

    [Display(Name = "TextInfoId:")]
    [Column("TextInfoId")]
    public Guid TextInfoModelId { get; set; }

    #endregion

    #region Описание текста

    [Required(ErrorMessage = "Добавьте описание текста")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание текста :")]
    public string TextDescription { get; set; } = string.Empty;

    #endregion

    #region Файл TXT

    [DataType(DataType.Text)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Файл (txt) :")]
    public string TxtFileName { get; set; } = string.Empty;

    [Display(Name = "Размер файла (txt) :")]
    public int TxtFileSize { get; set; }

    #endregion

    #region Файл HTML

    [DataType(DataType.Text)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Файл (html) :")]
    public string HtmlFileName { get; set; } = string.Empty;

    [Display(Name = "Размер файла (html) :")]
    public int HtmlFileSize { get; set; }

    #endregion

    #region Папка для текста

    [DataType(DataType.Text)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Папка :")]
    public string FolderForText { get; set; } = string.Empty;

    #endregion

    #region Номер страницы книги (статьи)

    [Display(Name = "Номер страницы :")]
    [Range(0, 3000)]
    public int? SequenceNumber { get; set; }

    #endregion

    #region Связанные книга или статья

    [Display(Name = "Связанная книга (статья) :")]
    public Guid? BooksAndArticlesModelId { get; set; }
    public BooksAndArticlesModel? BooksAndArticlesModel { get; set; }

    #endregion

    #region Связанный аудиофайл

    public Guid? AudioFileForTextId { get; set; }
    public AudioInfoModel? AudioFileForText { get; set; }

    #endregion

    #region Текст аудиофайла без разметки (NotMapped)

    [NotMapped]
    public string? ClearText { get; set; }

    #endregion

    #region Текст аудиофайла с разметкой (NotMapped)

    [NotMapped]
    public string? HtmlText { get; set; }

    #endregion
}