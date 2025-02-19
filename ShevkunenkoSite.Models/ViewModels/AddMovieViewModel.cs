namespace ShevkunenkoSite.Models.ViewModels;

using Microsoft.AspNetCore.Http;

public class AddMovieViewModel : MovieFileModel
{
    [DataType(DataType.Upload)]
    [Display(Name = "Файл фильма:")]
    public IFormFile? FileForMovieFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Полный вариант фильма:")]
    public IFormFile? FullMovieFormFile { get; set; }

    [Required(ErrorMessage = "Выберите картинку для фильма")]
    [DataType(DataType.Upload)]
    [Display(Name = "Картинка фильма:")]
    public IFormFile? ImageForMovieFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Постер фильма:")]
    public IFormFile? PosterForMovieFormFile { get; set; } = null;

    [DataType(DataType.Upload)]
    [Display(Name = "Картинка над сериями:")]
    public IFormFile? ImageHeadForSeriesFormFile { get; set; } = null;

    [DataType(DataType.Upload)]
    [Display(Name = "Выбрать статью :")]
    public IFormFile? ArticleAboutMovie1FormFile { get; set; } = null;

    [DataType(DataType.Text)]
    [Display(Name = "Страница серий:")]
    public string? PageForSeries { get; set; } = null;

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Темы видео :")]
    public List<TopicMovieModel> TopicsForMovie { get; set; } = [];

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Вид ссылок:")]
    public string ImageTypeForRef1 { get; set; } = string.Empty;

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Вид ссылок:")]
    public string ImageTypeForRef2 { get; set; } = string.Empty;

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Вид ссылок:")]
    public string ImageTypeForRef3 { get; set; } = string.Empty;
}