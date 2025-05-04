namespace ShevkunenkoSite.Models.ViewModels;

public class AddAndEditArticleViewModel
{
    public BooksAndArticlesModel BookOrArticle { get; set; } = new();

    [DataType(DataType.Text)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string PageForBookOrArticle { get; set; } = string.Empty;

    [DataType(DataType.Upload)]
    [Display(Name = "Выбрать логотип :")]
    public IFormFile? LogoOfArticleFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Выбрать скан :")]
    public IFormFile? ScanOfArticleFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Выбрать видео :")]
    public IFormFile? VideoForBookOrArticleFormFile { get; set; }
}