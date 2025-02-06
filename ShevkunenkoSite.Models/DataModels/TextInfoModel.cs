namespace ShevkunenkoSite.Models.DataModels;

public class TextInfoModel
{
    [Display(Name = "TextInfoId:")]
    [Column("TextInfoId")]
    public Guid TextInfoModelId { get; set; }

    [Required(ErrorMessage = "Добавьте описание текста")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание текста :")]
    public string TextDescription { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Файл (txt) :")]
    public string TxtFileName { get; set; } = string.Empty;

    [Display(Name = "Размер файла (txt) :")]
    public int TxtFileSize { get; set; }

    [DataType(DataType.Text)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Display(Name = "Файл (html) :")]
    public string HtmlFileName { get; set; } = string.Empty;

    [Display(Name = "Размер файла (html) :")]
    public int HtmlFileSize { get; set; }
}