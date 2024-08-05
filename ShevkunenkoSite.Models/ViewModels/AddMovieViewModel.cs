namespace ShevkunenkoSite.Models.ViewModels;

using Microsoft.AspNetCore.Http;

public class AddMovieViewModel : MovieFileModel
{
    [DataType(DataType.Upload)]
    [Display(Name = "Файл фильма:")]
    public IFormFile? FileForMovieFormFile { get; set; }

    [Required(ErrorMessage = "Выберите картинку для фильма")]
    [DataType(DataType.Upload)]
    [Display(Name = "Картинка фильма:")]
    public IFormFile? ImageForMovieFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Постер фильма:")]
    public IFormFile? PosterForMovieFormFile { get; set; } = null;

    [DataType(DataType.Upload)]
    [Display(Name = "Картинка заголовка страницы серий:")]
    public IFormFile? ImageHeadForSeriesFormFile { get; set; } = null;

    [DataType(DataType.Upload)]
    [Display(Name = "Полный вариант фильма:")]
    public IFormFile? FullMovieFormFile { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Страница фильма:")]
    public string? PageForMovie { get; set; } = null;

    [DataType(DataType.Text)]
    [Display(Name = "Страница серий:")]
    public string? PageForSeries { get; set; } = null;
}