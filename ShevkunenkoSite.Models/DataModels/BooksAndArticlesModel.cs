﻿


// Ignore Spelling: Pdf

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
            "web",
            "book",
            "article"
        ];

    #endregion

    #region Издатель

    [Required(ErrorMessage = "Введите издателя текста", AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Издатель текста :")]
    public string Publisher { get; set; } = string.Empty;

    #endregion

    #region Логотип издания

    [Display(Name = "Логотип издания :")]
    [DataType(DataType.Upload)]
    public Guid? LogoOfArticleId { get; set; }
    public ImageFileModel? LogoOfArticle { get; set; }

    #endregion

    #region Скан статьи

    [Display(Name = "Скан статьи :")]
    public Guid? ScanOfArticleId { get; set; }
    public ImageFileModel? ScanOfArticle { get; set; }

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
    [Display(Name = "Заголовок :")]
    public string CaptionOfText { get; set; } = string.Empty;

    #endregion

    #region Подзаголовок книги (статьи)

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Подзаголовок :")]
    public string TheSubtitle { get; set; } = string.Empty;

    #endregion

    #region Описание книги (статьи)

    [Required(ErrorMessage = "Добавте описание")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание текста :")]
    public string BookDescription { get; set; } = string.Empty;

    #endregion

    #region Количество страниц

    [Display(Name = "Количество страниц :")]
    public int? NumberOfPages { get; set; }

    #endregion

    #region Дата публикации

    [Required(ErrorMessage = "Укажите дату публикации")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата публикации :")]
    public DateTime DateOfPublication { get; set; } = DateTime.Today;

    #endregion

    #region Ссылка на статью или издателя

    [DataType(DataType.Url)]
    [Display(Name = "Ссылка на статью :")]
    public Uri? UrlOfArticle { get; set; }
    #endregion

    #region Теги по содержанию книги (статьи)

    [Required(ErrorMessage = "Добавте теги")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Теги по содержанию :")]
    public string TagsForBook { get; set; } = string.Empty;

    #endregion

    #region Видео связанное с книгой (статьёй)

    [Display(Name = "Видео для текста :")]
    public Guid? VideoForBookOrArticleId { get; set; }
    public MovieFileModel? VideoForBookOrArticle { get; set; }

    #endregion

    #region Страница книги (оглавление) или статьи (первая страница)

    public Guid? PageInfoModelId { get; set; }
    public PageInfoModel? PageInfoModel { get; set; }

    #endregion

    #region Ссылка на документ Word

    [DataType(DataType.Url)]
    [Display(Name = "Документ Word : ")]
    public Uri? RefToWordDoc { get; set; }

    #endregion

    #region Ссылка на документ PDF

    [DataType(DataType.Url)]
    [Display(Name = "Документ PDF : ")]
    public Uri? RefToPdf { get; set; }

    #endregion

    #region Ссылка на аудиокнигу

    [DataType(DataType.Url)]
    [Display(Name = "Аудиокнига : ")]
    public Uri? RefToAudio { get; set; }

    #endregion
}