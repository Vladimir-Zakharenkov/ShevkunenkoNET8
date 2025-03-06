namespace ShevkunenkoSite.Models.ViewModels;

public class AddAndEditArticleViewModel
{
    public BooksAndArticlesModel BookOrArticle { get; set; } = new();

    [DataType(DataType.Upload)]
    [Display(Name = "Выбрать логотип :")]
    public IFormFile? LogoOfArticleFormFile { get; set; }
}