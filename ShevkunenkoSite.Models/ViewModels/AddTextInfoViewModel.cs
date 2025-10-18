namespace ShevkunenkoSite.Models.ViewModels;

public class AddTextInfoViewModel : TextInfoModel
{
    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Новый каталог :")]
    public string NewTextFolder { get; set; } = string.Empty;

    [Required(ErrorMessage = "Выберите файл (txt)")]
    [DataType(DataType.Upload)]
    [Display(Name = "Файл (txt) :")]
    public IFormFile? TxtFileFormFile { get; set; }

    [Required(ErrorMessage = "Выберите файл (html)")]
    [DataType(DataType.Upload)]
    [Display(Name = "Файл (html) :")]
    public IFormFile? HtmlFileFormFile { get; set; }
}