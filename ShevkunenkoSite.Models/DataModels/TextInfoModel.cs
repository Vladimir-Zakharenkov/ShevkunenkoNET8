namespace ShevkunenkoSite.Models.DataModels;

public class TextInfoModel
{
    // идентификатор текста
    [Display(Name = "TextInfoId:")]
    [Column("TextInfoId")]
    public Guid TextInfoModelId { get; set; }

    // описание текста
    [Required(ErrorMessage = "Добавьте описание текста")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание текста :")]
    public string TextDescription { get; set; } = string.Empty;

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

    #region Связанные книга или статья

    [Display(Name = "Страница книги (статьи) :")]
    public Guid? BooksAndArticlesModelId { get; set; }
    public BooksAndArticlesModel? BooksAndArticlesModel { get; set; }

    // номер страницы книги (статьи)
    [Display(Name = "Номер страницы :")]
    [Range(0,3000)]
    public int? SequenceNumber { get; set; }

    #endregion
}