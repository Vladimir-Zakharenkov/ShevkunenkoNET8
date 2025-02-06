namespace ShevkunenkoSite.Models.ViewModels;

public class AddTextInfoViewModel : TextInfoModel
{
    [Required(ErrorMessage = "Выберите файл (txt)")]
    [DataType(DataType.Upload)]
    [Display(Name = "Файл (txt) :")]
    public IFormFile? TxtFileFormFile { get; set; }

    [Required(ErrorMessage = "Выберите файл (html)")]
    [DataType(DataType.Upload)]
    [Display(Name = "Файл (html) :")]
    public IFormFile? HtmlFileFormFile { get; set; }
}