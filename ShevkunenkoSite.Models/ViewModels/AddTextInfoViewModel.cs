namespace ShevkunenkoSite.Models.ViewModels;

public class AddTextInfoViewModel : TextInfoModel
{
    public List<SelectListItem> TextDirectories { get; set; } = new([.. Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\wwwroot\\texts", "*", SearchOption.AllDirectories)
        .Select(a => new SelectListItem
        {
            Value = a[(Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\wwwroot\\texts")[0].IndexOf("texts") + 6)..].Replace('\\', '/'),
            Text = a[(Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\wwwroot\\texts")[0].IndexOf("texts") +6)..].Replace('\\', '/')
        })]);

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

    // связанная книга (статья)
    [DataType(DataType.Text)]
    [Display(Name = "Связанная книга (статья) :")]
    public string? RefForBookOrArticle { get; set; }
}