namespace ShevkunenkoSite.Models.DataModels;

public class BooksAndArticlesModel
{
    #region Guid книги или статьи

    [Key]
    [Display(Name = "Идентификатор издания :")]
    [Column("BooksArticlesId")]
    public Guid BooksAndArticlesModelId { get; set; } = Guid.Empty;

    #endregion

    #region Тип текста

    [DataType(DataType.Text)]
    [Display(Name = "Тип текста :")]
    public string TypeOfText { get; set; } = string.Empty;

    [NotMapped]
    public string[] TypesOfText =
        [
                "book",
                "article"
        ];

    #endregion

    #region Логотип статьи

    [Display(Name = "Логотип статьи :")]
    public Guid? LogoOfArticleId { get; set; }
    public ImageFileModel? LogoOfArticle { get; set; }

    #endregion

    #region Автор книги (статьи)

    [Required(ErrorMessage = "Введите автора текста", AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Автор текста :")]
    public string AuthorOfText { get; set; } = string.Empty;

    #endregion

    #region Название книги (статьи)

    [DataType(DataType.Text)]
    [Display(Name = "Название (заголовок) :")]
    public string CaptionOfText { get; set; } = string.Empty;

    #endregion

    #region Описание книги (статьи)

    [Required(ErrorMessage = "Добавте описание")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание книги (статьи):")]
    public string BookDescription { get; set; } = string.Empty;

    #endregion

    #region Количество страниц

    [Display(Name = "Количество страниц :")]
    public int? NumberOfPages { get; set; }

    #endregion
}