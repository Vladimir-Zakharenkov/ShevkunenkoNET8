namespace ShevkunenkoSite.Models.DataModels;

public class TextInfoModel
{
    [Display(Name = "TextInfoId:")]
    [Column("TextInfoId")]
    public Guid TextInfoModelId { get; set; }

    [Required(ErrorMessage = "Добавьте описание текста")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Описание текста :")]
    public string TextDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите текст")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Текст без разметки (txt) :")]
    public string ClearText { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите html текст")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Текст с разметкой (html) :")]
    public string HtmlText { get; set; } = string.Empty;
}