using Microsoft.AspNetCore.Http;

namespace ShevkunenkoSite.Models.ViewModels;

public class EditMovieViewModel : DetailsMovieViewModel
{
    [DataType(DataType.Upload)]
    [Display(Name = "файл фильма:")]
    public IFormFile? FileForMovieFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "полный вариант фильма:")]
    public IFormFile? FullMovieFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "картинка фильма:")]
    public IFormFile? ImageForMovieFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "постер фильма:")]
    public IFormFile? PosterForMovieFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Картинка страницы серий:")]
    public IFormFile? ImageHeadForSeriesFormFile { get; set; } = null;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Страница фильма:")]
    public string PageForMovie { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Страница серий:")]
    public string PageForSeriesString { get; set; } = string.Empty;

    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Темы видео:")]
    public List<TopicMovieModel> TopicsForMovie { get; set; } = new List<TopicMovieModel>();
}