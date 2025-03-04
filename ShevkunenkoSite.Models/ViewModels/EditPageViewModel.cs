namespace ShevkunenkoSite.Models.ViewModels;

public class EditPageViewModel : DetailsPageViewModel
{
    [DataType(DataType.Upload)]
    [Display(Name = "Картинка страницы :")]
    public IFormFile? ImageFileFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Фон страницы :")]
    public IFormFile? BackgroundFormFile { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Картинка под заголовком :")]
    public IFormFile? ImagePageHeadingFormFile { get; set; }
}